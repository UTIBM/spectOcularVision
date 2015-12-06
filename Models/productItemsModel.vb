Public Class productItemsModel
    Private _ProductItemID As Guid

    Public Property ProductItemID As Guid
        Get
            Return _ProductItemID
        End Get
        Set(value As Guid)
            _ProductItemID = value
        End Set
    End Property

    Public Property ProductName As String

    Public Sub New(product As String)
        ProductItemID = Guid.NewGuid
        ProductName = product
    End Sub
    Public Sub New(sample As Integer)
        ProductItemID = Guid.NewGuid

        Select Case sample
            Case 0
                ProductName = "Cool Glasses"
        End Select
    End Sub
End Class
