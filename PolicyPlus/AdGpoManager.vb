Imports System.DirectoryServices
Imports System.IO
Imports System.Security.Principal
Imports Microsoft.Win32

Public Class AdGpoManager
    Private domainName As String
    Private gpoPath As String
    Private gpoDisplayName As String
    Private gpoGuid As String

    Public Sub New(domain As String, gpoName As String, gpoId As String)
        domainName = domain
        gpoDisplayName = gpoName
        gpoGuid = gpoId

        ' Construir caminho UNC para o GPO
        ' Formato: \\domain\SYSVOL\domain\Policies\{GUID}
        gpoPath = $"\\{domain}\SYSVOL\{domain}\Policies\{gpoId}"
    End Sub
    
    Public Function GetPolFilePath(isUser As Boolean) As String
        Dim section = If(isUser, "User", "Machine")
        Return Path.Combine(gpoPath, section, "Registry.pol")
    End Function
    
    Public Function GetGptIniPath() As String
        Return Path.Combine(gpoPath, "gpt.ini")
    End Function
    
    Public Function IsAccessible() As Boolean
        Try
            Return Directory.Exists(gpoPath)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function CanWrite() As Boolean
        Try
            ' Tentar criar um arquivo temporário para verificar permissão de escrita
            Dim testPath = Path.Combine(gpoPath, "_test_write_" & Guid.NewGuid().ToString() & ".tmp")
            File.WriteAllText(testPath, "test")
            File.Delete(testPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function IsUserDomainAdmin() As Boolean
        Try
            Dim identity = WindowsIdentity.GetCurrent()
            Dim principal As New WindowsPrincipal(identity)

            ' Verificar se é administrador local (pode ser admin de domínio)
            If principal.IsInRole(WindowsBuiltInRole.Administrator) Then
                ' Tentar obter mais informações sobre grupos do domínio
                Try
                    Dim domainName = GetCurrentDomain()
                    If String.IsNullOrEmpty(domainName) Then Return False

                    ' Verificar grupos do usuário
                    For Each group In identity.Groups
                        Try
                            Dim sid = group.Translate(GetType(NTAccount)).ToString()
                            ' Verificar se é Domain Admins ou Enterprise Admins
                            If sid.ToLower().Contains("domain admins") OrElse 
                               sid.ToLower().Contains("enterprise admins") OrElse
                               sid.ToLower().Contains("administradores do domínio") OrElse
                               sid.ToLower().Contains("administradores de empresa") Then
                                Return True
                            End If
                        Catch
                            ' Ignorar erros de tradução de SID
                        End Try
                    Next
                Catch
                    ' Se falhar, retornar True se for admin local
                    Return True
                End Try
            End If

            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function GetUserPermissionLevel() As String
        Try
            Dim identity = WindowsIdentity.GetCurrent()
            Dim principal As New WindowsPrincipal(identity)
            Dim permissions As New List(Of String)

            If principal.IsInRole(WindowsBuiltInRole.Administrator) Then
                permissions.Add("Administrador Local")
            End If

            ' Verificar grupos do domínio
            For Each group In identity.Groups
                Try
                    Dim sid = group.Translate(GetType(NTAccount)).ToString()
                    Dim sidLower = sid.ToLower()

                    If sidLower.Contains("domain admins") OrElse sidLower.Contains("administradores do domínio") Then
                        permissions.Add("Administrador de Domínio")
                    End If

                    If sidLower.Contains("enterprise admins") OrElse sidLower.Contains("administradores de empresa") Then
                        permissions.Add("Administrador Corporativo")
                    End If

                    If sidLower.Contains("group policy creator") OrElse sidLower.Contains("criadores") Then
                        permissions.Add("Criador de Políticas de Grupo")
                    End If
                Catch
                    ' Ignorar erros
                End Try
            Next

            If permissions.Count = 0 Then
                Return "Usuário Padrão (Somente Leitura)"
            Else
                Return String.Join(", ", permissions)
            End If
        Catch ex As Exception
            Return "Desconhecido"
        End Try
    End Function
    
    Public Shared Function GetDomainGpos(domain As String) As List(Of GpoInfo)
        Dim gpoList As New List(Of GpoInfo)

        Try
            ' Conectar ao Active Directory
            Dim dcName = domain.Split("."c)(0).ToUpper()
            Dim domainDN = "DC=" & domain.Replace(".", ",DC=")
            Dim ldapPath As String = $"LDAP://{domain}/CN=Policies,CN=System,{domainDN}"

            Dim entry As New DirectoryEntry(ldapPath)
            Dim searcher As New DirectorySearcher(entry)

            ' Buscar todos os GPOs
            searcher.Filter = "(objectClass=groupPolicyContainer)"
            searcher.PropertiesToLoad.Add("displayName")
            searcher.PropertiesToLoad.Add("name")
            searcher.PropertiesToLoad.Add("gPCFileSysPath")
            searcher.PropertiesToLoad.Add("whenCreated")
            searcher.PropertiesToLoad.Add("whenChanged")
            searcher.PropertiesToLoad.Add("versionNumber")
            searcher.PageSize = 1000 ' Para suportar muitos GPOs

            Dim results = searcher.FindAll()

            For Each result As SearchResult In results
                Dim gpoInfo As New GpoInfo()

                ' Nome do GPO
                If result.Properties.Contains("displayName") AndAlso result.Properties("displayName").Count > 0 Then
                    gpoInfo.DisplayName = result.Properties("displayName")(0).ToString()
                Else
                    gpoInfo.DisplayName = "Sem nome"
                End If

                ' GUID do GPO
                If result.Properties.Contains("name") AndAlso result.Properties("name").Count > 0 Then
                    gpoInfo.Guid = result.Properties("name")(0).ToString()
                End If

                ' Caminho do sistema de arquivos
                If result.Properties.Contains("gPCFileSysPath") AndAlso result.Properties("gPCFileSysPath").Count > 0 Then
                    gpoInfo.FileSystemPath = result.Properties("gPCFileSysPath")(0).ToString()
                End If

                ' Data de criação
                If result.Properties.Contains("whenCreated") AndAlso result.Properties("whenCreated").Count > 0 Then
                    gpoInfo.CreatedDate = CDate(result.Properties("whenCreated")(0))
                End If

                ' Data de modificação
                If result.Properties.Contains("whenChanged") AndAlso result.Properties("whenChanged").Count > 0 Then
                    gpoInfo.ModifiedDate = CDate(result.Properties("whenChanged")(0))
                End If

                ' Versão
                If result.Properties.Contains("versionNumber") AndAlso result.Properties("versionNumber").Count > 0 Then
                    gpoInfo.Version = CInt(result.Properties("versionNumber")(0))
                End If

                gpoInfo.Domain = domain

                ' Verificar se existe no sistema de arquivos
                gpoInfo.IsAccessible = Directory.Exists(gpoInfo.FileSystemPath)

                gpoList.Add(gpoInfo)
            Next

            ' Ordenar por nome
            gpoList = gpoList.OrderBy(Function(g) g.DisplayName).ToList()

        Catch ex As Exception
            Throw New Exception($"Erro ao conectar ao domínio {domain}: {ex.Message}", ex)
        End Try

        Return gpoList
    End Function
    
    Public Shared Function GetCurrentDomain() As String
        Try
            Return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName
        Catch ex As Exception
            Return ""
        End Try
    End Function
    
    Public ReadOnly Property DisplayName As String
        Get
            Return gpoDisplayName
        End Get
    End Property
    
    Public ReadOnly Property GpoIdentifier As String
        Get
            Return gpoGuid
        End Get
    End Property

    Public ReadOnly Property Domain As String
        Get
            Return domainName
        End Get
    End Property
End Class

Public Class GpoInfo
    Public Property DisplayName As String
    Public Property Guid As String
    Public Property FileSystemPath As String
    Public Property Domain As String
    Public Property CreatedDate As Date?
    Public Property ModifiedDate As Date?
    Public Property Version As Integer
    Public Property IsAccessible As Boolean

    Public Overrides Function ToString() As String
        Return DisplayName
    End Function

    Public Function GetDetailedInfo() As String
        Dim info As New Text.StringBuilder()
        info.AppendLine($"Nome: {DisplayName}")
        info.AppendLine($"GUID: {Guid}")
        info.AppendLine($"Domínio: {Domain}")
        info.AppendLine($"Caminho: {FileSystemPath}")

        If CreatedDate.HasValue Then
            info.AppendLine($"Criado em: {CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")}")
        End If

        If ModifiedDate.HasValue Then
            info.AppendLine($"Modificado em: {ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")}")
        End If

        info.AppendLine($"Versão: {Version}")
        info.AppendLine($"Acessível: {If(IsAccessible, "Sim", "Não")}")

        Return info.ToString()
    End Function
End Class
