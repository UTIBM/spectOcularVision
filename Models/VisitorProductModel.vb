Imports System.ComponentModel



Public Class VisitorProductModel

    Public Property StoreID As Guid
    Public Property MerchID As Guid
    Public Property VisitorID As Guid
    Public Property Product As String
    Public Property VisitorProductID As Guid
    Public Property timestamp As Date
    Public Property Looks As List(Of LookModel)
    Private _currentLook As LookModel
    Public Property CurrentLook As LookModel
        Get
            Return _currentLook
        End Get
        Set(value As LookModel)
            _currentLook = value
        End Set
    End Property

    Public Sub New(productID As Guid, product As String)
        VisitorProductID = productID
        Me.Product = product
        timestamp = Date.Now()
        Me.Looks = New List(Of LookModel)
    End Sub

    Public Sub New(MerchID As Guid, StoreID As Guid, VisitorID As Guid, productID As Guid, product As String)
        Me.New(productID, product)
        _MerchID = MerchID
        _StoreID = StoreID
        _VisitorID = VisitorID
    End Sub

    Public Sub New(productID As Guid, product As String, look As LookModel)
        Me.New(productID, product)
        Me.Looks.Add(look)
    End Sub

    Public Sub New(productID As Guid, product As String, looks As List(Of LookModel))
        Me.New(productID, product)
        Me.Looks.AddRange(looks)
    End Sub


End Class

