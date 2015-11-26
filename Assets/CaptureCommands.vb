Public Class CaptureCommands
    Private _CaptureCommandList As New List(Of String) From {"Smile", "Inquisitive", "Turn Left", "Turn Right"}
    Private _CurrCommand As Integer = 0
    Public Sub New()

    End Sub

    Public ReadOnly Property current As String
        Get
            If _CurrCommand < _CaptureCommandList.Count Then
                Return _CaptureCommandList.Item(_CurrCommand)
            Else
                Return Nothing
            End If

        End Get
    End Property

    Public Sub first()
        _CurrCommand = 0
    End Sub

    Public Sub [next]()
        _CurrCommand += 1
    End Sub

    Public Sub [nextLoop]()

        _CurrCommand += 1
        If _CurrCommand >= _CaptureCommandList.Count Then
            _CurrCommand = 0
        End If

    End Sub
End Class
