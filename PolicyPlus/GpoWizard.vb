Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
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
    Private BtnValidate As Button
    Private BtnExportReport As Button
    Private BtnOpenSysvol As Button
    Private BtnOpenBackups As Button
    Private BtnOpenLogs As Button
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

        BtnBackup = New Button() With {.Left = 340, .Top = 320, .Width = 90, .Text = "Backup", .Enabled = False}
        BtnRestore = New Button() With {.Left = 435, .Top = 320, .Width = 90, .Text = "Restaurar", .Enabled = False}
        BtnValidate = New Button() With {.Left = 530, .Top = 320, .Width = 150, .Text = "Validar/Afetados", .Enabled = False}

        BtnExportReport = New Button() With {.Left = 340, .Top = 350, .Width = 150, .Text = "Exportar Relatório...", .Enabled = False}
        BtnOpenSysvol = New Button() With {.Left = 500, .Top = 350, .Width = 180, .Text = "Abrir pasta SYSVOL", .Enabled = False}

        BtnOpenBackups = New Button() With {.Left = 10, .Top = 350, .Width = 320, .Text = "Abrir pasta de Backups", .Enabled = True}
        BtnOpenLogs = New Button() With {.Left = 10, .Top = 380, .Width = 320, .Text = "Abrir logs/auditoria", .Enabled = True}
        BtnClose = New Button() With {.Left = 580, .Top = 360, .Width = 100, .Text = "Fechar"}

        Me.Controls.AddRange(New Control() {TxtDomain, BtnRefresh, LblStatus, LstGpos, TxtPath, TxtDetails, BtnBackup, BtnRestore, BtnValidate, BtnExportReport, BtnOpenSysvol, BtnOpenBackups, BtnOpenLogs, BtnClose})

        BgWorker = New BackgroundWorker()
        BgWorker.WorkerSupportsCancellation = False
        AddHandler BgWorker.DoWork, AddressOf BgWorker_DoWork
        AddHandler BgWorker.RunWorkerCompleted, AddressOf BgWorker_RunWorkerCompleted

        AddHandler BtnRefresh.Click, Sub(s, e) LoadGpos()
        AddHandler LstGpos.SelectedIndexChanged, AddressOf LstGpos_SelectedIndexChanged
        AddHandler BtnBackup.Click, AddressOf BtnBackup_Click
        AddHandler BtnRestore.Click, AddressOf BtnRestore_Click
        AddHandler BtnValidate.Click, AddressOf BtnValidate_Click
        AddHandler BtnExportReport.Click, AddressOf BtnExportReport_Click
        AddHandler BtnOpenSysvol.Click, AddressOf BtnOpenSysvol_Click
        AddHandler BtnOpenBackups.Click, AddressOf BtnOpenBackups_Click
        AddHandler BtnOpenLogs.Click, AddressOf BtnOpenLogs_Click
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
            BtnValidate.Enabled = g.IsAccessible
            BtnExportReport.Enabled = g.IsAccessible
            BtnOpenSysvol.Enabled = g.IsAccessible
        Else
            TxtPath.Text = ""
            TxtDetails.Text = ""
            BtnBackup.Enabled = False
            BtnRestore.Enabled = False
            BtnValidate.Enabled = False
            BtnExportReport.Enabled = False
            BtnOpenSysvol.Enabled = False
        End If
    End Sub

    Private Sub BtnBackup_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
        Try
            Dim path = GpoBackupManager.BackupGpo(g)
            GpoAuditLogger.Log("UI", $"Backup acionado para GPO='{g.DisplayName}'")
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
                    GpoAuditLogger.Log("UI", $"Restauração acionada para GPO='{g.DisplayName}' Backup='{dlg.SelectedBackup}'")
                    MessageBox.Show("Restaurado com sucesso.", "Restaurar", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Falha ao restaurar: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub BtnValidate_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)

        Dim issues = GpoValidation.ValidateGpoStructure(g)
        Dim links = New List(Of String)
        Dim computers = New List(Of String)

        Try
            links = GpoValidation.GetGpoLinks(g.Domain, g.Guid)
        Catch ex As Exception
            issues.Add("Falha ao consultar vínculos (gPLink) no AD: " & ex.Message)
        End Try

        Try
            computers = GpoValidation.GetComputersUnderOus(g.Domain, links)
        Catch ex As Exception
            issues.Add("Falha ao listar computadores nas OUs vinculadas: " & ex.Message)
        End Try

        Dim sb As New StringBuilder()
        sb.AppendLine("Validação do GPO")
        sb.AppendLine("================")
        sb.AppendLine("Nome: " & g.DisplayName)
        sb.AppendLine("GUID: " & g.Guid)
        sb.AppendLine("SYSVOL: " & g.FileSystemPath)
        sb.AppendLine()

        If issues.Count = 0 Then
            sb.AppendLine("Estrutura: OK")
        Else
            sb.AppendLine("Problemas encontrados:")
            For Each it In issues
                sb.AppendLine("- " & it)
            Next
        End If

        sb.AppendLine()
        sb.AppendLine("Vínculos (OUs/Containers) encontrados: " & links.Count)
        For Each dn In links.Take(100)
            sb.AppendLine("- " & dn)
        Next
        If links.Count > 100 Then sb.AppendLine("... (mais " & (links.Count - 100) & ")")

        sb.AppendLine()
        sb.AppendLine("Computadores potencialmente afetados (estimativa): " & computers.Count)
        For Each c In computers.Take(200)
            sb.AppendLine("- " & c)
        Next
        If computers.Count > 200 Then sb.AppendLine("... (mais " & (computers.Count - 200) & ")")

        TxtDetails.Text = sb.ToString()
        GpoAuditLogger.Log("VALIDATE", $"GPO='{g.DisplayName}' GUID='{g.Guid}' Links={links.Count} Computers={computers.Count} Issues={issues.Count}")
        MessageBox.Show("Validação concluída. Veja os detalhes no painel à direita.", "Validador de GPO", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnExportReport_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Arquivos de texto|*.txt|CSV|*.csv"
            sfd.FileName = "Relatorio_GPO_" & SanitizeFileName(g.DisplayName) & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
            If sfd.ShowDialog(Me) <> DialogResult.OK Then Return
            Try
                Dim content = TxtDetails.Text
                If sfd.FilterIndex = 2 Then
                    content = ConvertReportToCsv(content)
                End If
                IO.File.WriteAllText(sfd.FileName, content, Encoding.UTF8)
                GpoAuditLogger.Log("EXPORT", $"GPO='{g.DisplayName}' -> '{sfd.FileName}'")
                MessageBox.Show("Relatório exportado com sucesso.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Falha ao exportar relatório: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub BtnOpenSysvol_Click(sender As Object, e As EventArgs)
        If LstGpos.SelectedIndex < 0 Then Return
        Dim g = DirectCast(LstGpos.SelectedItem, GpoInfo)
        Try
            If Not String.IsNullOrEmpty(g.FileSystemPath) Then
                Process.Start("explorer.exe", g.FileSystemPath)
            End If
        Catch ex As Exception
            MessageBox.Show("Falha ao abrir pasta: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnOpenBackups_Click(sender As Object, e As EventArgs)
        Try
            Dim root = GpoBackupManager.GetBackupRoot()
            Process.Start("explorer.exe", root)
        Catch ex As Exception
            MessageBox.Show("Falha ao abrir pasta de backups: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnOpenLogs_Click(sender As Object, e As EventArgs)
        Try
            Dim root = GpoAuditLogger.GetLogRoot()
            Process.Start("explorer.exe", root)
        Catch ex As Exception
            MessageBox.Show("Falha ao abrir logs: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Shared Function SanitizeFileName(name As String) As String
        If String.IsNullOrEmpty(name) Then Return "GPO"
        For Each ch In IO.Path.GetInvalidFileNameChars()
            name = name.Replace(ch, "_"c)
        Next
        Return name
    End Function

    Private Shared Function ConvertReportToCsv(text As String) As String
        ' Minimal export: each line becomes a row "Tipo";"Valor"
        Dim sb As New StringBuilder()
        sb.AppendLine("Campo;Valor")
        For Each line In text.Split({vbCrLf, vbLf}, StringSplitOptions.None)
            If String.IsNullOrWhiteSpace(line) Then Continue For
            Dim parts = line.Split({":"c}, 2)
            If parts.Length = 2 Then
                sb.AppendLine(EscapeCsv(parts(0).Trim()) & ";" & EscapeCsv(parts(1).Trim()))
            Else
                sb.AppendLine("Linha;" & EscapeCsv(line.Trim()))
            End If
        Next
        Return sb.ToString()
    End Function

    Private Shared Function EscapeCsv(value As String) As String
        If value Is Nothing Then Return ""
        If value.Contains(";") OrElse value.Contains(ChrW(34)) OrElse value.Contains(vbCr) OrElse value.Contains(vbLf) Then
            Return ChrW(34) & value.Replace(ChrW(34), ChrW(34) & ChrW(34)) & ChrW(34)
        End If
        Return value
    End Function
End Class
