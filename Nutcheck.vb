Public Class Nutcheck

    ' Shared variable(s)
    Private tgtAddress As New Net.IPAddress(0)
    Private tgtPort As Integer = 80
    Private testTcpSocket As Net.Sockets.TcpClient

    Private currentBoredom As Integer = 0
    Private ReadOnly maxBoredom As Integer = 5 ' seconds until I abort

    Private waitForPing As Boolean
    Private waitForTCP As Boolean
    Private waitForUDP As Boolean

    Private Enum TestState
        idle
        working
        finished
    End Enum

    Private myTestState As Integer = TestState.idle

    Private Sub LogOut(logText As String)
        If Strings.Right(logText, 3) <> "..." And Strings.Right(logText, 1) <> "!" Then logText = "  " & logText
        txtLog.Text &= logText & vbNewLine
        Me.Refresh()
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        ' RESET & START PROCESS
        txtLog.Text = ""
        lblPingReply.Text = "untested"
        lblTcpReply.Text = "untested"
        currentBoredom = 0
        lblSpinner.Visible = True
        btnTest.Enabled = False
        Timer1.Enabled = True
        myTestState = TestState.working

        If chkPing.Checked Or chkTcp.Checked Or chkUdp.Checked Then
            GetIPAddress()
            If myTestState = TestState.working Then
                If chkPing.Checked Then DoPingTest() Else LogOut("Ping test skipped!")
                If chkTcp.Checked Or chkUdp.Checked Then GetPort()
                If chkTcp.Checked Then DoTcpTest() Else LogOut("TCP test skipped!")
                'If chkUdp.Checked Then DoUdpTest() Else LogOut("UDP test skipped!")
            End If
        Else
            LogOut("User attempted test without enabling any tests. Silly user.")
            MsgBox("You must enable at least one test.", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub GetIPAddress()
        LogOut("Attempting to read IP address...")

        Try
            ' Test input for ip-addressyness
            tgtAddress = Net.IPAddress.Parse(txtIpAddress.Text)
            If txtIpAddress.Text <> tgtAddress.ToString Then
                LogOut("IP address was simplified")
                ' Show user what input was parsed to
                txtIpAddress.Text = tgtAddress.ToString
            End If
            LogOut("IP input accepted: " & tgtAddress.ToString)
        Catch ex As Exception
            LogOut("EXCEPTION:  " & ex.Message)
            Try_DNS()
        End Try

        LogOut("IP address acquisition finished")
    End Sub

    Private Sub Try_DNS()
        LogOut("Attempting DNS process...")

        Dim DNSLookup As New Net.IPHostEntry
        Try
            DNSLookup = Net.Dns.GetHostEntry(txtIpAddress.Text)
        Catch ex As Exception ' Failure to resolve, terminate test prematurely
            LogOut("EXCEPTION:  " & ex.Message)
            LogOut("Terminating test prematurely")
            myTestState = TestState.finished
        End Try

        If DNSLookup IsNot Nothing Then
            LogOut("NS record(s) found for: " & txtIpAddress.Text)
            If DNSLookup.AddressList IsNot Nothing Then
                For Each ip As Net.IPAddress In DNSLookup.AddressList
                    LogOut(ip.ToString)
                Next
                tgtAddress = DNSLookup.AddressList.First
                If DNSLookup.AddressList.Count > 1 Then LogOut("More than one DNS entries found, using first: " & tgtAddress.ToString)
                txtIpAddress.Text = tgtAddress.ToString
            Else
                LogOut("No addresses attached to NS record, aborting")
                myTestState = TestState.finished
            End If
        Else
            LogOut("No addresses found")
        End If

        LogOut("DNS resolution finished")
    End Sub

    Private Sub DoPingTest()
        LogOut("Attempting ping operation...")
        waitForPing = True

        ' Idiot-proofing
        Dim myTimeout As Integer = CInt(txtPingTimeout.Text)
        If myTimeout.ToString <> txtPingTimeout.Text Then
            ' Show user input was idiot-proofed
            LogOut("Ping timeout input was changed")
            txtPingTimeout.Text = myTimeout.ToString
        End If
        Dim myPing As New Net.NetworkInformation.Ping
        Dim pingReply As Net.NetworkInformation.PingReply
        LogOut("Sending ping (ICMP Echo Request) to " & tgtAddress.ToString)
        Try
            'The mid-old way.
            'If My.Computer.Network.Ping(txtIP1.Text) Then lblPingReply.Text = "Hit!" Else lblPingReply.Text = "Miss!"
            pingReply = myPing.Send(txtIpAddress.Text, myTimeout)
            If pingReply.Status = Net.NetworkInformation.IPStatus.Success Then
                lblPingReply.Text = pingReply.RoundtripTime.ToString & " ms"
                LogOut("Ping received after " & pingReply.RoundtripTime.ToString & " ms")
            Else
                lblPingReply.Text = "no reply"
                LogOut("There was no reply")
            End If

        Catch ex As Exception
            LogOut("EXCEPTION:  " & ex.Message)
            lblPingReply.Text = "ERROR!"
        End Try

        waitForPing = False
        LogOut("Ping test finished")
    End Sub

    Private Sub GetPort()
        LogOut("Attempting to read port...")

        ' Test input for portiness
        tgtPort = CInt(txtPort.Text)
        ' Test port for idiot-proofing
        If tgtPort >= 65536 Or tgtPort <= 0 Then
            tgtPort = 80
            LogOut("Target port was out of range, setting to HTTP (80)")
        End If
        If tgtPort.ToString <> txtPort.Text Or tgtPort >= 65536 Or tgtPort <= 0 Then
            LogOut("Port integer input was changed")
            txtPort.Text = tgtPort.ToString
        End If

        LogOut("Port acquisition finished")
    End Sub

    Private Sub DoTcpTest()
        LogOut("Testing TCP connection with " & tgtAddress.ToString & " over port " & tgtPort.ToString)
        waitForTCP = True

        testTcpSocket = New Net.Sockets.TcpClient
        Try
            testTcpSocket.ConnectAsync(tgtAddress, tgtPort)
        Catch ex As Exception
            LogOut("EXCEPTION:  " & ex.Message)
        End Try

    End Sub

    Private Sub DoUdpTest()
        LogOut("Testing port " & tgtPort & " over TCP...")
        waitForUDP = True

        waitForUDP = False
        LogOut("UDP test finished")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Refresh()

        If waitForPing Then
        End If
        If waitForTCP Then
            If testTcpSocket.Connected Then
                lblTcpReply.Text = "We get signal!"
                LogOut("Connection made in under " & (1 + currentBoredom) * Timer1.Interval & " ms")
                ResetTcpTest()
            ElseIf currentBoredom > maxBoredom * 1000 / Timer1.Interval Then
                lblTcpReply.Text = "Port closed?"
                LogOut("TCP test timed out (> " & maxBoredom & " secs)")
                ResetTcpTest()
            End If
        End If
        If waitForUDP Then
        End If
        If Not waitForPing And Not waitForTCP And Not waitForUDP Then
            'Reset abort to prepare for next test
            btnTest.Enabled = True
            Timer1.Enabled = False
            lblSpinner.Visible = False
            LogOut(vbNewLine & "Work complete!")
            myTestState = TestState.finished
        Else
            currentBoredom += 1
            Select Case lblSpinner.Text
                Case "-" : lblSpinner.Text = "\"
                Case "\" : lblSpinner.Text = "|"
                Case "|" : lblSpinner.Text = "/"
                Case "/" : lblSpinner.Text = "-"
            End Select
        End If
    End Sub

    Private Sub ResetTcpTest()
        ' RESET SOCKET
        testTcpSocket.Close()
        testTcpSocket.Dispose()
        waitForTCP = False
        LogOut("TCP test finished")
    End Sub

    Private Sub lblAbout_Click(sender As Object, e As EventArgs) Handles lblAbout.Click
        MsgBox("By Darren Stults" & vbNewLine & "1/8/2020")
    End Sub

End Class
