Public Class SelectBackupDialog
    Inherits System.Windows.Forms.Form

    Private backups As List(Of String)
    Public SelectedBackup As String

    Private LstBackups As New System.Windows.Forms.ListBox()
    Private BtnOk As New System.Windows.Forms.Button()
    Private BtnCancel As New System.Windows.Forms.Button()

    Public Sub New(list As List(Of String))
        Me.backups = If(list, New List(Of String)())
        Me.Text = "Selecionar Backup"
        Me.ClientSize = New System.Drawing.Size(600, 300)
        InitializeComponents()
    End Sub

    Private Sub InitializeComponents()
        LstBackups.Left = 10
        LstBackups.Top = 10
        LstBackups.Width = 580
        LstBackups.Height = 220

        BtnOk.Text = "OK"
        BtnOk.Width = 100
        BtnOk.Left = 320
        BtnOk.Top = 240

        BtnCancel.Text = "Cancelar"
        BtnCancel.Width = 100
        BtnCancel.Left = 440
        BtnCancel.Top = 240

        Me.Controls.Add(LstBackups)
        Me.Controls.Add(BtnOk)
        Me.Controls.Add(BtnCancel)

        AddHandler Me.Load, AddressOf DialogLoaded
        AddHandler BtnOk.Click, AddressOf OnOk
        AddHandler BtnCancel.Click, AddressOf OnCancel
    End Sub

    Private Sub DialogLoaded(sender As Object, e As EventArgs)
        LstBackups.Items.Clear()
        For Each b In backups
            LstBackups.Items.Add(b)
        Next
        If LstBackups.Items.Count > 0 Then LstBackups.SelectedIndex = 0
    End Sub

    Private Sub OnOk(sender As Object, e As EventArgs)
        If LstBackups.SelectedIndex < 0 Then Return
        SelectedBackup = DirectCast(LstBackups.SelectedItem, String)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub OnCancel(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
