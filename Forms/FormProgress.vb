Public Class FormProgress

    Public Enum FileTask
        Idle
        Scan
        Save
    End Enum

    Public MyTask As Integer

    Private Sub FormFileProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainForm.Enabled = False

    End Sub

    Private Sub FormReset()
        ProgressBar1.Value = 0
        BtnOK.Enabled = False
        BtnOK.Text = "Waiting..."
    End Sub

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        MainForm.Enabled = True
        Me.Hide()
        FormReset()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case MyTask
            Case FileTask.Idle
                Timer1.Enabled = False
            Case FileTask.Scan
                ' Not done yet.
            Case FileTask.Save
                ProgressBar = ContinueSaving()
        End Select
    End Sub
End Class