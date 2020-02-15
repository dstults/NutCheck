Module ModCore
    ' Darren Stults
    ' Love thy DranKof

    'Imports System.Net.NetworkInformation
    ' Acorn Icon: https://www.iconfinder.com/icons/92461/acorn_icon
    ' Free Use License: https://creativecommons.org/licenses/by/3.0/us/
    Public programNameWithVersion As String = "NutCheck v0.98"
    Public lastMajorRevisionDate As String = "2020/02/14"

    Public MainForm As FrmNutCheck

    Public tgtPorts As New SortedSet(Of Integer)
    Public hitPorts As New SortedSet(Of Integer)
    Public currentBoredom As Integer = 0
    Public myTimeout As Integer = 2000 ' ms until I abort
    Public thisScanDnsServer As String = ""
    Public TestTime As String

    Public netJobs As New List(Of ClsNetJob)
    Public pingJobs As New List(Of ClsNetJob)
    Public hostnameJobs As New List(Of ClsNetJob)
    Public tcpJobs As New List(Of ClsTcpJob)
    Public udpJobs As New List(Of ClsUdpJob)

    Public Sub Main()
        FrmNutCheck.Show()
        Application.Run()
    End Sub

End Module
