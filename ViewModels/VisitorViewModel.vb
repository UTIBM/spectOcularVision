Imports Windows.UI.Xaml.Media.Imaging

Namespace ViewModel
    Public Class VisitorViewModel
        Inherits ViewModelBase
        Public Sub New()
            _VisitorID = Guid.NewGuid
        End Sub

        Public Sub New(visitorModel As visitorModel)
            _VisitorID = visitorModel.VisitorID
            _VisitorName = visitorModel.VisitorName
            _ProductViews = ConvertVP_Model_to_VM(visitorModel.VisitorProducts)
        End Sub

#Region "Declarations"
        Private _VisitorID As Guid
        Private _VisitorName As String
        Private _ProductViews As List(Of VisitorProductViewModel)
        Private _image As BitmapImage
#End Region
#Region "Helpers"
        Private Function ConvertVP_Model_to_VM(inP As List(Of VisitorProductModel)) As List(Of VisitorProductViewModel)
            Dim vpvm As New List(Of VisitorProductViewModel)
            For Each vpm As VisitorProductModel In inP
                vpvm.Add(New VisitorProductViewModel(vpm))
            Next
            Return vpvm
        End Function
#End Region
#Region "Properties"
        Public Property VisitorID As Guid
            Get
                Return _VisitorID
            End Get
            Set(value As Guid)
                _VisitorID = value
                OnPropertyChanged("VisitorID")
            End Set
        End Property
        Public Property VisitorName As String
            Get
                Return _VisitorName
            End Get
            Set(value As String)
                _VisitorName = value
                OnPropertyChanged("VisitorName")
            End Set
        End Property
        Public Property ProductViews As List(Of VisitorProductViewModel)
            Get
                Return _ProductViews
            End Get
            Set(value As List(Of VisitorProductViewModel))
                _ProductViews = value
                OnPropertyChanged("ProductViews")
            End Set
        End Property
        Public ReadOnly Property Image As BitmapImage
            Get
                Return _image
            End Get
        End Property
#End Region

    End Class
End Namespace

