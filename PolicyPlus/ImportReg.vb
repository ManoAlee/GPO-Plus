Public Class ImportReg
    Dim PolicySource As IPolicySource
    Public Function PresentDialog(Target As IPolicySource) As DialogResult
        TextReg.Text = ""
        TextRoot.Text = ""
        PolicySource = Target
        Return ShowDialog()
    End Function
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Registry scripts|*.reg"
            If ofd.ShowDialog() <> DialogResult.OK Then Exit Sub
            TextReg.Text = ofd.FileName
            If TextRoot.Text = "" Then
                Try
                    Dim reg = RegFile.Load(ofd.FileName, "")
                    TextRoot.Text = reg.GuessPrefix()
                    If reg.HasDefaultValues Then MsgBox("Este arquivo REG contem dados para valores padrao, que nao podem ser aplicados a todas as origens de politica.", MsgBoxStyle.Exclamation)
                Catch ex As Exception
                    MsgBox("Ocorreu um erro ao tentar adivinhar o prefixo.", MsgBoxStyle.Exclamation)
                End Try
            End If
        End Using
    End Sub
    Private Sub ImportReg_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        If TextReg.Text = "" Then
            MsgBox("Especifique um arquivo REG para importar.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If TextRoot.Text = "" Then
            MsgBox("Especifique o prefixo usado para qualificar completamente os caminhos no arquivo REG.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Try
            Dim reg = RegFile.Load(TextReg.Text, TextRoot.Text)
            reg.Apply(PolicySource)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MsgBox("Falha ao importar o arquivo REG.", MsgBoxStyle.Exclamation)
        End Try
    End Sub
End Class