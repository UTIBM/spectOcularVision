Imports Windows.UI.Xaml.Media.Imaging
Namespace ViewModel
    Public Class LookViewModel
        Inherits ViewModelBase

        Private _lookModel As LookModel
        Private _lookCommand As String
        Private _lookID As Guid
        Private _lookImage As BitmapImage
        Private _lookTimeStamp As Date
        Private _lookViewState As lookModelEnum

        Public Sub New(lookModel As LookModel)
            _lookModel = lookModel
            ConvertFromLookModel(_lookModel)
        End Sub

        Public Sub New(visitorProductViewModel As ViewModel.VisitorProductViewModel, LookCommand As String, Image As BitmapImage, ViewState As lookModelEnum)
            _lookModel = New LookModel(LookCommand, Image)
            ConvertFromLookModel(_lookModel)
        End Sub



#Region "Properties"
        Public Property LookCommand As String
            Get
                Return _lookCommand
            End Get
            Set(value As String)
                _lookCommand = value
                OnPropertyChanged("LookCommand")
            End Set
        End Property
        Public Property LookID As Guid
            Get
                Return _lookID
            End Get
            Set(value As Guid)
                _lookID = value
                OnPropertyChanged("LookID")
            End Set
        End Property
        Public Property LookImage As BitmapImage
            Get
                Return _lookImage
            End Get
            Set(value As BitmapImage)
                _lookImage = value
                OnPropertyChanged("LookImage")
            End Set
        End Property
        Public Property LookTimeStamp As Date
            Get
                Return _lookTimeStamp
            End Get
            Set(value As Date)
                _lookTimeStamp = value
                OnPropertyChanged("LookTimeStamp")
            End Set
        End Property
        Public Property LookViewState As lookModelEnum
            Get
                Return _lookViewState
            End Get
            Set(value As lookModelEnum)
                _lookViewState = value
                OnPropertyChanged("LookViewState")
            End Set
        End Property
#End Region
#Region "Helper"
        Private Sub ConvertFromLookModel(lm As LookModel)
            LookCommand = lm.lookCommand
            LookImage = lm.lookImage
            LookID = lm.lookId
            LookTimeStamp = lm.timestamp
        End Sub
        Private Sub ConvertToLookModel()
            _lookModel.lookCommand = _lookCommand
            _lookModel.lookId = _lookID
            _lookModel.lookImage = _lookImage
            _lookModel.timestamp = _lookTimeStamp
        End Sub
#End Region
    End Class
End Namespace
