Imports System.IO

Public Class GpoBackupManager
    Public Shared Function GetBackupRoot() As String
        Dim root = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\GPO-Plus\Backups")
        If Not Directory.Exists(root) Then Directory.CreateDirectory(root)
        Return root
    End Function

    Public Shared Function BackupGpo(gpo As GpoInfo) As String
        ' Create a timestamped folder and copy files recursively
        Dim root = GetBackupRoot()
        Dim dest = Path.Combine(root, gpo.Domain, gpo.Guid, DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"))
        Directory.CreateDirectory(dest)
        If String.IsNullOrEmpty(gpo.FileSystemPath) OrElse Not Directory.Exists(gpo.FileSystemPath) Then
            Throw New DirectoryNotFoundException("Caminho do GPO não encontrado: " & gpo.FileSystemPath)
        End If
        CopyDirectory(gpo.FileSystemPath, dest)
        Return dest
    End Function

    Public Shared Sub RestoreGpoFromBackup(backupPath As String, targetGpoPath As String)
        If Not Directory.Exists(backupPath) Then Throw New DirectoryNotFoundException("Backup não encontrado: " & backupPath)
        If Not Directory.Exists(targetGpoPath) Then Directory.CreateDirectory(targetGpoPath)
        ' Copy to a temporary folder then move to target to reduce partial states
        Dim tmp = targetGpoPath & ".tmp_restore_" & Guid.NewGuid().ToString()
        If Directory.Exists(tmp) Then Directory.Delete(tmp, True)
        Directory.CreateDirectory(tmp)
        CopyDirectory(backupPath, tmp)
        ' Try to replace files
        For Each srcFile In Directory.GetFiles(tmp, "*", SearchOption.AllDirectories)
            Dim rel = srcFile.Substring(tmp.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Dim destFile = Path.Combine(targetGpoPath, rel)
            Dim destDir = Path.GetDirectoryName(destFile)
            If Not Directory.Exists(destDir) Then Directory.CreateDirectory(destDir)
            File.Copy(srcFile, destFile, True)
        Next
        Directory.Delete(tmp, True)
    End Sub

    Public Shared Function ListBackups(gpo As GpoInfo) As List(Of String)
        Dim list As New List(Of String)
        Dim root = GetBackupRoot()
        Dim backupPath As String = Path.Combine(root, gpo.Domain, gpo.Guid)
        If Directory.Exists(backupPath) Then
            For Each d In Directory.GetDirectories(backupPath)
                list.Add(d)
            Next
            list.Sort()
        End If
        Return list
    End Function

    Private Shared Sub CopyDirectory(sourceDir As String, destDir As String)
        For Each subDir In Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories)
            Dim relative = subDir.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Directory.CreateDirectory(Path.Combine(destDir, relative))
        Next
        For Each srcFile In Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories)
            Dim relative = srcFile.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Dim destFile = Path.Combine(destDir, relative)
            Dim destFolder = Path.GetDirectoryName(destFile)
            If Not Directory.Exists(destFolder) Then Directory.CreateDirectory(destFolder)
            System.IO.File.Copy(srcFile, destFile, True)
        Next
    End Sub
End Class
