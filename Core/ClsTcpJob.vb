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
