Namespace ViewModel
    Public Class StoreViewModel
        Inherits ViewModelBase
#Region "Declarations"
        Private _store As New merchantStore(0)
        Private _storeName As String
        Private _merchID As Guid
        Private _storeID As Guid
        Private _visitors As List(Of VisitorViewModel)
#End Region
        Public Sub New()

        End Sub

        Public Sub New(storeModel As merchantStore)
            _storeID = storeModel.StoreID
            _merchID = storeModel.MerchantID
            _storeName = storeModel.StoreName
            _visitors = ConvertMtoVM(storeModel.visitors)

        End Sub
#Region "Helper"
        Private Function ConvertMtoVM(inp As List(Of visitorModel)) As List(Of VisitorViewModel)
            Dim nVM As New List(Of VisitorViewModel)
            For Each m As visitorModel In inp
                nVM.Add(New VisitorViewModel(m))
            Next
            Return nVM
        End Function
#End Region
#Region "Properties"
        Public Property StoreName As String
            Get
                Return _storeName
            End Get
            Set(value As String)
                _storeName = value
                OnPropertyChanged("value")
            End Set
        End Property
        Public Property Visitors As List(Of VisitorViewModel)
            Get
                Return _visitors
            End Get
            Set(value As List(Of VisitorViewModel))
                _visitors = value
                OnPropertyChanged("Visitors")
            End Set
        End Property
#End Region
    End Class
End Namespace