Public Class OpenDomainGpo
    Private selectedGpo As GpoInfo
    Private domainName As String
    Private userPermissions As String
    Private isDomainAdmin As Boolean

    Private Sub OpenDomainGpo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Verificar permissões do usuário
        isDomainAdmin = AdGpoManager.IsUserDomainAdmin()
        userPermissions = AdGpoManager.GetUserPermissionLevel()

        ' Mostrar permissões do usuário
        LblUserPermissions.Text = $"Suas permissões: {userPermissions}"

        If Not isDomainAdmin Then
            LblUserPermissions.ForeColor = Color.DarkOrange
            LblWarning.Text = "⚠️ AVISO: Você não tem permissões de administrador. Os GPOs serão abertos em modo SOMENTE LEITURA."
            LblWarning.Visible = True
            LblWarning.ForeColor = Color.Red
        Else
            LblUserPermissions.ForeColor = Color.DarkGreen
            LblWarning.Text = "✓ Você tem permissões de administrador. Pode editar os GPOs."
            LblWarning.Visible = True
            LblWarning.ForeColor = Color.DarkGreen
        End If

        ' Obter domínio atual
        domainName = AdGpoManager.GetCurrentDomain()

        If String.IsNullOrEmpty(domainName) Then
            MsgBox("Esta máquina não está em um domínio do Active Directory.", MsgBoxStyle.Exclamation)
            TxtDomain.Text = ""
        Else
            TxtDomain.Text = domainName
            LoadGpos()
        End If
    End Sub
    
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        LoadGpos()
    End Sub
    
    Private Sub LoadGpos()
        domainName = TxtDomain.Text.Trim()
        
        If String.IsNullOrEmpty(domainName) Then
            MsgBox("Por favor, digite o nome do domínio.", MsgBoxStyle.Exclamation)
            Return
        End If
        
        LstGpos.Items.Clear()
        LblStatus.Text = "Carregando GPOs..."
        LblStatus.Visible = True
        BtnRefresh.Enabled = False
        BtnOk.Enabled = False
        
        ' Usar um BackgroundWorker para não travar a UI
        BgWorker.RunWorkerAsync(domainName)
    End Sub
    
    Private Sub BgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BgWorker.DoWork
        Try
            Dim domain = e.Argument.ToString()
            e.Result = AdGpoManager.GetDomainGpos(domain)
        Catch ex As Exception
            e.Result = ex
        End Try
    End Sub
    
    Private Sub BgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BgWorker.RunWorkerCompleted
        LblStatus.Visible = False
        BtnRefresh.Enabled = True
        
        If TypeOf e.Result Is Exception Then
            MsgBox("Erro ao carregar GPOs: " & DirectCast(e.Result, Exception).Message, MsgBoxStyle.Critical)
            Return
        End If
        
        Dim gpoList = DirectCast(e.Result, List(Of GpoInfo))
        
        If gpoList.Count = 0 Then
            MsgBox("Nenhum GPO encontrado no domínio.", MsgBoxStyle.Information)
            Return
        End If

        ' Adicionar contador
        LblGpoCount.Text = $"Total de GPOs encontrados: {gpoList.Count}"
        LblGpoCount.Visible = True

        For Each gpo In gpoList
            ' Adicionar ícone visual para indicar acessibilidade
            Dim displayText = gpo.DisplayName
            If Not gpo.IsAccessible Then
                displayText = "⚠️ " & displayText
            End If
            LstGpos.Items.Add(gpo)
        Next

        If LstGpos.Items.Count > 0 Then
            LstGpos.SelectedIndex = 0
        End If
    End Sub
    
    Private Sub LstGpos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstGpos.SelectedIndexChanged
        BtnOk.Enabled = LstGpos.SelectedIndex >= 0

        If LstGpos.SelectedIndex >= 0 Then
            Dim gpo = DirectCast(LstGpos.SelectedItem, GpoInfo)
            TxtPath.Text = gpo.FileSystemPath

            ' Mostrar informações detalhadas
            TxtDetails.Text = gpo.GetDetailedInfo()

            ' Adicionar informação sobre permissão de escrita
            If gpo.IsAccessible Then
                Dim manager As New AdGpoManager(gpo.Domain, gpo.DisplayName, gpo.Guid)
                Dim canWrite = manager.CanWrite()

                If canWrite Then
                    TxtDetails.Text &= vbCrLf & "Permissão de Escrita: Sim ✓"
                Else
                    TxtDetails.Text &= vbCrLf & "Permissão de Escrita: Não (Somente Leitura)"
                End If
            Else
                TxtDetails.Text &= vbCrLf & "⚠️ GPO não acessível no sistema de arquivos"
            End If
        Else
            TxtPath.Text = ""
            TxtDetails.Text = ""
        End If
    End Sub
    
    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        If LstGpos.SelectedIndex < 0 Then
            MsgBox("Por favor, selecione um GPO.", MsgBoxStyle.Exclamation)
            Return
        End If

        selectedGpo = DirectCast(LstGpos.SelectedItem, GpoInfo)

        ' Verificar se o GPO é acessível
        If Not selectedGpo.IsAccessible Then
            MsgBox($"O GPO '{selectedGpo.DisplayName}' não está acessível no sistema de arquivos:{vbCrLf}{selectedGpo.FileSystemPath}{vbCrLf}{vbCrLf}" &
                   "Verifique se você tem acesso à rede e permissões adequadas.", MsgBoxStyle.Critical)
            Return
        End If

        Dim manager As New AdGpoManager(selectedGpo.Domain, selectedGpo.DisplayName, selectedGpo.Guid)

        ' Verificar permissão de escrita
        Dim canWrite = manager.CanWrite()
        Dim mode = If(canWrite, "EDIÇÃO", "SOMENTE LEITURA")

        If Not canWrite Then
            Dim result = MsgBox($"ATENÇÃO: Você não tem permissão de escrita neste GPO.{vbCrLf}{vbCrLf}" &
                              $"O GPO '{selectedGpo.DisplayName}' será aberto em modo SOMENTE LEITURA.{vbCrLf}{vbCrLf}" &
                              "Você poderá visualizar as configurações mas não poderá salvá-las.{vbCrLf}{vbCrLf}" &
                              "Deseja continuar?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo)

            If result = MsgBoxResult.No Then
                Return
            End If
        Else
            ' Confirmar edição
            Dim result = MsgBox($"Você vai abrir o GPO: {selectedGpo.DisplayName}{vbCrLf}{vbCrLf}" &
                              $"Modo: {mode}{vbCrLf}" &
                              $"Domínio: {selectedGpo.Domain}{vbCrLf}{vbCrLf}" &
                              "Qualquer alteração será aplicada ao domínio inteiro!{vbCrLf}{vbCrLf}" &
                              "Deseja continuar?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo)

            If result = MsgBoxResult.No Then
                Return
            End If
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub
    
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
    
    Public Function GetSelectedGpo() As GpoInfo
        Return selectedGpo
    End Function
End Class
