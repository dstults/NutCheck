' Darren Stults
' Love thy DranKof

'Imports System.Net.NetworkInformation
' Acorn Icon: https://www.iconfinder.com/icons/92461/acorn_icon
' Free Use License: https://creativecommons.org/licenses/by/3.0/us/


Public Class FrmNutCheck

#Region "Declarations"

    Public programNameWithVersion As String = "NutCheck v0.98"
    Public lastMajorRevisionDate As String = "2020/01/11"
    Private ReadOnly MinWidth As Integer = 1050
    Private ReadOnly MinHeight As Integer = 400

    Public Class ClsTcpJob
        Public parent As ClsNetJob
        Public tgtPort As Integer
        Public tcpClient As New Net.Sockets.TcpClient
        Public portOpen As Boolean
        Public replyTime As Integer
        Public Sub New(setParent As ClsNetJob, setPort As Integer)
            parent = setParent
            parent.TcpJobs.Add(Me)
            tgtPort = setPort
        End Sub

    End Class
    Public Class ClsUdpJob
        Public parent As ClsNetJob
        Public tgtPort As Integer
        Public udpClient As New Net.Sockets.UdpClient
        Public portReplied As Boolean
        Public replyTime As Integer
        Public Sub New(setParent As ClsNetJob, setPort As Integer)
            parent = setParent
            parent.UdpJobs.Add(Me)
            tgtPort = setPort
        End Sub

    End Class

    Public Class ClsNetJob

        Public TgtIp As Net.IPAddress

        Public PingTest As New Net.NetworkInformation.Ping
        Public PingErrored As Boolean
        Public PingSuccessful As Boolean
        Public PingRoundtripTime As Long

        Public Hostname As String = ""

        Public TcpJobs As New List(Of ClsTcpJob)
        Public UdpJobs As New List(Of ClsUdpJob)

        Public Sub New(setIp As Net.IPAddress)
            TgtIp = setIp
        End Sub

    End Class

    Public TestTime As String

    Public tgtPorts As New SortedSet(Of Integer)
    Public hitPorts As New SortedSet(Of Integer)
    Public currentBoredom As Integer = 0
    Public myTimeout As Integer = 2000 ' ms until I abort
    Public thisScanDnsServer As String = ""

    Public netJobs As New List(Of ClsNetJob)
    Public pingJobs As New List(Of ClsNetJob)
    Public hostnameJobs As New List(Of ClsNetJob)
    Public tcpJobs As New List(Of ClsTcpJob)
    Public udpJobs As New List(Of ClsUdpJob)

#End Region

#Region "Startup, Resets, Shared Functions"

    Private Sub Nutcheck_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetComputerStats()
        txtAddresses.Text = myIpAddress & "/" & subnetMask
        Form_Reset()
    End Sub

    Public myComputerName As String
    Public myIpAddress As String
    Public subnetMask As String
    Public myGateway As String
    Public dns1 As String
    Public dns2 As String
    Private Sub GetComputerStats()
        ' Courtesy of: https://stackoverflow.com/questions/40814462/get-ip-address-subnet-default-gateway-dns1-and-dns2-with-vb-net

        'Computer Name
        myComputerName = System.Net.Dns.GetHostName()
        For Each ip In System.Net.Dns.GetHostEntry(myComputerName).AddressList
            If ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                'IPv4 Adress
                myIpAddress = ip.ToString()

                For Each adapter As Net.NetworkInformation.NetworkInterface In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    For Each unicastIPAddressInformation As Net.NetworkInformation.UnicastIPAddressInformation In adapter.GetIPProperties().UnicastAddresses
                        If unicastIPAddressInformation.Address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                            If ip.Equals(unicastIPAddressInformation.Address) Then
                                'Subnet Mask
                                subnetMask = unicastIPAddressInformation.IPv4Mask.ToString

                                Dim adapterProperties As Net.NetworkInformation.IPInterfaceProperties = adapter.GetIPProperties()
                                For Each gateway As Net.NetworkInformation.GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                    'Default Gateway
                                    myGateway = gateway.Address.ToString()
                                Next

                                'DNS1
                                If adapterProperties.DnsAddresses.Count > 0 Then
                                    dns1 = adapterProperties.DnsAddresses(0).ToString()
                                End If

                                'DNS2
                                If adapterProperties.DnsAddresses.Count > 1 Then
                                    dns2 = adapterProperties.DnsAddresses(1).ToString()
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Next
    End Sub

    Private Sub Form_Reset()
        ' RESET GUI TO STARTUP STATE
        Me.Text = "Darren's " & programNameWithVersion & " - IDLE"
        txtAddresses.Enabled = True
        txtPorts.Enabled = True
        btnTest.Enabled = True
        btnPreset1.Enabled = True
        btnPreset2.Enabled = True
        btnPreset3.Enabled = True
        btnPreset4.Enabled = True
        btnReset.Enabled = False
        BtnSaveCSV.Enabled = False
        BtnSaveText.Enabled = False

        Timer1.Enabled = False
        lblSpinner.Text = "-"
        lblSpinner.BackColor = Color.Yellow
        lblSpinner.Visible = False

        ' CLEAR DATA
        txtLog.Text = "Welcome to Darren's " & programNameWithVersion & "!" & vbNewLine & vbNewLine & "Check for updates at: https://github.com/DranKof/NutCheck/releases"
        txtResults.Text = ""
        txtOrganizedResults.Text = ""
        thisScanDnsServer = ""
        tgtPorts.Clear()
        hitPorts.Clear()
        netJobs.Clear()
        pingJobs.Clear()
        hostnameJobs.Clear()
        tcpJobs.Clear()
        udpJobs.Clear()

    End Sub

    Private Sub Form_Work_Begin()
        Me.Text = "Darren's " & programNameWithVersion & " - WORKING"
        txtAddresses.Enabled = False
        txtPorts.Enabled = False
        btnTest.Enabled = False
        btnPreset1.Enabled = False
        btnPreset2.Enabled = False
        btnPreset3.Enabled = False
        btnPreset4.Enabled = False
        btnReset.Enabled = False
        BtnSaveCSV.Enabled = False
        BtnSaveCSV.Enabled = False
        BtnSaveText.Enabled = False

        Timer1.Enabled = True
        lblSpinner.Visible = True
        btnTest.Enabled = False
        Timer1.Enabled = True

        TestTime = DateTime.Now.ToString
        txtLog.Text = "Test started: " & TestTime & vbNewLine
        txtResults.Text = "Test started: " & TestTime & vbNewLine
        txtOrganizedResults.Text = "Test started: " & TestTime & vbNewLine
        currentBoredom = 0

    End Sub

    Private Sub Form_Work_End()
        Me.Text = "Darren's " & programNameWithVersion & " - FINISHED"
        txtAddresses.Enabled = False
        txtPorts.Enabled = False
        btnTest.Enabled = False
        btnPreset1.Enabled = False
        btnPreset2.Enabled = False
        btnPreset3.Enabled = False
        btnPreset4.Enabled = False
        btnReset.Enabled = True
        BtnSaveCSV.Enabled = True
        BtnSaveText.Enabled = True

        Timer1.Enabled = False
        lblSpinner.Visible = True
        lblSpinner.Text = "😃"
        lblSpinner.BackColor = Color.Lime

        LogVerbose("Work complete!")
        LogResult(vbNewLine & "No more active tests remaining.")
        DoFullPrettyLog()

    End Sub

    Public Function MakeIpWide(anIp As Net.IPAddress) As String
        ' Whether this should use spaces or zeroes should be an option!
        Dim octets As List(Of Byte) = anIp.GetAddressBytes.ToList
        'Return Format(CInt(octets(0)), "000") & "." & Format(CInt(octets(1)), "000") & "." & Format(CInt(octets(2)), "000") & "." & Format(CInt(octets(3)), "000")
        Return Strings.Left("  ",
                        3 - Strings.Len(octets(0).ToString)) & octets(0).ToString & "." & Strings.Left("  ",
                        3 - Strings.Len(octets(1).ToString)) & octets(1).ToString & "." & Strings.Left("  ",
                        3 - Strings.Len(octets(2).ToString)) & octets(2).ToString & "." & Strings.Left("  ",
                        3 - Strings.Len(octets(3).ToString)) & octets(3).ToString
    End Function

    Public Function MakePortWide(aPort As Integer) As String
        Return Strings.Left("     ", 5 - Strings.Len(aPort.ToString)) & aPort.ToString
    End Function

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
            LogVerbose("EXCEPTION:  " & ex.Message)
        End If
    End Sub

#End Region

    Private Sub BtnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        If chkHostname.Checked Then ConfirmScanHostnames()
        ' RESET & START PROCESS
        Form_Work_Begin()

        If chkPing.Checked Or chkTcp.Checked Or chkUdp.Checked Then
            GetTestTimeout()
            GetIPAddresses()
            If netJobs.Count > 0 Then
                If chkPing.Checked Then PreparePingTest() Else LogBasic("Ping test skipped!")
                If chkHostname.Checked Then PrepareHostnameQueries() Else LogBasic("Hostname test skipped!")
                If chkTcp.Checked Or chkUdp.Checked Then GetPort()
                If chkTcp.Checked Then PrepareTcpTests() Else LogBasic("TCP test skipped!")
                If chkUdp.Checked Then PrepareUdpTest() Else LogBasic("UDP test skipped!")
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
        myTimeout = CInt(txtTimeout.Text)
        If myTimeout.ToString <> txtTimeout.Text Then
            ' Show user input was idiot-proofed
            LogBasic("Ping timeout input was changed")
            txtTimeout.Text = myTimeout.ToString
        End If
    End Sub

    Private Sub GetIPAddresses()
        LogBasic("Attempting to parse user IP addresses...")
        ' first split by commas
        ' then check for slashes -- later we'll add ability to use hyphens for ranges
        ' then attempt to parse as IP
        ' then dns lookup if failed

        Dim checkForCommas() As String = Split(txtAddresses.Text, ",")
        ' DNS querie will fail unless you trim excess spaces
        For intA As Integer = 0 To checkForCommas.Length - 1
            checkForCommas(intA) = Trim(checkForCommas(intA))
        Next
        For Each iStr As String In checkForCommas
            Dim checkForSlashes() As String = Split(iStr, "/")
            If checkForSlashes.Length > 1 Then
                ' Parse as masked
                GetIPAddress_MaskedGroup(checkForSlashes)
            Else
                ' Parse as single entity
                GetIPAddress_Single(checkForSlashes(0))
            End If
        Next

        LogBasic("IP address acquisition finished: " & netJobs.Count & " ips added")
    End Sub

    Private Sub ConfirmScanHostnames()
        Dim ReallyScanHostnames As MsgBoxResult = MsgBox("Warning: Hostname lookups are not fully operational (and won't be any time soon -- .Net doesn't natively support anything that works well)." & vbNewLine & vbNewLine & "There are still a couple fringe cases where you might actually want to use it, but it hasn't been optimized and it will lag, a lot." & vbNewLine & vbNewLine & "Are you sure you want to enable hostname lookups?", MsgBoxStyle.YesNo)
        Select Case ReallyScanHostnames
            Case MsgBoxResult.Yes
                ' do nothing
            Case MsgBoxResult.No
                chkHostname.Checked = False
        End Select
    End Sub

    Private Sub GetIPAddress_MaskedGroup(ipAndMask As String())
        ' Nice little shrunk snippet for converting CIDR to subnet:
        'Dim ip As Int64 = Convert.ToInt64(New String("1"c, cidr).PadRight(32, "0"c), 2)
        'Return (String.Join(".", New Net.IPAddress(ip).GetAddressBytes.Reverse))

        LogBasic("Attempting IP mask parse: " & ipAndMask(0) & " / " & ipAndMask(1))
        ' Sampled from: https://social.msdn.microsoft.com/Forums/en-US/b216daf2-aac4-4a43-91e1-e1d216c63f0c/find-ip-range-from-cidr-mask?forum=vbgeneral
        Dim sampleIp As Net.IPAddress = Net.IPAddress.Parse(ipAndMask(0))
        'get the bytes of the address
        Dim addrOctets As List(Of Byte) = sampleIp.GetAddressBytes.ToList
        ' 192.168.1.104 ==> 104.1.168.192
        addrOctets.Reverse() 'reverse for bitconverter

        'convert to IP addr to number
        '    104 = 01101000 / 1 = 00000001 / 168 = ‭10101000‬ / 192 = ‭11000000‬ ...
        '            01101000 00000001 10101000 11000000? so that should be 1744939200, but they got -‭1062731416‬
        ' reverse:   11000000 10101000 00000001 01101000 that's a perfect match...negativized
        '   result:  ‭11111111 11111111 11111111 11111111
        '            11000000 10101000 00000001 01101000‬
        Dim ipAsNumber As Integer = BitConverter.ToInt32(addrOctets.ToArray, 0)

        Dim cidr As Integer
        Dim maskAsNumber As Integer
        Dim ipCount As Integer
        If InStr(ipAndMask(1), ".") Then
            ' PROCESS A RAW MASK -- ACTUALLY SEEMS EASY!
            Dim maskOctets As List(Of Byte) = Net.IPAddress.Parse(ipAndMask(1)).GetAddressBytes.ToList
            maskOctets.Reverse()
            maskAsNumber = BitConverter.ToInt32(maskOctets.ToArray, 0)
            ipCount = Not maskAsNumber + 1
        Else
            cidr = CInt(ipAndMask(1))
            ' 00000000 00000000 00000000 00000001 gets shifted left by 32-cidr (so 32-24 is 8...shifted left 8 is:)
            ' 00000000 00000000 00000001 00000000 <--- that's 256 clients! AMAZING!
            ipCount = 1 << (32 - cidr) ' network size (hosts will be minus 2)

            ' &H is hexadecimal, 8000 0000 would therefore be...2,147,483,648..which is half of 4,294,967,296, which is 256*256*256*256
            ' ... More importantly, it's 10000000 00000000 00000000 00000000 so >> (cidr - 1) would mean move 1 right (24-1=) 23 spaces
            ' So...00000000 00000000 00000001 00000000 is the final result!
            maskAsNumber = &H80000000 >> (cidr - 1) 'create mask
        End If

        ' This makes perfect sense:
        Dim netnum As Integer = ipAsNumber And maskAsNumber
        Dim subnetInt() As Byte = BitConverter.GetBytes(netnum) 'get bytes for network number
        Array.Reverse(subnetInt) 'in correct order
        Dim subnetId As New Net.IPAddress(subnetInt) ' actual network id

        'generate hosts -- skip 0 and go to max-2
        For intA As Integer = 1 To ipCount - 2
            Dim host As Integer = netnum + intA
            Dim hostNum() As Byte = BitConverter.GetBytes(host) 'get bytes for host 
            Array.Reverse(hostNum) 'in correct order
            Dim hostIp As New Net.IPAddress(hostNum) 'host
            netJobs.Add(New ClsNetJob(hostIp))
        Next
        LogBasic("Bulk subnet IPs added: " & (ipCount - 2).ToString)

    End Sub

    Private Function GetIPAddress_Single(ipRaw As String) As Net.IPAddress
        Dim tgtAddress As Net.IPAddress = New Net.IPAddress(0)
        Try
            ' Test input for ip-addressyness
            tgtAddress = Net.IPAddress.Parse(ipRaw)
            netJobs.Add(New ClsNetJob(tgtAddress))
            LogVerbose("IP parsed: " & tgtAddress.ToString)
        Catch ex As Exception
            LogError(ex)
            LogBasic("'" & ipRaw & "' not IPv4, trying DNS")
            Try_DNS(ipRaw)
        End Try
        Return tgtAddress
    End Function

    Private Function Try_DNS(domainRaw As String) As Boolean
        LogVerbose("Attempting DNS process...")

        Dim DNSLookup As New Net.IPHostEntry
        Dim getIpSuccess As Boolean
        Try
            DNSLookup = Net.Dns.GetHostEntry(domainRaw)
        Catch ex As Exception
            LogError(ex)
            LogBasic("DNS resolution: Failed to resolve host '" & domainRaw & "', terminating test prematurely")
        End Try

        If DNSLookup IsNot Nothing Then
            If DNSLookup.AddressList IsNot Nothing Then
                LogBasic("NS record(s) found for: '" & domainRaw & "' with " & DNSLookup.AddressList.Count & " ips")
                For Each tgtIp As Net.IPAddress In DNSLookup.AddressList
                    LogVerbose(tgtIp.ToString)
                    netJobs.Add(New ClsNetJob(tgtIp))
                Next
                netJobs.Last.Hostname = domainRaw
                LogVerbose("DNS resolution: successful")
                getIpSuccess = True
            Else
                LogBasic("DNS resolution: No addresses attached to NS record '" & domainRaw & "', aborting")
                getIpSuccess = False
            End If
        Else
            LogBasic("DNS resolution: NS found for '" & domainRaw & "' but no addresses associated with it??")
            getIpSuccess = False
        End If

        Return getIpSuccess
    End Function

    Private Sub PreparePingTest()
        LogBasic("Launching ping operations...")

        Dim pingTestCount As Integer
        For Each pingJob As ClsNetJob In netJobs
            pingTestCount += 1
            pingJobs.Add(pingJob)
            If chkVerboseMode.Checked Then LogVerbose("Sending ping (ICMP Echo Request) to " & pingJob.TgtIp.ToString)
            Try
                pingJob.PingTest.SendAsync(pingJob.TgtIp.ToString, myTimeout, pingJob.TgtIp.ToString)
                AddHandler pingJob.PingTest.PingCompleted, AddressOf GetPingResult
            Catch ex As Exception
                LogError(ex)
            End Try
        Next

        LogBasic("Ping tests started: " & pingTestCount.ToString)
    End Sub

    Private Sub PrepareHostnameQueries()
        LogVerbose("Checking conditions for hostname lookup tests...")

        If dns1 <> "" Then
            thisScanDnsServer = dns1
            LogBasic("Attempting to use DNS to scan machine names.")
        ElseIf myGateway <> "" Then
            thisScanDnsServer = myGateway
            LogBasic("No DNSs set, will attempt to use gateway to query hostnames.")
        ElseIf myGateway = "" And dns1 = "" And chkTcp.Checked Then
            If Not tgtPorts.Contains(53) Then
                thisScanDnsServer = "Port 53"
                LogBasic("Including DNS port scan (53) to aid in detection of hostnames.")
                tgtPorts.Add(53)
            End If
        ElseIf myGateway = "" And dns1 = "" And Not chkTcp.Checked Then
            LogBasic("Cannot perform hostname scan without a gateway, dns or enabled tcp port scan.")
        End If

        LogVerbose("hostname scan prep checks finished")
    End Sub

    Private Sub GetPort()
        LogBasic("Parsing user port input...")

        ' Test input for portiness
        Dim strTgtPortsPre() As String = Split(txtPorts.Text, ",")
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

        LogBasic("Port input complete")
    End Sub

    Private Sub PrepareTcpTests()
        LogBasic("Preparing TCP tests...")

        Dim tcpTestCount As Integer
        For Each NetJob As ClsNetJob In netJobs
            For Each tgtPort As Integer In tgtPorts
                tcpTestCount += 1
                Dim tcpJob = New ClsTcpJob(NetJob, tgtPort)
                tcpJobs.Add(tcpJob)
                LogVerbose("Attempting TCP connection with " & NetJob.TgtIp.ToString & " over port " & tgtPort.ToString)
                Try
                    tcpJob.tcpClient.ConnectAsync(NetJob.TgtIp, tgtPort)
                Catch ex As Exception
                    LogError(ex)
                End Try
            Next
        Next

        LogBasic("All TCP tests commenced (total: " & tcpTestCount & ")")
    End Sub

    Private Sub PrepareUdpTest()
        LogVerbose("Conducting UDP experiments...")

        Dim udpTestCount As Integer
        For Each netJob As ClsNetJob In netJobs
            For Each tgtPort As Integer In tgtPorts
                udpTestCount += 1
                Dim udpJob = New ClsUdpJob(netJob, tgtPort)
                udpJobs.Add(udpJob)
            Next
        Next

        LogBasic("All UDP tests commenced (total: " & udpTestCount & ")")
    End Sub

    Private Function JobsAllDone() As Boolean
        'Future site of checking if all the tests have been resolved.
        If pingJobs.Count = 0 And hostnameJobs.Count = 0 And tcpJobs.Count = 0 And udpJobs.Count = 0 Then Return True Else Return False
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If tcpJobs.Count > 0 Then
            For intA As Integer = tcpJobs.Count - 1 To 0 Step -1
                Dim tcpJob As ClsTcpJob = tcpJobs(intA)
                If tcpJob.tcpClient.Connected Then
                    tcpJob.portOpen = True
                    If Not hitPorts.Contains(tcpJob.tgtPort) Then hitPorts.Add(tcpJob.tgtPort)
                    tcpJob.replyTime = (1 + currentBoredom) * Timer1.Interval
                    LogResult(MakeIpWide(tcpJob.parent.TgtIp) & " : " & MakePortWide(tcpJob.tgtPort) & " PORT  OPEN   ( < " & tcpJob.replyTime.ToString & " ms )")
                    KillTcpJob(tcpJob)
                ElseIf currentBoredom > myTimeout / Timer1.Interval Then
                    tcpJob.portOpen = False
                    tcpJob.replyTime = (-1 + currentBoredom) * Timer1.Interval
                    If Not chkIgnoreDead.Checked Then LogResult(MakeIpWide(tcpJob.parent.TgtIp) & " : " & MakePortWide(tcpJob.tgtPort) & " PORT CLOSED? ( > " & tcpJob.replyTime.ToString & " ms )")
                    KillTcpJob(tcpJob)
                End If
            Next
        End If
        If hostnameJobs.Count > 0 Then
            For intA As Integer = hostnameJobs.Count - 1 To 0 Step -1
                Dim hnJob As ClsNetJob = netJobs(intA)
                If hnJob.Hostname = "" Then
                    Try
                        hnJob.Hostname = Net.Dns.GetHostEntry(hnJob.TgtIp).HostName
                        LogBasic(MakeIpWide(hnJob.TgtIp) & " hostname: " & hnJob.Hostname)
                    Catch ex As Exception
                        LogError(ex)
                        hnJob.Hostname = "-"
                    End Try
                End If
                hostnameJobs.Remove(hnJob)
            Next
        End If
        If currentBoredom - 1 > myTimeout / Timer1.Interval Or JobsAllDone() Then
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

    Private Sub KillTcpJob(tcpJob As ClsTcpJob)
        tcpJob.tcpClient.Close()
        tcpJob.tcpClient.Dispose()
        tcpJobs.Remove(tcpJob)
    End Sub

    Private Sub GetPingResult(ByVal sender As Object, ByVal e As System.Net.NetworkInformation.PingCompletedEventArgs)
        ' e.UserState is the UserToken passed to the pingAsync, we will use it to find the matching ping job
        Dim pingJob As ClsNetJob = pingJobs.Find(Function(p) p.TgtIp.ToString = e.UserState.ToString)
        pingJob.PingRoundtripTime = e.Reply.RoundtripTime
        'Unused stat:  e.Reply.Buffer.Length.ToString)

        If e.Error IsNot Nothing Then
            pingJob.PingErrored = True
            LogBasic("Error from ping job " & pingJob.TgtIp.ToString & " !")
            LogError(e.Error)
        Else
            Select Case e.Reply.Status
                Case Net.NetworkInformation.IPStatus.Success
                    pingJob.PingSuccessful = True
                    LogResult(MakeIpWide(pingJob.TgtIp) & " :  ICMP PING REPLY   ( = " & pingJob.PingRoundtripTime.ToString & " ms )")
                    If chkHostname.Checked And pingJob.Hostname = "" Then hostnameJobs.Add(pingJob)
                Case Else
                    pingJob.PingSuccessful = False
                    ' we might also want to flag it as errored though depending on the specific reply...?
                    If Not chkIgnoreDead.Checked Then LogResult(MakeIpWide(pingJob.TgtIp) & " :  ICMP PING  FAIL   ( = " & pingJob.PingRoundtripTime.ToString & " ms )")
            End Select

        End If

        pingJobs.Remove(pingJob)

        With DirectCast(sender, Net.NetworkInformation.Ping)
            ' Remove handler because it is no longer needed
            RemoveHandler .PingCompleted, AddressOf GetPingResult
            ' Clean up unmanaged resources
            .Dispose()
        End With
    End Sub

    Private Sub DoFullPrettyLog() ' Formerly "LogOrganizedReport"
        'lblSpinner.BackColor = Color.LightPink
        'lblSpinner.Text = "GENERATING REPORT"
        Dim lotsOfEquals As String = ""
        Dim addPing As String = ""
        Dim addHostname As String = ""
        Dim portList As String = ""
        If chkPing.Checked Then
            lotsOfEquals &= "===="
            addPing = " P |"
        End If
        If chkHostname.Checked Then
            lotsOfEquals &= "=========="
            addHostname = "HOSTNAME |"
        End If
        If chkTcp.Checked Then
            For intA As Integer = 0 To hitPorts.Count - 1
                lotsOfEquals &= "======"
                portList &= MakePortWide(hitPorts(intA)) & "|"
            Next
        End If
        Dim pingResult As String = ""
        Dim hostname As String = ""
        Dim portResults As String = ""
        Dim canIgnoreJob As Boolean = True
        Dim hitPings As Integer
        'Dim hitPorts(65535) As Integer
        Dim hitIPs As New HashSet(Of String)
        Dim ignoredIps As New HashSet(Of String)
        For Each netJob As ClsNetJob In netJobs
            canIgnoreJob = True
            portResults = ""
            If chkPing.Checked Then
                If netJob.PingSuccessful Then
                    hitPings += 1
                    pingResult = " ! |"
                    canIgnoreJob = False
                Else
                    pingResult = "   |"
                End If
            End If
            If chkHostname.Checked Then
                If netJob.Hostname = "-" Or netJob.Hostname = "" Then hostname = " ? ? ? ? |" Else hostname = Strings.Left(netJob.Hostname & "         ", 9) & "|" : canIgnoreJob = False
            End If
            If chkTcp.Checked Or chkUdp.Checked Then
                For Each hitPort As Integer In hitPorts
                    portResults &= " "
                    If chkTcp.Checked Then
                        If netJob.TcpJobs.Find(Function(p) p.tgtPort = hitPort).portOpen Then
                            portResults &= "T"
                            canIgnoreJob = False
                        Else
                            portResults &= " "
                        End If
                    Else
                        portResults &= "."
                    End If
                    portResults &= " "
                    If chkUdp.Checked Then
                        If netJob.UdpJobs.Find(Function(p) p.tgtPort = hitPort).portReplied Then
                            portResults &= "U"
                            canIgnoreJob = False
                        Else
                            portResults &= " "
                        End If
                    Else
                        portResults &= "."
                    End If
                    portResults &= " |"
                Next
            End If
            If netJob.TgtIp.ToString = myIpAddress Then
                canIgnoreJob = False
                hitIPs.Add("|" & MakeIpWide(netJob.TgtIp) & "|" & pingResult & hostname & portResults & "<--Me")
            Else
                If canIgnoreJob = False Then
                    hitIPs.Add("|" & MakeIpWide(netJob.TgtIp) & "|" & pingResult & hostname & portResults)
                Else
                    ignoredIps.Add(netJob.TgtIp.ToString)
                End If
            End If
        Next

        LogPretty("  Scan Input -")
        LogPretty("  - IP Scan Input: " & txtAddresses.Text)
        LogPretty("  - Port Scan Input: " & txtPorts.Text)
        LogPretty("  - Timeout: " & txtTimeout.Text)
        Dim testList As String = ""
        If chkPing.Checked Then testList &= " PING"
        If chkHostname.Checked Then testList &= " HOSTNAME"
        If chkTcp.Checked Then testList &= " TCP"
        If chkUdp.Checked Then testList &= " UDP"
        LogPretty("  - Tests Conducted:" & testList)
        LogPretty("")
        LogPretty("  Scan Result Summary -")
        LogPretty("  - Hosts Discovered: " & hitIPs.Count)
        LogPretty("  - Ping Replies: " & hitPings)
        If ignoredIps.Count > 0 Then
            If ignoredIps.Count <= 32 Then
                LogPretty("  - The following IPs were omitted due to no contact:")
                Dim logOut As String = "  " & ignoredIps.First
                If ignoredIps.Count > 1 Then
                    For intA As Integer = 1 To ignoredIps.Count - 1
                        logOut &= ", " & ignoredIps(intA)
                    Next
                End If
                LogPretty(logOut)
            Else
                LogPretty("  - There were over 32 IP addresses omitted due to no contact.")
            End If
        End If
        If hitPorts.Count < tgtPorts.Count Then
            Dim ignoredPorts As New HashSet(Of Integer)
            For Each testPort As Integer In tgtPorts
                If Not hitPorts.Contains(testPort) Then ignoredPorts.Add(testPort)
            Next
            If ignoredPorts.Count <= 32 Then
                LogPretty("  - The following ports were omitted due to no contact:")
                Dim logOut As String = "  " & ignoredPorts.First.ToString
                If ignoredPorts.Count > 1 Then
                    For intA As Integer = 1 To ignoredPorts.Count - 1
                        logOut &= ", " & ignoredPorts(intA).ToString
                    Next
                End If
                LogPretty(logOut)
            Else
                LogPretty("  - There were over 32 ports omitted due to no contact.")
            End If
        End If
        LogPretty("")
        LogPretty("  Scan Detailed Report:")
        LogPretty("=================" & lotsOfEquals)
        LogPretty("| IP  ADDRESESS |" & addPing & addHostname & portList)
        LogPretty("=================" & lotsOfEquals)
        For Each iHit As String In hitIPs
            LogPretty(iHit)
        Next
        LogPretty("=================" & lotsOfEquals)
        LogPretty("")
        LogPretty("  End of log.")
    End Sub

    Private Sub LblAbout_Click(sender As Object, e As EventArgs) Handles lblAbout.Click
        MsgBox(programNameWithVersion & vbNewLine & "by Darren Stults (drankof@gmail.com)" & vbNewLine & lastMajorRevisionDate & vbNewLine & "Check for updates at:" & vbNewLine & "https://github.com/DranKof/NutCheck/releases")
    End Sub

    Private Sub ChkTcp_CheckedChanged(sender As Object, e As EventArgs)
        MsgBox(chkTcp.Checked)
    End Sub

    Private Sub BtnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Form_Reset()
    End Sub

    Private Sub ChkPing_CheckedChanged(sender As Object, e As EventArgs) Handles chkPing.CheckedChanged
        Select Case chkPing.Checked
            Case True
                chkHostname.Enabled = True
            Case False
                chkHostname.Enabled = False
                chkHostname.Checked = False
        End Select
    End Sub

    Private Sub BtnSaveCSV_Click(sender As Object, e As EventArgs) Handles BtnSaveCSV.Click
        Select Case SaveFileDialogCSV.ShowDialog()
            Case DialogResult.Abort, DialogResult.Cancel
                ' do nothing
            Case Else
                Try
                    IO.File.WriteAllText(SaveFileDialogCSV.FileName, MachineReadableResults)
                Catch ex As Exception
                    MsgBox("Could not save file:" & vbNewLine & ex.Message)
                End Try
        End Select
    End Sub

    Private Sub BtnSaveText_Click(sender As Object, e As EventArgs) Handles BtnSaveText.Click
        Select Case SaveFileDialogTXT.ShowDialog()
            Case DialogResult.Abort, DialogResult.Cancel
                ' do nothing
            Case Else
                IO.File.WriteAllText(SaveFileDialogTXT.FileName, txtOrganizedResults.Text)
        End Select
    End Sub

    Private Function MachineReadableResults() As String
        Dim MachineReport As String = "TEST TIME," & TestTime & vbNewLine
        MachineReport &= "INPUTS IPS," & txtAddresses.Text & vbNewLine
        MachineReport &= "INPUT PORTS," & txtPorts.Text & vbNewLine
        MachineReport &= "TIMEOUT," & txtTimeout.Text & vbNewLine
        MachineReport &= "TESTS,"
        If chkPing.Checked Then MachineReport &= "PING,"
        If chkHostname.Checked Then MachineReport &= "HOSTNAME,"
        If chkTcp.Checked Then MachineReport &= "TCP,"
        If chkUdp.Checked Then MachineReport &= "UDP,"
        MachineReport &= vbNewLine

        For Each iJob As ClsNetJob In netJobs
            MachineReport &= iJob.TgtIp.ToString
            If iJob.PingSuccessful Then
                MachineReport &= ",+ping"
            ElseIf iJob.PingErrored Then
                MachineReport &= ",!ping"
            Else
                MachineReport &= ",-ping"
            End If
            If iJob.TcpJobs.Count > 0 Then
                MachineReport &= ",tcp"
                For Each jTcpJob As ClsTcpJob In iJob.TcpJobs
                    If jTcpJob.portOpen Then
                        MachineReport &= ",+"
                    Else
                        MachineReport &= ",-"
                    End If
                    MachineReport &= jTcpJob.tgtPort
                Next
            End If
            MachineReport &= vbNewLine
        Next

        Return MachineReport

    End Function

    Private Sub FrmNutCheck_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If Me.Width < MinWidth Then Me.Width = MinWidth
        If Me.Height < MinHeight Then Me.Height = MinHeight
    End Sub

    Private Sub btnPreset1_Click(sender As Object, e As EventArgs) Handles btnPreset1.Click
        txtPorts.Text = "20-22, 80, 139, 443, 445, 3389, 5900"
    End Sub

    Private Sub btnPreset2_Click(sender As Object, e As EventArgs) Handles btnPreset2.Click
        txtPorts.Text = "7, 13, 17, 20-22, 53, 80, 139, 443, 445, 500, 1723, 3389, 5900"
    End Sub

End Class
