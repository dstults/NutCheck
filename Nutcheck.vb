' Darren Stults
' Love thy DranKof

'Imports System.Net.NetworkInformation

Public Class Nutcheck

#Region "Declarations"

    Public Class ClsTcpJob
        Public tgtIp As String
        Public tgtIpWide As String
        Public tgtPort As Integer
        Public tgtPortWide As String
        Public tcpClient As New Net.Sockets.TcpClient
    End Class

    Public Class ClsPingJob
        Public tgtIp As String
        Public tgtIpWide As String
        Public myPing As New Net.NetworkInformation.Ping
        Public errored As Boolean
        Public successful As Boolean
        Public roundtripTime As Long
    End Class

    Public programNameWithVersion As String = "NutCheck v0.90"
    Public tgtAddresses As New HashSet(Of Net.IPAddress)
    Public tgtPorts As New HashSet(Of Integer)

    Private currentBoredom As Integer = 0
    Private myTimeout As Integer = 2000 ' ms until I abort

    Public pingJobs As New List(Of ClsPingJob)
    Public tcpJobs As New List(Of ClsTcpJob)

#End Region

#Region "Startup & Resets"

    Private Sub Nutcheck_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form_Reset()
    End Sub

    Private Sub Form_Reset()
        ' RESET GUI TO STARTUP STATE
        Me.Text = "Darren's " & programNameWithVersion & " - IDLE"
        btnTest.Enabled = True
        btnPreset1.Enabled = True
        btnPreset2.Enabled = True
        btnPreset3.Enabled = True
        btnPreset4.Enabled = True
        btnReset.enabled = False

        Timer1.Enabled = False
        lblSpinner.Text = "-"
        lblSpinner.Visible = False

        ' CLEAR DATA
        txtLog.Text = "Welcome to Darren's " & programNameWithVersion & "!" & vbNewLine & vbNewLine & "Check for updates at: https://github.com/DranKof/NutCheck/releases"
        txtResults.Text = ""
        txtOrganizedResults.Text = ""
        tgtPorts.Clear()
        tgtAddresses.Clear()
        pingJobs.Clear()
        tcpJobs.Clear()

    End Sub

    Private Sub Form_Work_Begin()
        Me.Text = "Darren's " & programNameWithVersion & " - WORKING"
        btnTest.Enabled = False
        btnPreset1.Enabled = False
        btnPreset2.Enabled = False
        btnPreset3.Enabled = False
        btnPreset4.Enabled = False
        btnReset.Enabled = False

        Timer1.Enabled = True
        lblSpinner.Visible = True
        btnTest.Enabled = False
        Timer1.Enabled = True

        txtLog.Text = "Test started: " & DateTime.Now.ToString & vbNewLine
        txtResults.Text = "Test started: " & DateTime.Now.ToString & vbNewLine
        txtOrganizedResults.Text = "Test started: " & DateTime.Now.ToString & vbNewLine
        currentBoredom = 0

    End Sub

    Private Sub Form_Work_End()
        Me.Text = "Darren's " & programNameWithVersion & " - FINISHED"
        btnTest.Enabled = False
        btnPreset1.Enabled = False
        btnPreset2.Enabled = False
        btnPreset3.Enabled = False
        btnPreset4.Enabled = False
        btnReset.Enabled = True

        Timer1.Enabled = False
        lblSpinner.Visible = True
        lblSpinner.Text = "!"

        LogVerbose("Work complete!")
        LogResult(vbNewLine & "No more active tests remaining.")
        LogPretty("")
        LogPretty("=============================")
        LogPretty("End of organized result dump.")
        LogPretty("=============================")

    End Sub

#End Region

#Region "Logging & User Feedback"

    Private Sub LogBasic(message As String)
        If Strings.Right(message, 3) <> "..." And Strings.Right(message, 1) <> "!" Then message = "  " & message
        txtLog.Text &= message & vbNewLine
    End Sub
    Private Sub LogVerbose(message As String)
        If chkVerboseMode.Checked Then LogBasic(message)
    End Sub

    Private Sub LogResult(message As String)
        txtResults.Text &= message & vbNewLine
    End Sub

    Private Sub LogPretty(message As String)
        txtOrganizedResults.Text &= message & vbNewLine
    End Sub

    Private Sub LogError(ex As Exception)
        If ex.InnerException IsNot Nothing Then
            LogVerbose("EXCEPTION:  " & ex.Message & vbNewLine & "    --> " & ex.InnerException.Message)
        Else
            LogVerbose("EXCEPTION:  " & ex.Message & vbNewLine & "    --> (no further information accompanies this error)")
        End If
    End Sub

    Private Function MakeIpWide(ip As String) As String
        ' Whether this should use spaces or zeroes should be an option!
        Dim octets() As String = Strings.Split(ip, ".")
        'Return Format(CInt(octets(0)), "000") & "." & Format(CInt(octets(1)), "000") & "." & Format(CInt(octets(2)), "000") & "." & Format(CInt(octets(3)), "000")
        Return Strings.Left("  ", 3 - Strings.Len(octets(0))) & octets(0) & "." & Strings.Left("  ", 3 - Strings.Len(octets(1))) & octets(1) & "." & Strings.Left("  ", 3 - Strings.Len(octets(2))) & octets(2) & "." & Strings.Left("  ", 3 - Strings.Len(octets(3))) & octets(3)
    End Function

#End Region

    Private Sub BtnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        ' RESET & START PROCESS
        Form_Work_Begin()

        If chkPing.Checked Or chkTcp.Checked Or chkUdp.Checked Then
            GetTestTimeout()
            GetIPAddresses()
            If tgtAddresses.Count > 0 Then
                If chkPing.Checked Then DoPingTest() Else LogBasic("Ping test skipped!")
                If chkTcp.Checked Or chkUdp.Checked Then GetPort()
                If chkTcp.Checked Then PrepareTcpTests() Else LogBasic("TCP test skipped!")
                'If chkUdp.Checked Then DoUdpTest() Else LogBasic("UDP test skipped!")
            Else
                LogBasic("No addresses found, aborting!")
            End If
        Else
            LogBasic("User attempted test without enabling any tests.")
            MsgBox("Please enable at least one test.", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub GetTestTimeout()
        ' Idiot-proofing
        Dim myTimeout As Integer = CInt(txtTimeout.Text)
        If myTimeout.ToString <> txtTimeout.Text Then
            ' Show user input was idiot-proofed
            LogBasic("Ping timeout input was changed")
            txtTimeout.Text = myTimeout.ToString
        End If
    End Sub

    Private Sub GetIPAddresses()
        LogBasic("Attempting to read IP address...")
        ' first split by commas
        ' then check for slashes -- later we'll add ability to use hyphens for ranges
        ' then attempt to parse as IP
        ' then dns lookup if failed

        Dim checkForCommas() As String = Split(txtTgtStart.Text, ",")
        For Each iStr As String In checkForCommas
            Dim checkForSlashes() As String = Split(iStr, "/")
            If checkForSlashes.Length > 1 Then
                ' Parse as masked
                LogVerbose("Detected masked group: " & checkForSlashes(0) & " / " & checkForSlashes(1))
                GetIPAddress_MaskedGroup(checkForSlashes)
            Else
                ' Parse as single entity
                GetIPAddress_Single(checkForSlashes(0))
            End If
        Next

        LogBasic("IP address acquisition finished")
    End Sub

    Private Sub GetIPAddress_MaskedGroup(ipAndMask As String())
        ' CIDR ONLY FOR NOW!
        ' SAMPLE INPUT: 192.168.1.0/24
        ' OUTPUT: 192.168.1.1-254

        ' Sampled from: https://social.msdn.microsoft.com/Forums/en-US/b216daf2-aac4-4a43-91e1-e1d216c63f0c/find-ip-range-from-cidr-mask?forum=vbgeneral
        Dim sampleIp As Net.IPAddress = Net.IPAddress.Parse(ipAndMask(0))
        'get the bytes of the address
        Dim addrOctets As List(Of Byte) = sampleIp.GetAddressBytes.ToList
        addrOctets.Reverse() 'reverse for bitconverter

        'convert to IP addr to number
        Dim ipAsInt As Integer = BitConverter.ToInt32(addrOctets.ToArray, 0)
        Dim cidr As Integer = CInt(ipAndMask(1))
        Dim ipCount As Integer = 1 << (32 - cidr) ' network size (hosts will be minus 2) '' wtf is << and HOW DOES THAT WORK?!
        Dim mask As Integer = &H80000000 >> (cidr - 1) 'create mask
        Dim netnum As Integer = ipAsInt And mask
        Dim subnetInt() As Byte = BitConverter.GetBytes(netnum) 'get bytes for network number
        Array.Reverse(subnetInt) 'in correct order
        Dim subnetId As New Net.IPAddress(subnetInt) ' actual network id

        'generate hosts -- skip 0 and go to max-2
        For intA As Integer = 1 To ipCount - 2
            Dim host As Integer = netnum + intA
            Dim hostNum() As Byte = BitConverter.GetBytes(host) 'get bytes for host 
            Array.Reverse(hostNum) 'in correct order
            Dim hostIp As New Net.IPAddress(hostNum) 'host
            tgtAddresses.Add(hostIp)
        Next

    End Sub

    Private Function GetIPAddress_Single(ipRaw As String) As Net.IPAddress
        Dim tgtAddress As Net.IPAddress = New Net.IPAddress(0)
        Try
            ' Test input for ip-addressyness
            tgtAddress = Net.IPAddress.Parse(ipRaw)
            tgtAddresses.Add(tgtAddress)
            LogVerbose("IP parse: " & tgtAddress.ToString)
        Catch ex As Exception
            LogError(ex)
            LogBasic("IP parse: failed to parse '" & ipRaw & "', will attempt DNS process")
            Try_DNS()
        End Try
        Return tgtAddress
    End Function

    Private Function Try_DNS() As Boolean
        LogBasic("Attempting DNS process...")

        Dim DNSLookup As New Net.IPHostEntry
        Dim getIpSuccess As Boolean
        Try
            DNSLookup = Net.Dns.GetHostEntry(txtTgtStart.Text)
        Catch ex As Exception
            LogError(ex)
            LogBasic("DNS resolution: Failed to resolve host, terminating test prematurely")
            getIpSuccess = False
        End Try

        If DNSLookup IsNot Nothing Then
            LogBasic("NS record(s) found for: " & txtTgtStart.Text)
            If DNSLookup.AddressList IsNot Nothing Then
                For Each tgtIp As Net.IPAddress In DNSLookup.AddressList
                    LogVerbose(tgtIp.ToString)
                    tgtAddresses.Add(tgtIp)
                Next
                LogBasic("DNS resolution: successful")
                getIpSuccess = True
            Else
                LogBasic("DNS resolution: No addresses attached to NS record, aborting")
                getIpSuccess = False
            End If
        Else
            LogBasic("DNS resolution: NS found but no addresses found associated with it??")
            getIpSuccess = False
        End If

        Return getIpSuccess
    End Function

    Private Sub DoPingTest()
        LogBasic("Attempting ping operation...")

        For Each tgtIp As Net.IPAddress In tgtAddresses
            pingJobs.Add(New ClsPingJob)
            pingJobs.Last.tgtIp = tgtIp.ToString
            pingJobs.Last.tgtIpWide = MakeIpWide(pingJobs.Last.tgtIp)
            ' Suppress this message if doing more than a few IPs:
            If chkVerboseMode.Checked Then LogVerbose("Sending ping (ICMP Echo Request) to " & pingJobs.Last.tgtIp)
            Try
                pingJobs.Last.myPing.SendAsync(tgtIp, myTimeout, pingJobs.Last.tgtIp)
                AddHandler pingJobs.Last.myPing.PingCompleted, AddressOf GetPingResult
            Catch ex As Exception
                LogError(ex)
            End Try
        Next

        LogBasic("Ping tests started")
    End Sub

    Private Sub GetPort()
        LogBasic("Attempting TCP port scan...")

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
                LogVerbose("Target port of: " & aStr & " was out of range, dropping.")
                tgtPorts.Remove(tgtPorts.Last)
            End If
        Next

        LogBasic("Port acquisition finished")
    End Sub

    Private Sub PrepareTcpTests()
        LogBasic("Preparing TCP tests...")

        Dim tcpTestCount As Integer
        For Each tgtIp As Net.IPAddress In tgtAddresses
            For Each tgtPort As Integer In tgtPorts
                tcpTestCount += 1

                tcpJobs.Add(New ClsTcpJob)
                tcpJobs.Last.tgtIp = tgtIp.ToString
                tcpJobs.Last.tgtIpWide = MakeIpWide(tcpJobs.Last.tgtIp)
                tcpJobs.Last.tgtPort = tgtPort.ToString
                tcpJobs.Last.tgtPortWide = Strings.Left("     ", 5 - Strings.Len(tgtPort.ToString)) & tgtPort.ToString
                LogVerbose("Attempting TCP connection with " & tcpJobs.Last.tgtIp & " over port " & tcpJobs.Last.tgtPort)
                Try
                    tcpJobs.Last.tcpClient.ConnectAsync(tgtIp, tgtPort)
                Catch ex As Exception
                    LogError(ex)
                End Try
            Next
        Next

        LogBasic("All TCP tests commenced (total: " & tcpTestCount & ")")
    End Sub

    Private Sub DoUdpTest()
        'LogVerbose("Testing port " & tgtPort & " over TCP...")


        'LogVerbose("UDP test finished")
    End Sub

    Private Function JobsAllDone() As Boolean
        'Future site of checking if all the tests have been resolved.
        Return False
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Me.Refresh()

        If tcpJobs.Count > 0 Then
            For Each tcpJob As ClsTcpJob In tcpJobs
                If tcpJob.tcpClient.Connected Then
                    LogResult(tcpJob.tgtIpWide & " : " & tcpJob.tgtPortWide & " PORT  OPEN   ( < " & (1 + currentBoredom) * Timer1.Interval & " ms )")
                    ResetTcpTest(tcpJob.tcpClient)
                ElseIf currentBoredom > myTimeout / Timer1.Interval Then
                    If Not chkIgnoreDead.Checked Then LogResult(tcpJob.tgtIpWide & " : " & tcpJob.tgtPortWide & " PORT CLOSED? ( > " & (-1 + currentBoredom) * Timer1.Interval & " ms )")
                    ResetTcpTest(tcpJob.tcpClient)
                End If
            Next
            ' Removal command must be done outside of foreach by integer countback to prevent failed enumeration error
            For intA As Integer = tcpJobs.Count - 1 To 0 Step -1
                If tcpJobs(intA).tcpClient.Client Is Nothing Then tcpJobs.Remove(tcpJobs(intA))
            Next
        End If
        If currentBoredom - 1 > myTimeout / Timer1.Interval Or JobsAllDone Then
            Form_Work_End()
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

    Private Sub GetPingResult(ByVal sender As Object, ByVal e As System.Net.NetworkInformation.PingCompletedEventArgs)
        ' e.UserState is the UserToken passed to the pingAsync, we will use it to find the matching ping job
        Dim myPingJob As ClsPingJob = pingJobs.Find(Function(p) p.tgtIp = e.UserState.ToString)
        myPingJob.roundtripTime = e.Reply.RoundtripTime
        'Unused stat:  e.Reply.Buffer.Length.ToString)

        If e.Error IsNot Nothing Then
            myPingJob.errored = True
            LogVerbose("Error from ping job " & myPingJob.tgtIp & " !")
            LogError(e.Error)
        Else
            Select Case e.Reply.Status
                Case Net.NetworkInformation.IPStatus.Success
                    myPingJob.successful = True
                    LogResult(myPingJob.tgtIpWide & " :  ICMP PING REPLY   ( = " & myPingJob.roundtripTime.ToString & " ms )")
                Case Else
                    ' successful false by default
                    If Not chkIgnoreDead.Checked Then LogResult(myPingJob.tgtIpWide & " :  ICMP PING  FAIL   ( = " & myPingJob.roundtripTime.ToString & " ms )")
                    ' we might want to flag it as errored though depending on the specific reply...?
            End Select

        End If

        With DirectCast(sender, Net.NetworkInformation.Ping)
            ' Remove handler because it is no longer needed
            RemoveHandler .PingCompleted, AddressOf GetPingResult
            ' Clean up unmanaged resources
            .Dispose()
        End With
    End Sub

    Private Sub ResetTcpTest(tcpJob As Net.Sockets.TcpClient)
        ' RESET SOCKET
        tcpJob.Close()
        tcpJob.Dispose()
    End Sub

    Private Sub lblAbout_Click(sender As Object, e As EventArgs) Handles lblAbout.Click
        MsgBox(programNameWithVersion & vbNewLine & "by Darren Stults (drankof@gmail.com)" & vbNewLine & "1/8/2020" & vbNewLine & "Check for updates at:" & vbNewLine & "https://github.com/DranKof/NutCheck/releases")
    End Sub

    Private Sub chkTcp_CheckedChanged(sender As Object, e As EventArgs)
        MsgBox(chkTcp.Checked)
    End Sub

    Private Sub btnDebugTest_Click(sender As Object, e As EventArgs) Handles btnPreset1.Click
        Dim tgtIpAddress As Net.IPAddress = Net.IPAddress.Parse("192.168.1.1")
        tgtAddresses.Add(tgtIpAddress)
        tgtPorts.Add(80)
        PrepareTcpTests()
        Timer1.Enabled = True
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Form_Reset()
    End Sub
End Class
