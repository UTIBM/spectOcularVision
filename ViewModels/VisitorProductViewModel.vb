Namespace ViewModel
    Public Class VisitorProductViewModel
        Inherits ViewModelBase
#Region "Declarations"
        Private _currentLook As LookViewModel
        Private _looks As Dictionary(Of String, LookViewModel)
        Private _visitorProductModel As VisitorProductModel
        Private _viewState As VisitorProductViewModelStates
        Private _ProductName As String
        Private _VisitorProductID As Guid
        Private _ProductID As Guid
#End Region
#Region "New"
        Public Sub New()

        End Sub
        Public Sub New(v As visitorModel, p As productItemsModel)
            _visitorProductModel = New VisitorProductModel(v.MerchID, v.StoreID, v.VisitorID, p.ProductItemID, p.ProductName)
            _looks = New Dictionary(Of String, LookViewModel)
        End Sub
        Public Sub New(vpm As VisitorProductModel)
            convertVPM(vpm)
        End Sub

        Public Sub New(sample As Integer)
            Select Case sample
                Case 0
                    _visitorProductModel = New VisitorProductModel(Guid.NewGuid, "Cool Glasses")
                    convertVPM(_visitorProductModel)
                    _currentLook = Nothing
                    myCaptureCommands.first()
                    OnPropertyChanged("LookCommand")
            End Select
        End Sub
#End Region
#Region "Methods"
        Public Sub setCurrent(LookCommand As String)
            _currentLook = _looks.Item(LookCommand)
            OnPropertyChanged("CurrentLook")
        End Sub
#End Region
#Region "Properties"
        Public Property CurrentLook As LookViewModel
            Get
                Return _currentLook
            End Get
            Set(value As LookViewModel)
                _currentLook = value
                If value Is Nothing Then
                    Exit Property
                ElseIf _looks.ContainsKey(value.LookCommand) Then
                    _looks.Item(value.LookCommand) = value
                Else
                    _looks.Add(value.LookCommand, value)
                End If
                OnPropertyChanged("CurrentLook")
                OnPropertyChanged("Looks")
            End Set
        End Property
        Public Property Looks As List(Of LookViewModel)
            Get
                Return _looks.Values.ToList
            End Get
            Set(value As List(Of LookViewModel))
                convertVLM(value)
                OnPropertyChanged("Looks")
            End Set
        End Property
        Public Property ProductID As Guid
            Get
                Return _ProductID
            End Get
            Set(value As Guid)
                _ProductID = value
                OnPropertyChanged("ProductID")
            End Set
        End Property
        Public Property ProductName As String
            Get
                Return _ProductName
            End Get
            Set(value As String)
                _ProductName = value
                OnPropertyChanged("ProductName")
            End Set
        End Property
        Public Property VisitorProductID As Guid
            Get
                Return _VisitorProductID
            End Get
            Set(value As Guid)
                _VisitorProductID = value
                OnPropertyChanged("VisitorProductID")
            End Set
        End Property
#End Region
#Region "Helper"
        Private Sub convertVPM(vpm As VisitorProductModel)
            _looks = New Dictionary(Of String, LookViewModel)
            For Each look As LookModel In vpm.Looks
                _looks.Add(look.lookCommand, New LookViewModel(look))
            Next

        End Sub
        Private Sub convertVLM(vlm As List(Of LookViewModel))
            _looks = New Dictionary(Of String, LookViewModel)
            For Each l As LookViewModel In vlm
                _looks.Add(l.LookCommand, l)
            Next
        End Sub

#End Region
    End Class

    Public Enum VisitorProductViewModelStates
        blank = 1
        viewingExisting = 2
    End Enum
End Namespace

