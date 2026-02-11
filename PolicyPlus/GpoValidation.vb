Imports System.DirectoryServices
Imports System.IO

Public Class GpoValidation
    Public Shared Function ValidateGpoStructure(gpo As GpoInfo) As List(Of String)
        Dim issues As New List(Of String)

        If gpo Is Nothing Then
            issues.Add("GPO não selecionado.")
            Return issues
        End If

        If String.IsNullOrEmpty(gpo.FileSystemPath) Then
            issues.Add("Caminho do SYSVOL está vazio.")
            Return issues
        End If

        If Not Directory.Exists(gpo.FileSystemPath) Then
            issues.Add("Pasta do GPO não existe no SYSVOL: " & gpo.FileSystemPath)
            Return issues
        End If

        Dim gptIni = Path.Combine(gpo.FileSystemPath, "gpt.ini")
        If Not File.Exists(gptIni) Then
            issues.Add("Arquivo gpt.ini não encontrado.")
        End If

        Dim userPol = Path.Combine(gpo.FileSystemPath, "User", "Registry.pol")
        Dim compPol = Path.Combine(gpo.FileSystemPath, "Machine", "Registry.pol")

        If Not File.Exists(userPol) AndAlso Not File.Exists(compPol) Then
            issues.Add("Nenhum Registry.pol encontrado (User/Machine).")
        End If

        Return issues
    End Function

    Public Shared Function GetGpoLinks(domain As String, gpoGuid As String) As List(Of String)
        Dim links As New List(Of String)

        If String.IsNullOrEmpty(domain) OrElse String.IsNullOrEmpty(gpoGuid) Then Return links

        Dim domainDN = "DC=" & domain.Replace(".", ",DC=")
        Dim ldapPath = $"LDAP://{domain}/{domainDN}"

        Using root As New DirectoryEntry(ldapPath)
            Using searcher As New DirectorySearcher(root)
                searcher.Filter = "(gPLink=*)"
                searcher.PropertiesToLoad.Add("distinguishedName")
                searcher.PropertiesToLoad.Add("gPLink")
                searcher.PageSize = 1000

                For Each res As SearchResult In searcher.FindAll()
                    If Not res.Properties.Contains("gPLink") Then Continue For
                    Dim gplink = res.Properties("gPLink")(0).ToString()
                    If gplink.IndexOf(gpoGuid, StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Dim dn = If(res.Properties.Contains("distinguishedName"), res.Properties("distinguishedName")(0).ToString(), "(sem DN)")
                        links.Add(dn)
                    End If
                Next
            End Using
        End Using

        Return links
    End Function

    Public Shared Function GetComputersUnderOus(domain As String, ous As IEnumerable(Of String), Optional maxComputers As Integer = 2000) As List(Of String)
        Dim computers As New List(Of String)
        If String.IsNullOrEmpty(domain) Then Return computers

        Dim domainDN = "DC=" & domain.Replace(".", ",DC=")
        Dim ldapBase = $"LDAP://{domain}/"

        Dim ouList = ous?.ToList()
        If ouList Is Nothing OrElse ouList.Count = 0 Then
            ouList = New List(Of String) From {domainDN}
        End If

        For Each baseDn In ouList
            Using entry As New DirectoryEntry(ldapBase & baseDn)
                Using searcher As New DirectorySearcher(entry)
                    searcher.Filter = "(&(objectClass=computer)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))"
                    searcher.PropertiesToLoad.Add("name")
                    searcher.PageSize = 1000
                    For Each res As SearchResult In searcher.FindAll()
                        If res.Properties.Contains("name") Then
                            computers.Add(res.Properties("name")(0).ToString())
                            If computers.Count >= maxComputers Then Return computers
                        End If
                    Next
                End Using
            End Using
        Next

        computers.Sort()
        Return computers
    End Function
End Class
