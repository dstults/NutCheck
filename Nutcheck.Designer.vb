<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Nutcheck
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.txtIpAddress = New System.Windows.Forms.TextBox()
        Me.lblPingReply = New System.Windows.Forms.Label()
        Me.lblPing = New System.Windows.Forms.Label()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.lblTCP = New System.Windows.Forms.Label()
        Me.lblTcpReply = New System.Windows.Forms.Label()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPingTimeout = New System.Windows.Forms.TextBox()
        Me.chkPing = New System.Windows.Forms.CheckBox()
        Me.chkTcp = New System.Windows.Forms.CheckBox()
        Me.lblAbout = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.chkUdp = New System.Windows.Forms.CheckBox()
        Me.lblSpinner = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btnTest.Location = New System.Drawing.Point(440, 20)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(137, 70)
        Me.btnTest.TabIndex = 0
        Me.btnTest.Text = "RUN TEST"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'txtIpAddress
        '
        Me.txtIpAddress.Location = New System.Drawing.Point(52, 30)
        Me.txtIpAddress.Name = "txtIpAddress"
        Me.txtIpAddress.Size = New System.Drawing.Size(100, 20)
        Me.txtIpAddress.TabIndex = 1
        Me.txtIpAddress.Text = "yahoo.com"
        Me.txtIpAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPingReply
        '
        Me.lblPingReply.Location = New System.Drawing.Point(158, 30)
        Me.lblPingReply.Name = "lblPingReply"
        Me.lblPingReply.Size = New System.Drawing.Size(100, 20)
        Me.lblPingReply.TabIndex = 2
        Me.lblPingReply.Text = "-"
        Me.lblPingReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPing
        '
        Me.lblPing.Location = New System.Drawing.Point(158, 10)
        Me.lblPing.Name = "lblPing"
        Me.lblPing.Size = New System.Drawing.Size(100, 20)
        Me.lblPing.TabIndex = 18
        Me.lblPing.Text = "PING"
        Me.lblPing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(264, 31)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(67, 20)
        Me.txtPort.TabIndex = 19
        Me.txtPort.Text = "80"
        Me.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblTCP
        '
        Me.lblTCP.Location = New System.Drawing.Point(337, 8)
        Me.lblTCP.Name = "lblTCP"
        Me.lblTCP.Size = New System.Drawing.Size(100, 20)
        Me.lblTCP.TabIndex = 20
        Me.lblTCP.Text = "TCP"
        Me.lblTCP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTcpReply
        '
        Me.lblTcpReply.Location = New System.Drawing.Point(337, 31)
        Me.lblTcpReply.Name = "lblTcpReply"
        Me.lblTcpReply.Size = New System.Drawing.Size(100, 20)
        Me.lblTcpReply.TabIndex = 21
        Me.lblTcpReply.Text = "-"
        Me.lblTcpReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPort
        '
        Me.lblPort.Location = New System.Drawing.Point(261, 8)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(70, 20)
        Me.lblPort.TabIndex = 23
        Me.lblPort.Text = "PORT"
        Me.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblIP
        '
        Me.lblIP.Location = New System.Drawing.Point(52, 8)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(100, 20)
        Me.lblIP.TabIndex = 22
        Me.lblIP.Text = "IP"
        Me.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(5, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 20)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Ping Timeout:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPingTimeout
        '
        Me.txtPingTimeout.Location = New System.Drawing.Point(84, 78)
        Me.txtPingTimeout.Name = "txtPingTimeout"
        Me.txtPingTimeout.Size = New System.Drawing.Size(48, 20)
        Me.txtPingTimeout.TabIndex = 25
        Me.txtPingTimeout.Text = "500"
        Me.txtPingTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkPing
        '
        Me.chkPing.AutoCheck = False
        Me.chkPing.AutoSize = True
        Me.chkPing.Checked = True
        Me.chkPing.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPing.Location = New System.Drawing.Point(66, 56)
        Me.chkPing.Name = "chkPing"
        Me.chkPing.Size = New System.Drawing.Size(71, 17)
        Me.chkPing.TabIndex = 27
        Me.chkPing.Text = "Ping Test"
        Me.chkPing.UseVisualStyleBackColor = True
        '
        'chkTcp
        '
        Me.chkTcp.AutoCheck = False
        Me.chkTcp.AutoSize = True
        Me.chkTcp.Checked = True
        Me.chkTcp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTcp.Location = New System.Drawing.Point(264, 57)
        Me.chkTcp.Name = "chkTcp"
        Me.chkTcp.Size = New System.Drawing.Size(71, 17)
        Me.chkTcp.TabIndex = 28
        Me.chkTcp.Text = "TCP Test"
        Me.chkTcp.UseVisualStyleBackColor = True
        '
        'lblAbout
        '
        Me.lblAbout.AutoSize = True
        Me.lblAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAbout.Location = New System.Drawing.Point(559, 446)
        Me.lblAbout.Name = "lblAbout"
        Me.lblAbout.Size = New System.Drawing.Size(19, 13)
        Me.lblAbout.TabIndex = 29
        Me.lblAbout.Text = "@"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(132, 78)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(24, 20)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "ms"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer1
        '
        Me.Timer1.Interval = 10
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(10, 113)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(532, 346)
        Me.txtLog.TabIndex = 35
        Me.txtLog.Text = "Welcome to Darren's Nutcheck!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Feel free to contact me at drankof@gmail.com" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'chkUdp
        '
        Me.chkUdp.AutoCheck = False
        Me.chkUdp.AutoSize = True
        Me.chkUdp.Enabled = False
        Me.chkUdp.Location = New System.Drawing.Point(264, 76)
        Me.chkUdp.Name = "chkUdp"
        Me.chkUdp.Size = New System.Drawing.Size(73, 17)
        Me.chkUdp.TabIndex = 33
        Me.chkUdp.Text = "UDP Test"
        Me.chkUdp.UseVisualStyleBackColor = True
        '
        'lblSpinner
        '
        Me.lblSpinner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSpinner.Location = New System.Drawing.Point(548, 410)
        Me.lblSpinner.Name = "lblSpinner"
        Me.lblSpinner.Size = New System.Drawing.Size(23, 23)
        Me.lblSpinner.TabIndex = 36
        Me.lblSpinner.Text = "-"
        Me.lblSpinner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblSpinner.Visible = False
        '
        'Nutcheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(590, 471)
        Me.Controls.Add(Me.lblSpinner)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.chkUdp)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblAbout)
        Me.Controls.Add(Me.chkTcp)
        Me.Controls.Add(Me.chkPing)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPingTimeout)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.lblTcpReply)
        Me.Controls.Add(Me.lblTCP)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.lblPing)
        Me.Controls.Add(Me.lblPingReply)
        Me.Controls.Add(Me.txtIpAddress)
        Me.Controls.Add(Me.btnTest)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "Nutcheck"
        Me.Text = "Darren's Nutcheck"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnTest As Button
    Friend WithEvents txtIpAddress As TextBox
    Friend WithEvents lblPingReply As Label
    Friend WithEvents lblPing As Label
    Friend WithEvents txtPort As TextBox
    Friend WithEvents lblTCP As Label
    Friend WithEvents lblTcpReply As Label
    Friend WithEvents lblPort As Label
    Friend WithEvents lblIP As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPingTimeout As TextBox
    Friend WithEvents chkPing As CheckBox
    Friend WithEvents chkTcp As CheckBox
    Friend WithEvents lblAbout As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents txtLog As TextBox
    Friend WithEvents chkUdp As CheckBox
    Friend WithEvents lblSpinner As Label
End Class
