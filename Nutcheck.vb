Public Class Nutcheck

    Public Class ClsTcpJob
        Public tgtIp As String
        Public tgtPort As Integer
        Public tcpClient As New Net.Sockets.TcpClient
    End Class

    Public Class ClsPingJob
        Public tgtIp As String
        Public ping As New Net.NetworkInformation.Ping
        Public pingReply As New Task(Of Net.NetworkInformation.PingReply)
    End Class

    Public programNameWithVersion As String = "NutCheck v0.84"
    Public tgtAddresses As New HashSet(Of Net.IPAddress)
    Public tgtPorts As New HashSet(Of Integer)

    Private currentBoredom As Integer = 0
    Private maxWaitTime As Integer = 2000 ' ms until I abort

    Public pingJobs As New List(Of ClsPingJob)
    Public tcpJobs As New List(Of ClsTcpJob)

    Private Enum TestState
        idle
        working
        finished
    End Enum

    Private myTestState As Integer = TestState.idle

    Private Sub LogOut(logText As String)
        If Strings.Right(logText, 3) <> "..." And Strings.Right(logText, 1) <> "!" Then logText = "  " & logText
        txtLog.Text &= logText & vbNewLine
        'Me.Refresh()
    End Sub
    Private Sub ResultOut(message As String)
        txtResults.Text &= message & vbNewLine
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        ' RESET & START PROCESS
        txtLog.Text = ""
        txtResults.Text = ""
        currentBoredom = 0
        lblSpinner.Visible = True
        btnTest.Enabled = False
        Timer1.Enabled = True
        myTestState = TestState.working
        tgtPorts.Clear()
        tgtAddresses.Clear()
        maxWaitTime = CInt(txtTimeout.Text)

        If chkPing.Checked Or chkTcp.Checked Or chkUdp.Checked Then
            GetIPAddress()
            If myTestState = TestState.working Then
                If chkPing.Checked Then DoPingTest() Else LogOut("Ping test skipped!")
                If chkTcp.Checked Or chkUdp.Checked Then GetPort()
                If chkTcp.Checked Then PrepareTcpTests() Else LogOut("TCP test skipped!")
                'If chkUdp.Checked Then DoUdpTest() Else LogOut("UDP test skipped!")
            End If
        Else
            LogOut("User attempted test without enabling any tests. Silly user.")
            MsgBox("You must enable at least one test.", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub GetIPAddress()
        LogOut("Attempting to read IP address...")

        Dim tgtAddress As New Net.IPAddress(0)
        Try
            ' Test input for ip-addressyness
            tgtAddress = Net.IPAddress.Parse(txtTgtStart.Text)
            LogOut("IP input accepted: " & tgtAddress.ToString)
            tgtAddresses.Add(tgtAddress)
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
            DNSLookup = Net.Dns.GetHostEntry(txtTgtStart.Text)
        Catch ex As Exception ' Failure to resolve, terminate test prematurely
            LogOut("EXCEPTION:  " & ex.Message)
            LogOut("Terminating test prematurely")
            myTestState = TestState.finished
        End Try

        If DNSLookup IsNot Nothing Then
            LogOut("NS record(s) found for: " & txtTgtStart.Text)
            If DNSLookup.AddressList IsNot Nothing Then
                For Each tgtIp As Net.IPAddress In DNSLookup.AddressList
                    LogOut(tgtIp.ToString)
                    tgtAddresses.Add(tgtIp)
                Next
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

        ' Idiot-proofing
        Dim myTimeout As Integer = CInt(txtTimeout.Text)
        If myTimeout.ToString <> txtTimeout.Text Then
            ' Show user input was idiot-proofed
            LogOut("Ping timeout input was changed")
            txtTimeout.Text = myTimeout.ToString
        End If
        For Each tgtIp As Net.IPAddress In tgtAddresses
            pingJobs.Add(New ClsPingJob)
            pingJobs.Last.tgtIp = tgtIp.ToString
            LogOut("Sending ping (ICMP Echo Request) to " & tgtAddresses.First.ToString)
            Try
                pingJobs.Last.pingResult.SendPingAsync(tgtIp)
            Catch ex As Exception
                LogOut("EXCEPTION:  " & ex.Message)
            End Try
        Next

        LogOut("Ping tests started")
    End Sub

    Private Sub GetPort()
        LogOut("Attempting to read port...")

        ' Test input for portiness
        Dim strTgtPortsPre() As String = Split(txtPort.Text, ",")
        Dim strTgtPorts As New HashSet(Of String)
        For Each iStr As String In strTgtPortsPre
            If InStr(iStr, "-") Then
                Dim rangeSplit() As String = Split(iStr, "-")
                Dim intStart As Integer = CInt(rangeSplit(0))
                Dim intEnd As Integer = CInt(rangeSplit(1))
                For intA As Integer = intStart To intEnd
                    strTgtPorts.Add(intA)
                Next
            Else
                strTgtPorts.Add(CInt(iStr))
            End If
        Next
        For Each aStr In strTgtPorts
            tgtPorts.Add(CInt(aStr))
            ' Test values for further idiot-proofing
            If tgtPorts.Last >= 65536 Or tgtPorts.Last <= 0 Then
                LogOut("Target port of: " & aStr & " was out of range, dropping.")
                tgtPorts.Remove(tgtPorts.Last)
            End If
        Next

        LogOut("Port acquisition finished")
    End Sub

    Private Sub PrepareTcpTests()
        LogOut("Preparing TCP tests...")

        Dim tcpTestCount As Integer
        For Each tgtIp As Net.IPAddress In tgtAddresses
            For Each tgtPort As Integer In tgtPorts
                tcpTestCount += 1

                tcpJobs.Add(New ClsTcpJob)
                tcpJobs.Last.tgtIp = tgtIp.ToString
                tcpJobs.Last.tgtPort = tgtPort.ToString
                LogOut("Attempting TCP connection with " & tcpJobs.Last.tgtIp & " over port " & tcpJobs.Last.tgtPort)
                Try
                    tcpJobs.Last.tcpClient.ConnectAsync(tgtIp, tgtPort)
                Catch ex As Exception
                    LogOut("EXCEPTION:  " & ex.Message)
                End Try
            Next
        Next

        LogOut("All TCP tests commenced (total: " & tcpTestCount & ")")
    End Sub

    Private Sub DoUdpTest()
        'LogOut("Testing port " & tgtPort & " over TCP...")


        'LogOut("UDP test finished")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Me.Refresh()

        If pingJobs.count > 0 Then
            For Each pingJob As ClsPingJob In pingJobs
                If pingJob.pingResult.Then Then


                End If

                If pingReply.Status = Net.NetworkInformation.IPStatus.Success Then
                    ResultOut(pingReply.Address.ToString & " replied in " & pingReply.RoundtripTime.ToString & " ms")
                Else
                    ResultOut(pingReply.Address.ToString & " gave no ping reply")
                End If
            Next
        End If
        If tcpJobs.Count > 0 Then
            For Each tcpJob As ClsTcpJob In tcpJobs
                If tcpJob.tcpClient.Connected Then
                    ResultOut(tcpJob.tcpClient.Client.RemoteEndPoint.ToString & " connect in < " & (1 + currentBoredom) * Timer1.Interval & " ms")
                    ResetTcpTest(tcpJob.tcpClient)
                ElseIf currentBoredom > maxWaitTime / Timer1.Interval Then
                    ResultOut(tcpJob.tgtIp & ":" & tcpJob.tgtPort & " closed?")
                    ResetTcpTest(tcpJob.tcpClient)
                End If
            Next
            ' Removal command must be done outside of foreach by integer countback to prevent failed enumeration error
            For intA As Integer = tcpJobs.Count - 1 To 0 Step -1
                If tcpJobs(intA).tcpClient.Client Is Nothing Then tcpJobs.Remove(tcpJobs(intA))
            Next
        End If
        If tcpJobs.Count = 0 Then
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

    Private Sub ResetTcpTest(tcpJob As Net.Sockets.TcpClient)
        ' RESET SOCKET
        tcpJob.Close()
        tcpJob.Dispose()
    End Sub

    Private Sub lblAbout_Click(sender As Object, e As EventArgs) Handles lblAbout.Click
        MsgBox(programNameWithVersion & vbNewLine & "by Darren Stults (drankof@gmail.com)" & vbNewLine & "1/8/2020" & vbNewLine & "Check for updates at:" & vbNewLine & "https://github.com/DranKof/NutCheck/releases")
    End Sub

    Private Sub Nutcheck_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtLog.Text = "Welcome to Darren's " & programNameWithVersion & "!" & vbNewLine & vbNewLine & "Check for updates at: https://github.com/DranKof/NutCheck/releases"
    End Sub

    Private Sub chkTcp_CheckedChanged(sender As Object, e As EventArgs)
        MsgBox(chkTcp.Checked)
    End Sub

    Private Sub btnDebugTest_Click(sender As Object, e As EventArgs) Handles btnDebugTest.Click
        Dim tgtIpAddress As New Net.IPAddress(0)
        tgtIpAddress = Net.IPAddress.Parse("192.168.1.1")
        tgtAddresses.Add(tgtIpAddress)
        tgtPorts.Add(80)
        PrepareTcpTests()
        Timer1.Enabled = True
    End Sub

End Class
