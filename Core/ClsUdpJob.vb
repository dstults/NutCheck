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
