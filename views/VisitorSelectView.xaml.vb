'*********************************************************
'
' Copyright (c) Microsoft. All rights reserved.
' THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
' ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
' IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
' PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
'
'*********************************************************

Imports Windows.Media.MediaProperties
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Imaging
Imports Windows.UI.Xaml.Navigation
Imports SDKTemplate
Imports System
Imports Windows.Media
Imports Windows.Media.Capture
Imports Windows.Foundation
Imports System.ComponentModel
Imports MediaCapture.ViewModel

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Partial Public NotInheritable Class VisitorSelectView
    Inherits SDKTemplate.Common.LayoutAwarePage

    Private m_capture As Windows.Media.Capture.MediaCapture
    Private m_photoSequenceCapture As Windows.Media.Capture.LowLagPhotoSequenceCapture
    Private m_framePtr() As Windows.Media.Capture.CapturedFrame

    Private m_photoStorageFile As Windows.Storage.StorageFile
    Private m_mediaPropertyChanged As TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs)
    Private m_VMPropertyChanged As PropertyChangedEventHandler

    Private m_bPreviewing As Boolean
    Private m_bPhotoSequence As Boolean
    Private m_highLighted As Boolean

    Private m_selectedIndex As Integer
    Private m_frameNum As UInteger
    Private m_ThumbnailNum As UInteger
    Private m_pastFrame As UInteger
    Private m_futureFrame As UInteger

    Private viewModel As StoreViewModel

    Private ReadOnly PHOTOSEQ_FILE_NAME As String = "photoSequence.jpg"

    ' A pointer back to the main page.  This is needed if you want to call methods in MainPage such
    ' as NotifyUser()
    Private rootPage As MainPage = MainPage.Current

    Public Sub New()
        Me.InitializeComponent()
        ScenarioInit()
        m_mediaPropertyChanged = CType([Delegate].Combine(m_mediaPropertyChanged, New TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs)(AddressOf SystemMediaControls_PropertyChanged)), TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs))
        m_VMPropertyChanged = New PropertyChangedEventHandler(Sub(sender As Object, args As PropertyChangedEventArgs)
                                                                  Select Case args.PropertyName

                                                                  End Select
                                                              End Sub)
        viewModel = New StoreViewModel(New merchantStore(0))
    End Sub

    Private Async Sub SystemMediaControls_PropertyChanged(ByVal sender As SystemMediaTransportControls, ByVal e As SystemMediaTransportControlsPropertyChangedEventArgs)
        Select Case e.Property
            Case SystemMediaTransportControlsProperty.SoundLevel
                Await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub()
                                                                                             If sender.SoundLevel <> Windows.Media.SoundLevel.Muted Then
                                                                                                 ScenarioInit()
                                                                                             Else
                                                                                                 ScenarioClose()
                                                                                             End If
                                                                                         End Sub)

            Case Else
        End Select
    End Sub

    Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
        Dim systemMediaControls As SystemMediaTransportControls = SystemMediaTransportControls.GetForCurrentView()
        AddHandler systemMediaControls.PropertyChanged, m_mediaPropertyChanged
        AddHandler viewModel.PropertyChanged, m_VMPropertyChanged
        AddHandler rootPage.ScenarioLoaded, AddressOf OnScenarioLoaded
        Me.UpdateLayout()
    End Sub



    Protected Overrides Sub OnNavigatedFrom(ByVal e As NavigationEventArgs)
        Dim systemMediaControls As SystemMediaTransportControls = SystemMediaTransportControls.GetForCurrentView()
        RemoveHandler systemMediaControls.PropertyChanged, m_mediaPropertyChanged
        RemoveHandler viewModel.PropertyChanged, m_VMPropertyChanged
        RemoveHandler rootPage.ScenarioLoaded, AddressOf OnScenarioLoaded
        ScenarioClose()
    End Sub

    Private Sub OnScenarioLoaded(sender As Object, e As EventArgs)
        Me.DataContext = viewModel
        VisitorList.ItemsSource = viewModel.Visitors
    End Sub

    Private Sub ShowStatusMessage(ByVal text As String)
        rootPage.NotifyUser(text, NotifyType.StatusMessage)
    End Sub

    Private Sub ShowExceptionMessage(ByVal ex As Exception)
        rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage)
    End Sub

    Private Sub ScenarioInit()
        Try

        Catch e As Exception
            ShowExceptionMessage(e)
        End Try

    End Sub

    Private Async Sub ScenarioClose()
        Try

        Catch exception As Exception
            ShowExceptionMessage(exception)
        End Try

    End Sub

    Private Async Sub Failed(ByVal currentCaptureObject As Windows.Media.Capture.MediaCapture, ByVal currentFailure As MediaCaptureFailedEventArgs)
        Try
            Await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub() ShowStatusMessage("Fatal error" & currentFailure.Message))
        Catch e As Exception
            ShowExceptionMessage(e)
        End Try
    End Sub





    Private Sub AddNewVisitor_Click(sender As Object, e As RoutedEventArgs)
        Dim a As String = "AA"
    End Sub

    Private Sub Border_Holding(sender As Object, e As HoldingRoutedEventArgs)
        Dim a As String = "AA"
    End Sub

    Private Sub Border_Tapped(sender As Object, e As TappedRoutedEventArgs)



    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        For Each VVM As VisitorViewModel In viewModel.Visitors
            If VVM.VisitorID = CType(CType(sender, Button).Tag, Guid) Then
                If rootPage.viewModel.ContainsKey(GetType(VisitorView)) Then
                    rootPage.viewModel.Item(GetType(VisitorView)) = VVM
                Else
                    rootPage.viewModel.Add(GetType(VisitorView), VVM)
                End If
                Exit For
            End If
        Next
        rootPage.LoadScenario(GetType(VisitorView))
    End Sub
End Class
