Imports System.ComponentModel


Public Class visitorModel
    Implements ComponentModel.INotifyPropertyChanged

    Public Property VisitorName As String
    Public Property StoreID As Guid
    Public Property MerchID As Guid
    Public Property VisitorID As Guid
    Public Property VisitorProducts As List(Of VisitorProductModel)

    Public Sub New(visitor As String)
        VisitorID = Guid.NewGuid
        VisitorName = visitor
        VisitorProducts = New List(Of VisitorProductModel)
    End Sub

    Public Sub New(visitor As String, visitorProduct As VisitorProductModel)
        Me.New(visitor)
        VisitorProducts.Add(visitorProduct)
    End Sub

    Public Sub New(visitor As String, visitorProducts As List(Of VisitorProductModel))
        Me.New(visitor)
        Me.VisitorProducts.AddRange(visitorProducts)
    End Sub
    Public Sub New(sample As Integer)
        Select Case sample
            Case 0
                VisitorID = Guid.NewGuid
                VisitorName = "Jeffrey P Morgan"
                VisitorProducts = New List(Of VisitorProductModel)
        End Select
    End Sub
    Public ReadOnly Property URI As String
        Get
            Return "/Visitor?merchID=" + MerchID.ToString + "&StoreID=" + StoreID.ToString + "&VisitorID=" + VisitorID.ToString
        End Get
    End Property
    Public ReadOnly Property AddFrameURI As String
        Get
            Return "/AddFrame?merchID=" + MerchID.ToString + "&StoreID=" + StoreID.ToString + "&VisitorID=" + VisitorID.ToString
        End Get
    End Property
#Region "INotifyPropertyChanged members"
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
#End Region
End Class


