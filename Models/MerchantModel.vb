Imports System.ComponentModel


Public Class MerchantModel

#Region "Declarations"
    Private _merchantId As Guid
    Private _merchantName As String
    Private _stores As List(Of merchantStore)
#End Region

#Region "Properties"
    Public Property MerchantId As Guid
        Get
            Return _merchantId
        End Get
        Set(value As Guid)
            _merchantId = value
        End Set
    End Property
    Public Property MerchantName As String
        Get
            Return _merchantName
        End Get
        Set(value As String)
            _merchantName = value
        End Set
    End Property
    Public Property stores As List(Of merchantStore)
        Get
            Return _stores
        End Get
        Set(value As List(Of merchantStore))
            _stores = value
        End Set
    End Property

    Public ReadOnly Property URI As String
        Get
            Dim a As String = "/Merchant?ID="
            Return a + Me.MerchantId.ToString
        End Get
    End Property
#End Region

    Public Sub New(name As String)
            MerchantId = Guid.NewGuid
            MerchantName = name
            stores = New List(Of merchantStore)
        End Sub

        Public Sub New(name As String, store As merchantStore)
            Me.New(name)
            stores.Add(store)
        End Sub

        Public Sub New(name As String, stores As List(Of merchantStore))
            Me.New(name)
            stores.AddRange(stores)
        End Sub


End Class

