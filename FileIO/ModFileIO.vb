Module ModFileIO


    Private CurrentProgress As Integer

    Public Function ContinueSaving() As Integer
        Dim netJobs As Integer

        ' do next saving task

        Return CInt(100 * CurrentProgress / netJobs)
    End Function

End Module
