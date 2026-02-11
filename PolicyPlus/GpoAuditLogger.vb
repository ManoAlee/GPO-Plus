Imports System.IO
Imports System.Text

Public Class GpoAuditLogger
    Public Shared Function GetLogRoot() As String
        Dim root = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\GPO-Plus\Logs")
        If Not Directory.Exists(root) Then Directory.CreateDirectory(root)
        Return root
    End Function

    Public Shared Function GetLogFilePath() As String
        Return Path.Combine(GetLogRoot(), "audit.log")
    End Function

    Public Shared Sub Log(actionName As String, details As String)
        Try
            Dim line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " | " & Environment.UserName & " | " & actionName & " | " & details
            File.AppendAllText(GetLogFilePath(), line & Environment.NewLine, Encoding.UTF8)
        Catch
        End Try
    End Sub
End Class
