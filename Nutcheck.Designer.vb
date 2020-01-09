<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Nutcheck
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
        Me.components = New System.ComponentModel.Container()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.txtTgtStart = New System.Windows.Forms.TextBox()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.lblTimeout = New System.Windows.Forms.Label()
        Me.txtTimeout = New System.Windows.Forms.TextBox()
        Me.chkPing = New System.Windows.Forms.CheckBox()
        Me.lblAbout = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.lblSpinner = New System.Windows.Forms.Label()
        Me.chkTcp = New System.Windows.Forms.CheckBox()
        Me.chkUdp = New System.Windows.Forms.CheckBox()
        Me.txtResults = New System.Windows.Forms.TextBox()
        Me.lblPortExample = New System.Windows.Forms.Label()
        Me.btnDebugTest = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTest.Location = New System.Drawing.Point(588, 7)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(137, 70)
        Me.btnTest.TabIndex = 0
        Me.btnTest.Text = "RUN TEST"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'txtTgtStart
        '
        Me.txtTgtStart.Location = New System.Drawing.Point(138, 34)
        Me.txtTgtStart.Name = "txtTgtStart"
        Me.txtTgtStart.Size = New System.Drawing.Size(302, 20)
        Me.txtTgtStart.TabIndex = 1
        Me.txtTgtStart.Text = "192.168.1.1"
        Me.txtTgtStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(461, 34)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(120, 20)
        Me.txtPort.TabIndex = 19
        Me.txtPort.Text = "20-22, 80, 443"
        Me.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPort
        '
        Me.lblPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPort.Location = New System.Drawing.Point(461, 12)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(120, 20)
        Me.lblPort.TabIndex = 23
        Me.lblPort.Text = "Ports"
        Me.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblIP
        '
        Me.lblIP.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIP.Location = New System.Drawing.Point(134, 12)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(306, 20)
        Me.lblIP.TabIndex = 22
        Me.lblIP.Text = "Destination(s)"
        Me.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTimeout
        '
        Me.lblTimeout.Location = New System.Drawing.Point(15, 66)
        Me.lblTimeout.Name = "lblTimeout"
        Me.lblTimeout.Size = New System.Drawing.Size(51, 20)
        Me.lblTimeout.TabIndex = 26
        Me.lblTimeout.Text = "Timeout:"
        Me.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTimeout
        '
        Me.txtTimeout.Location = New System.Drawing.Point(72, 67)
        Me.txtTimeout.Name = "txtTimeout"
        Me.txtTimeout.Size = New System.Drawing.Size(48, 20)
        Me.txtTimeout.TabIndex = 25
        Me.txtTimeout.Text = "2000"
        Me.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkPing
        '
        Me.chkPing.AutoSize = True
        Me.chkPing.Checked = True
        Me.chkPing.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPing.Location = New System.Drawing.Point(34, 12)
        Me.chkPing.Name = "chkPing"
        Me.chkPing.Size = New System.Drawing.Size(71, 17)
        Me.chkPing.TabIndex = 27
        Me.chkPing.Text = "Ping Test"
        Me.chkPing.UseVisualStyleBackColor = True
        '
        'lblAbout
        '
        Me.lblAbout.AutoSize = True
        Me.lblAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAbout.Location = New System.Drawing.Point(9, 12)
        Me.lblAbout.Name = "lblAbout"
        Me.lblAbout.Size = New System.Drawing.Size(19, 13)
        Me.lblAbout.TabIndex = 29
        Me.lblAbout.Text = "@"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(120, 67)
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
        Me.txtLog.Location = New System.Drawing.Point(12, 108)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(378, 346)
        Me.txtLog.TabIndex = 35
        '
        'lblSpinner
        '
        Me.lblSpinner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSpinner.Location = New System.Drawing.Point(284, 82)
        Me.lblSpinner.Name = "lblSpinner"
        Me.lblSpinner.Size = New System.Drawing.Size(23, 23)
        Me.lblSpinner.TabIndex = 36
        Me.lblSpinner.Text = "-"
        Me.lblSpinner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblSpinner.Visible = False
        '
        'chkTcp
        '
        Me.chkTcp.AutoSize = True
        Me.chkTcp.Checked = True
        Me.chkTcp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTcp.Location = New System.Drawing.Point(34, 27)
        Me.chkTcp.Name = "chkTcp"
        Me.chkTcp.Size = New System.Drawing.Size(71, 17)
        Me.chkTcp.TabIndex = 28
        Me.chkTcp.Text = "TCP Test"
        Me.chkTcp.UseVisualStyleBackColor = True
        '
        'chkUdp
        '
        Me.chkUdp.AutoCheck = False
        Me.chkUdp.AutoSize = True
        Me.chkUdp.Enabled = False
        Me.chkUdp.Location = New System.Drawing.Point(34, 43)
        Me.chkUdp.Name = "chkUdp"
        Me.chkUdp.Size = New System.Drawing.Size(73, 17)
        Me.chkUdp.TabIndex = 33
        Me.chkUdp.Text = "UDP Test"
        Me.chkUdp.UseVisualStyleBackColor = True
        '
        'txtResults
        '
        Me.txtResults.Location = New System.Drawing.Point(396, 108)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.ReadOnly = True
        Me.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtResults.Size = New System.Drawing.Size(329, 346)
        Me.txtResults.TabIndex = 40
        '
        'lblPortExample
        '
        Me.lblPortExample.Location = New System.Drawing.Point(462, 57)
        Me.lblPortExample.Name = "lblPortExample"
        Me.lblPortExample.Size = New System.Drawing.Size(120, 20)
        Me.lblPortExample.TabIndex = 41
        Me.lblPortExample.Text = "Ex: 20-22, 80, 443"
        Me.lblPortExample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnDebugTest
        '
        Me.btnDebugTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDebugTest.Location = New System.Drawing.Point(12, 384)
        Me.btnDebugTest.Name = "btnDebugTest"
        Me.btnDebugTest.Size = New System.Drawing.Size(137, 70)
        Me.btnDebugTest.TabIndex = 42
        Me.btnDebugTest.Text = "DEBUG TEST"
        Me.btnDebugTest.UseVisualStyleBackColor = True
        Me.btnDebugTest.Visible = False
        '
        'Nutcheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 463)
        Me.Controls.Add(Me.btnDebugTest)
        Me.Controls.Add(Me.lblPortExample)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.lblSpinner)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.chkUdp)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblAbout)
        Me.Controls.Add(Me.chkTcp)
        Me.Controls.Add(Me.chkPing)
        Me.Controls.Add(Me.lblTimeout)
        Me.Controls.Add(Me.txtTimeout)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.txtTgtStart)
        Me.Controls.Add(Me.btnTest)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "Nutcheck"
        Me.Text = "Darren's Nutcheck"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnTest As Button
    Friend WithEvents txtTgtStart As TextBox
    Friend WithEvents txtPort As TextBox
    Friend WithEvents lblPort As Label
    Friend WithEvents lblIP As Label
    Friend WithEvents lblTimeout As Label
    Friend WithEvents txtTimeout As TextBox
    Friend WithEvents chkPing As CheckBox
    Friend WithEvents lblAbout As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents txtLog As TextBox
    Friend WithEvents lblSpinner As Label
    Friend WithEvents chkTcp As CheckBox
    Friend WithEvents chkUdp As CheckBox
    Friend WithEvents txtResults As TextBox
    Friend WithEvents lblPortExample As Label
    Friend WithEvents btnDebugTest As Button
End Class
