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
