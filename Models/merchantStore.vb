﻿Imports System.ComponentModel
Imports Windows.UI.Xaml.Media.Imaging
Imports System.IO


Public Class merchantStore
    Implements ComponentModel.INotifyPropertyChanged

    Public Property StoreName As String
    Public Property MerchantID As Guid
    Public Property StoreID As Guid
    Public Property visitors As List(Of visitorModel)
    Public ReadOnly Property URI As String
        Get
            Return "/storeMain?merchID=" + MerchantID.ToString + "&StoreID=" + StoreID.ToString
        End Get
    End Property
    Public Sub New(name As String)
        StoreID = Guid.NewGuid
        StoreName = name
        Me.visitors = New List(Of visitorModel)
    End Sub

    Public Sub New(name As String, visitor As visitorModel)
        Me.New(name)
        Me.visitors.Add(visitor)
    End Sub

    Public Sub New(name As String, visitors As List(Of visitorModel))
        Me.New(name)
        Me.visitors.AddRange(visitors)
    End Sub

    Public Sub New(Sample As Integer)
        Select Case Sample
            Case 0
                StoreID = Guid.NewGuid
                StoreName = "STORE"
                MerchantID = Guid.NewGuid
                visitors = New List(Of visitorModel)
                Dim tmpVisitor As New visitorModel("Jeff", New VisitorProductModel(Guid.NewGuid, "CoolA"))
                tmpVisitor.VisitorProducts.Item(0).Looks = New List(Of LookModel)


                Dim lm As New LookModel("Straight On", loadBitmap("C:\Users\jma42\Pictures\photo (66).jpg"))
                tmpVisitor.VisitorProducts.Item(0).Looks.Add(lm)
                visitors.Add(tmpVisitor)
        End Select
    End Sub
#Region "Helper"
    Private Function loadBitmap(filename As String) As BitmapImage
        Dim myBitmap As New BitmapImage(New System.Uri(filename))
        Return myBitmap

    End Function


#End Region
#Region "INotifyPropertyChanged members"
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
#End Region
End Class

