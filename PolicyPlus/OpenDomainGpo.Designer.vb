<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OpenDomainGpo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LblDomain = New System.Windows.Forms.Label()
        Me.TxtDomain = New System.Windows.Forms.TextBox()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        Me.LblGpos = New System.Windows.Forms.Label()
        Me.LstGpos = New System.Windows.Forms.ListBox()
        Me.LblPath = New System.Windows.Forms.Label()
        Me.TxtPath = New System.Windows.Forms.TextBox()
        Me.LblDetails = New System.Windows.Forms.Label()
        Me.TxtDetails = New System.Windows.Forms.TextBox()
        Me.BtnOk = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.LblUserPermissions = New System.Windows.Forms.Label()
        Me.LblWarning = New System.Windows.Forms.Label()
        Me.LblGpoCount = New System.Windows.Forms.Label()
        Me.BgWorker = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'LblDomain
        '
        Me.LblDomain.AutoSize = True
        Me.LblDomain.Location = New System.Drawing.Point(12, 15)
        Me.LblDomain.Name = "LblDomain"
        Me.LblDomain.Size = New System.Drawing.Size(51, 13)
        Me.LblDomain.TabIndex = 0
        Me.LblDomain.Text = "Domínio:"
        '
        'TxtDomain
        '
        Me.TxtDomain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtDomain.Location = New System.Drawing.Point(69, 12)
        Me.TxtDomain.Name = "TxtDomain"
        Me.TxtDomain.Size = New System.Drawing.Size(337, 20)
        Me.TxtDomain.TabIndex = 1
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnRefresh.Location = New System.Drawing.Point(412, 10)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(100, 23)
        Me.BtnRefresh.TabIndex = 2
        Me.BtnRefresh.Text = "Atualizar"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'LblGpos
        '
        Me.LblGpos.AutoSize = True
        Me.LblGpos.Location = New System.Drawing.Point(12, 100)
        Me.LblGpos.Name = "LblGpos"
        Me.LblGpos.Size = New System.Drawing.Size(209, 13)
        Me.LblGpos.TabIndex = 3
        Me.LblGpos.Text = "Objetos de Política de Grupo (GPOs):"
        '
        'LstGpos
        '
        Me.LstGpos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LstGpos.FormattingEnabled = True
        Me.LstGpos.Location = New System.Drawing.Point(15, 116)
        Me.LstGpos.Name = "LstGpos"
        Me.LstGpos.Size = New System.Drawing.Size(300, 199)
        Me.LstGpos.TabIndex = 4
        '
        'LblPath
        '
        Me.LblPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblPath.AutoSize = True
        Me.LblPath.Location = New System.Drawing.Point(12, 325)
        Me.LblPath.Name = "LblPath"
        Me.LblPath.Size = New System.Drawing.Size(48, 13)
        Me.LblPath.TabIndex = 5
        Me.LblPath.Text = "Caminho:"
        '
        'TxtPath
        '
        Me.TxtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtPath.Location = New System.Drawing.Point(15, 341)
        Me.TxtPath.Name = "TxtPath"
        Me.TxtPath.ReadOnly = True
        Me.TxtPath.Size = New System.Drawing.Size(650, 20)
        Me.TxtPath.TabIndex = 6
        '
        'LblDetails
        '
        Me.LblDetails.AutoSize = True
        Me.LblDetails.Location = New System.Drawing.Point(318, 100)
        Me.LblDetails.Name = "LblDetails"
        Me.LblDetails.Size = New System.Drawing.Size(52, 13)
        Me.LblDetails.TabIndex = 7
        Me.LblDetails.Text = "Detalhes:"
        '
        'TxtDetails
        '
        Me.TxtDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtDetails.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDetails.Location = New System.Drawing.Point(321, 116)
        Me.TxtDetails.Multiline = True
        Me.TxtDetails.Name = "TxtDetails"
        Me.TxtDetails.ReadOnly = True
        Me.TxtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtDetails.Size = New System.Drawing.Size(344, 199)
        Me.TxtDetails.TabIndex = 8
        '
        'BtnOk
        '
        Me.BtnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnOk.Enabled = False
        Me.BtnOk.Location = New System.Drawing.Point(509, 373)
        Me.BtnOk.Name = "BtnOk"
        Me.BtnOk.Size = New System.Drawing.Size(75, 23)
        Me.BtnOk.TabIndex = 9
        Me.BtnOk.Text = "OK"
        Me.BtnOk.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.Location = New System.Drawing.Point(590, 373)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 10
        Me.BtnCancel.Text = "Cancelar"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'LblStatus
        '
        Me.LblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LblStatus.AutoSize = True
        Me.LblStatus.Location = New System.Drawing.Point(280, 378)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(97, 13)
        Me.LblStatus.TabIndex = 11
        Me.LblStatus.Text = "Carregando GPOs..."
        Me.LblStatus.Visible = False
        '
        'LblUserPermissions
        '
        Me.LblUserPermissions.AutoSize = True
        Me.LblUserPermissions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUserPermissions.Location = New System.Drawing.Point(12, 45)
        Me.LblUserPermissions.Name = "LblUserPermissions"
        Me.LblUserPermissions.Size = New System.Drawing.Size(115, 13)
        Me.LblUserPermissions.TabIndex = 12
        Me.LblUserPermissions.Text = "Suas permissões:"
        '
        'LblWarning
        '
        Me.LblWarning.AutoSize = True
        Me.LblWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblWarning.Location = New System.Drawing.Point(12, 65)
        Me.LblWarning.MaximumSize = New System.Drawing.Size(650, 0)
        Me.LblWarning.Name = "LblWarning"
        Me.LblWarning.Size = New System.Drawing.Size(49, 13)
        Me.LblWarning.TabIndex = 13
        Me.LblWarning.Text = "Aviso"
        Me.LblWarning.Visible = False
        '
        'LblGpoCount
        '
        Me.LblGpoCount.AutoSize = True
        Me.LblGpoCount.Location = New System.Drawing.Point(227, 100)
        Me.LblGpoCount.Name = "LblGpoCount"
        Me.LblGpoCount.Size = New System.Drawing.Size(82, 13)
        Me.LblGpoCount.TabIndex = 14
        Me.LblGpoCount.Text = "Total de GPOs:"
        Me.LblGpoCount.Visible = False
        '
        'OpenDomainGpo
        '
        Me.AcceptButton = Me.BtnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(677, 408)
        Me.Controls.Add(Me.LblGpoCount)
        Me.Controls.Add(Me.LblWarning)
        Me.Controls.Add(Me.LblUserPermissions)
        Me.Controls.Add(Me.LblStatus)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnOk)
        Me.Controls.Add(Me.TxtDetails)
        Me.Controls.Add(Me.LblDetails)
        Me.Controls.Add(Me.TxtPath)
        Me.Controls.Add(Me.LblPath)
        Me.Controls.Add(Me.LstGpos)
        Me.Controls.Add(Me.LblGpos)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.TxtDomain)
        Me.Controls.Add(Me.LblDomain)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(693, 447)
        Me.Name = "OpenDomainGpo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Abrir GPO do Domínio - Active Directory"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblDomain As Label
    Friend WithEvents TxtDomain As TextBox
    Friend WithEvents BtnRefresh As Button
    Friend WithEvents LblGpos As Label
    Friend WithEvents LstGpos As ListBox
    Friend WithEvents LblPath As Label
    Friend WithEvents TxtPath As TextBox
    Friend WithEvents LblDetails As Label
    Friend WithEvents TxtDetails As TextBox
    Friend WithEvents BtnOk As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents LblStatus As Label
    Friend WithEvents LblUserPermissions As Label
    Friend WithEvents LblWarning As Label
    Friend WithEvents LblGpoCount As Label
    Friend WithEvents BgWorker As System.ComponentModel.BackgroundWorker
End Class
