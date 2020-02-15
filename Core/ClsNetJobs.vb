Public Class ClsNetJobs

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

End Class
