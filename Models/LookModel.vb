Imports System.ComponentModel
Imports Windows.UI.Xaml.Media.Imaging

Public Enum lookModelEnum
    large_capture = 1
    large_save = 2
    small_view = 20
End Enum

Public Class LookModel
    Implements ComponentModel.INotifyPropertyChanged

    Public Property ViewState As lookModelEnum
    Public Property lookCommand As String
    Public Property lookId As Guid
    Public Property lookImage As BitmapImage
    Public Property timestamp As Date
    Public ReadOnly Property lrg_OK As String
        Get
            Select Case ViewState
                Case lookModelEnum.large_capture
                    Return "Visible"
                Case Else
                    Return "Collapsed"
            End Select
        End Get
    End Property

    Public Sub New(lookCommand As String, image As BitmapImage)
        lookId = Guid.NewGuid
        Me.lookCommand = lookCommand
        lookImage = image
        timestamp = Date.Now()
        Me.ViewState = lookModelEnum.large_capture
    End Sub

    Public Sub New()
        lookId = Guid.NewGuid
    End Sub

#Region "INotifyPropertyChanged members"
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
#End Region
End Class
