Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Public Class GpoWizard
    Inherits Form

    Private TxtDomain As TextBox
    Private BtnRefresh As Button
    Private LstGpos As ListBox
    Private TxtPath As TextBox
    Private TxtDetails As TextBox
    Private BtnBackup As Button
    Private BtnRestore As Button
    Private BtnClose As Button
    Private LblStatus As Label
    Private BgWorker As BackgroundWorker
    Private currentGpos As List(Of GpoInfo)

    Public Sub New()
        Me.Text = "Assistente de GPOs do Domínio"
        Me.ClientSize = New Size(700, 420)
        InitializeComponents()
    End Sub

    Private Sub InitializeComponents()
        TxtDomain = New TextBox() With {.Left = 10, .Top = 10, .Width = 420}
        BtnRefresh = New Button() With {.Left = 440, .Top = 8, .Text = "Atualizar", .Width = 80}
        LblStatus = New Label() With {.Left = 530, .Top = 12, .AutoSize = True, .Text = ""}

        LstGpos = New ListBox() With {.Left = 10, .Top = 40, .Width = 320, .Height = 300}
        TxtPath = New TextBox() With {.Left = 340, .Top = 40, .Width = 340, .ReadOnly = True}
        TxtDetails = New TextBox() With {.Left = 340, .Top = 68, .Width = 340, .Height = 240, .Multiline = True, .ReadOnly = True, .ScrollBars = ScrollBars.Vertical}

        BtnBackup = New Button() With {.Left = 340, .Top = 320, .Width = 100, .Text = "Backup", .Enabled = False}
        BtnRestore = New Button() With {.Left = 450, .Top = 320, .Width = 100, .Text = "Restaurar", .Enabled = False}
        BtnClose = New Button() With {.Left = 580, .Top = 360, .Width = 100, .Text = "Fechar"}

        Me.Controls.AddRange(New Control() {TxtDomain, BtnRefresh, LblStatus, LstGpos, TxtPath, TxtDetails, BtnBackup, BtnRestore, BtnClose})

        BgWorker = New BackgroundWorker()
        BgWorker.WorkerSupportsCancellation = False
        AddHandler BgWorker.DoWork, AddressOf BgWorker_DoWork
        AddHandler BgWorker.RunWorkerCompleted, AddressOf BgWorker_RunWorkerCompleted

        AddHandler BtnRefresh.Click, Sub(s, e) LoadGpos()
        AddHandler LstGpos.SelectedIndexChanged, AddressOf LstGpos_SelectedIndexChanged
        AddHandler BtnBackup.Click, AddressOf BtnBackup_Click
        AddHandler BtnRestore.Click, AddressOf BtnRestore_Click
        AddHandler BtnClose.Click, Sub(s, e) Me.Close()

        TxtDomain.Text = AdGpoManager.GetCurrentDomain()
        LoadGpos()
    End Sub

    Private Sub LoadGpos()
        Dim domainName = TxtDomain.Text.Trim()
        If String.IsNullOrEmpty(domainName) Then
            MessageBox.Show("Por favor, digite o nome do domínio.", "Domínio inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        LstGpos.Items.Clear()
        LblStatus.Text = "Carregando GPOs..."
        BtnRefresh.Enabled = False
        BtnBackup.Enabled = False
        BtnRestore.Enabled = False
        If Not BgWorker.IsBusy Then BgWorker.RunWorkerAsync(domainName)
    End Sub

    Private Sub BgWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Try
            Dim domain = e.Argument.ToString()
            e.Result = AdGpoManager.GetDomainGpos(domain)
        Catch ex As Exception
            e.Result = ex
        End Try
    End Sub

    Private Sub BgWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        LblStatus.Text = ""
        BtnRefresh.Enabled = True
        If TypeOf e.Result Is Exception Then
            MessageBox.Show("Erro ao carregar GPOs: " & DirectCast(e.Result, Exception).Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        currentGpos = DirectCast(e.Result, List(Of GpoInfo))
        LstGpos.Items.Clear()
        For Each g In currentGpos
            LstGpos.Items.Add(g)
        Next
        If LstGpos.Items.Count > 0 Then LstGpos.SelectedIndex = 0
    End Sub

    Private Sub LstGpos_SelectedIndexChanged(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex >= 0 Then
            Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
            TxtPath.Text = g.FileSystemPath
            TxtDetails.Text = g.GetDetailedInfo()
            BtnBackup.Enabled = g.IsAccessible
            BtnRestore.Enabled = False
        Else
            TxtPath.Text = ""
            TxtDetails.Text = ""
            BtnBackup.Enabled = False
            BtnRestore.Enabled = False
        End If
    End Sub

    Private Sub BtnBackup_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
        Try
            Dim path = GpoBackupManager.BackupGpo(g)
            MessageBox.Show("Backup criado em: " & path, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
            BtnRestore.Enabled = True
        Catch ex As Exception
            MessageBox.Show("Falha ao criar backup: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnRestore_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
        Dim backups = GpoBackupManager.ListBackups(g)
        If backups.Count = 0 Then
            MessageBox.Show("Nenhum backup encontrado para este GPO.", "Restaurar", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Using dlg As New SelectBackupDialog(backups)
            If dlg.ShowDialog() = DialogResult.OK Then
                Try
                    GpoBackupManager.RestoreGpoFromBackup(dlg.SelectedBackup, g.FileSystemPath)
                    MessageBox.Show("Restaurado com sucesso.", "Restaurar", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Falha ao restaurar: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
End Class
