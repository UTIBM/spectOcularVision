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
Partial Public NotInheritable Class LookCapture
    Inherits SDKTemplate.Common.LayoutAwarePage

    Private m_mediaCaptureMgr As Windows.Media.Capture.MediaCapture
    Private m_photoStorageFile As Windows.Storage.StorageFile
    Private m_recordStorageFile As Windows.Storage.StorageFile
    Private m_bRecording As Boolean
    Private m_bSuspended As Boolean
    Private m_bPreviewing As Boolean
    Private m_mediaPropertyChanged As TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs)
    Private m_VMPropertyChanged As PropertyChangedEventHandler
    Private ReadOnly PHOTO_FILE_NAME As String = "photo.jpg"
    Private ReadOnly VIDEO_FILE_NAME As String = "video.mp4"



    ' A pointer back to the main page.  This is needed if you want to call methods in MainPage such
    ' as NotifyUser()
    Private rootPage As MainPage = MainPage.Current

    Private viewModel As New ViewModel.VisitorProductViewModel(0)

    Public Sub New()
        Me.InitializeComponent()
        ScenarioInit()
        m_mediaPropertyChanged = CType([Delegate].Combine(m_mediaPropertyChanged, New TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs)(AddressOf SystemMediaControls_PropertyChanged)), TypedEventHandler(Of SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs))
        m_VMPropertyChanged = New PropertyChangedEventHandler(Sub(sender As Object, args As PropertyChangedEventArgs)
                                                                  Select Case args.PropertyName
                                                                      Case "Looks"
                                                                          CapturedLooks.ItemsSource = viewModel.Looks
                                                                          CapturedLooks.UpdateLayout()
                                                                      Case "CaptureCommand"
                                                                          Me.CaptureCommand.Text = viewModel.CaptureCommand
                                                                          'CaptureCommand.UpdateLayout()
                                                                  End Select
                                                              End Sub)


    End Sub

    Private Function VM_PropertyChanged() As PropertyChangedEventHandler()
        Throw New NotImplementedException()
    End Function



    ''' <summary>
    ''' Invoked when this page is about to be displayed in a Frame.
    ''' </summary>
    ''' <param name="e">Event data that describes how this page was reached.  The Parameter
    ''' property is typically used to configure the page.</param>
    Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
        Dim systemMediaControls As SystemMediaTransportControls = SystemMediaTransportControls.GetForCurrentView()
        AddHandler systemMediaControls.PropertyChanged, m_mediaPropertyChanged

        AddHandler viewModel.PropertyChanged, m_VMPropertyChanged
        outputGridCol1.DataContext = viewModel.Looks
    End Sub

    Protected Overrides Sub OnNavigatedFrom(ByVal e As NavigationEventArgs)
        Dim systemMediaControls As SystemMediaTransportControls = SystemMediaTransportControls.GetForCurrentView()
        RemoveHandler systemMediaControls.PropertyChanged, m_mediaPropertyChanged
        ScenarioClose()
    End Sub

    Private Sub ScenarioInit()
        Me.DataContext = viewModel
        btnTakePhoto1.IsEnabled = True
        btnTakePhoto1.Content = "TakePhoto"
        'viewModel.FirstCaptureCommand
        CaptureCommand.Text = viewModel.CaptureCommand
        m_bRecording = False
        m_bPreviewing = False
        m_bSuspended = False

        previewElement1.Source = Nothing
        AcceptPanel.Visibility = Visibility.Collapsed
        CompletePanel.Visibility = Visibility.Collapsed

        ShowStatusMessage("")
        StartDevice()
        StartPreview()

    End Sub

    Private Async Sub ScenarioClose()
        Try
            If m_bRecording Then
                ShowStatusMessage("Stopping Record")

                Await m_mediaCaptureMgr.StopRecordAsync()
                m_bRecording = False
            End If
            If m_bPreviewing Then
                ShowStatusMessage("Stopping preview")
                Await m_mediaCaptureMgr.StopPreviewAsync()
                m_bPreviewing = False
            End If

            If m_mediaCaptureMgr IsNot Nothing Then
                ShowStatusMessage("Stopping Camera")
                previewElement1.Source = Nothing
                m_mediaCaptureMgr.Dispose()
            End If
        Catch e As Exception
            ShowExceptionMessage(e)
        End Try
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

    Public Async Sub Failed(ByVal currentCaptureObject As Windows.Media.Capture.MediaCapture, ByVal currentFailure As MediaCaptureFailedEventArgs)
        Try
            Await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub() ShowStatusMessage("Fatal error" & currentFailure.Message))
        Catch e As Exception
            ShowExceptionMessage(e)
        End Try
    End Sub

    Friend Async Sub StartDevice()
        Try
            ShowStatusMessage("Starting device")
            m_mediaCaptureMgr = New Windows.Media.Capture.MediaCapture()
            Await m_mediaCaptureMgr.InitializeAsync()

            If m_mediaCaptureMgr.MediaCaptureSettings.VideoDeviceId <> "" AndAlso m_mediaCaptureMgr.MediaCaptureSettings.AudioDeviceId <> "" Then
                btnTakePhoto1.IsEnabled = True
                ShowStatusMessage("Device initialized successful")
                AddHandler m_mediaCaptureMgr.Failed, AddressOf Failed
            Else
                ShowStatusMessage("No VideoDevice/AudioDevice Found")
            End If
        Catch exception As Exception
            ShowExceptionMessage(exception)
        End Try
    End Sub

    Friend Async Sub StartPreview()
        m_bPreviewing = False
        Try
            Dim sldBrightness As New Slider()
            sldBrightness.Value = 50
            Dim sldContrast As New Slider()
            sldContrast.Value = 50
            ShowStatusMessage("Starting preview")
            previewCanvas1.Visibility = Windows.UI.Xaml.Visibility.Visible
            previewElement1.Source = m_mediaCaptureMgr
            Await m_mediaCaptureMgr.StartPreviewAsync()
            If (m_mediaCaptureMgr.VideoDeviceController.Brightness IsNot Nothing) AndAlso m_mediaCaptureMgr.VideoDeviceController.Brightness.Capabilities.Supported Then
                SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Brightness, sldBrightness)
            End If
            If (m_mediaCaptureMgr.VideoDeviceController.Contrast IsNot Nothing) AndAlso m_mediaCaptureMgr.VideoDeviceController.Contrast.Capabilities.Supported Then
                SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Contrast, sldContrast)
            End If
            m_bPreviewing = True
            CapturePanel.Visibility = Visibility.Visible
            AcceptPanel.Visibility = Visibility.Collapsed
            CompletePanel.Visibility = Visibility.Collapsed

            imageElement2.Visibility = Visibility.Collapsed
            ShowStatusMessage("Start preview successful")
        Catch exception As Exception
            m_bPreviewing = False
            previewElement1.Source = Nothing
            ShowExceptionMessage(exception)
        End Try
    End Sub

    Friend Async Sub StopPreview()
        m_bPreviewing = False
        Try
            Dim sldBrightness As New Slider()
            sldBrightness.Value = 50
            Dim sldContrast As New Slider()
            sldContrast.Value = 50
            ShowStatusMessage("Stoping preview")
            previewCanvas1.Visibility = Windows.UI.Xaml.Visibility.Visible
            previewElement1.Source = m_mediaCaptureMgr
            Await m_mediaCaptureMgr.StopPreviewAsync
            If (m_mediaCaptureMgr.VideoDeviceController.Brightness IsNot Nothing) AndAlso m_mediaCaptureMgr.VideoDeviceController.Brightness.Capabilities.Supported Then
                SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Brightness, sldBrightness)
            End If
            If (m_mediaCaptureMgr.VideoDeviceController.Contrast IsNot Nothing) AndAlso m_mediaCaptureMgr.VideoDeviceController.Contrast.Capabilities.Supported Then
                SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Contrast, sldContrast)
            End If
            m_bPreviewing = False
            ShowStatusMessage("Stop preview successful")

        Catch exception As Exception
            m_bPreviewing = False
            previewElement1.Source = Nothing
            ShowExceptionMessage(exception)
        End Try
    End Sub

    Friend Async Sub btnTakePhoto_Click(ByVal sender As Object, ByVal e As Windows.UI.Xaml.RoutedEventArgs)

        Try
            ShowStatusMessage("Taking photo")
            btnTakePhoto1.IsEnabled = False

            m_photoStorageFile = Await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync(PHOTO_FILE_NAME, Windows.Storage.CreationCollisionOption.GenerateUniqueName)

            ShowStatusMessage("Create photo file successful")
            Dim imageProperties As ImageEncodingProperties = ImageEncodingProperties.CreateJpeg()

            Await m_mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageProperties, m_photoStorageFile)

            btnTakePhoto1.IsEnabled = True
            ShowStatusMessage("Photo taken")

            Dim photoStream = Await m_photoStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read)

            ShowStatusMessage("File open successful")
            Dim bmpimg = New BitmapImage()
            bmpimg.SetSource(photoStream)
            imageElement2.Source = bmpimg
            imageElement2.Visibility = Visibility.Visible
            'StopPreview()

            CapturePanel.Visibility = Visibility.Collapsed
            AcceptPanel.Visibility = Visibility.Visible
            CompletePanel.Visibility = Visibility.Collapsed
            ShowStatusMessage(Me.m_photoStorageFile.Path)

        Catch exception As Exception
            ShowExceptionMessage(exception)
            btnTakePhoto1.IsEnabled = True
        End Try
    End Sub

    Private Sub SetupVideoDeviceControl(ByVal videoDeviceControl As Windows.Media.Devices.MediaDeviceControl, ByVal slider As Slider)
        Try
            If (videoDeviceControl.Capabilities).Supported Then
                slider.IsEnabled = True
                slider.Maximum = videoDeviceControl.Capabilities.Max
                slider.Minimum = videoDeviceControl.Capabilities.Min
                slider.StepFrequency = videoDeviceControl.Capabilities.Step
                Dim controlValue As Double = 0
                If videoDeviceControl.TryGetValue(controlValue) Then
                    slider.Value = controlValue
                End If
            Else
                slider.IsEnabled = False
            End If
        Catch e As Exception
            ShowExceptionMessage(e)
        End Try
    End Sub

    ' VideoDeviceControllers
    Friend Sub sldBrightness_ValueChanged(ByVal sender As Object, ByVal e As Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs)
        'Try
        '    Dim succeeded As Boolean = m_mediaCaptureMgr.VideoDeviceController.Brightness.TrySetValue(sldBrightness.Value)
        '    If Not succeeded Then
        '        ShowStatusMessage("Set Brightness failed")
        '    End If
        'Catch exception As Exception
        '    ShowExceptionMessage(exception)
        'End Try
    End Sub

    Friend Sub sldContrast_ValueChanged(ByVal sender As Object, ByVal e As Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs)
        'Try
        '    Dim succeeded As Boolean = m_mediaCaptureMgr.VideoDeviceController.Contrast.TrySetValue(sldContrast.Value)
        '    If Not succeeded Then
        '        ShowStatusMessage("Set Contrast failed")
        '    End If
        'Catch exception As Exception
        '    ShowExceptionMessage(exception)
        'End Try
    End Sub

    Private Sub ShowStatusMessage(ByVal text As String)
        rootPage.NotifyUser(text, NotifyType.StatusMessage)
    End Sub

    Private Sub ShowExceptionMessage(ByVal ex As Exception)
        rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage)
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As RoutedEventArgs)
        If viewModel.CurrentLook Is Nothing Then
            viewModel.CurrentLook = New LookViewModel(viewModel, viewModel.CaptureCommand, CType(imageElement2.Source, BitmapImage), lookModelEnum.large_capture)
        ElseIf viewModel.CurrentLook.LookViewState <> lookModelEnum.large_save Then
            Dim tmpCurrentLook As LookViewModel = viewModel.CurrentLook
            tmpCurrentLook.LookTimeStamp = Date.Now
            tmpCurrentLook.LookImage = CType(imageElement2.Source, BitmapImage)
            viewModel.CurrentLook = tmpCurrentLook
        End If
        imageElement2.Visibility = Visibility.Collapsed
        viewModel.NextCaptureCommand()
        If viewModel.CaptureCommand Is Nothing Then
            CapturePanel.Visibility = Visibility.Visible
            AcceptPanel.Visibility = Visibility.Collapsed
            CompletePanel.Visibility = Visibility.Collapsed
        Else
            CapturePanel.Visibility = Visibility.Collapsed
            AcceptPanel.Visibility = Visibility.Collapsed
            CompletePanel.Visibility = Visibility.Visible
        End If
        viewModel.CurrentLook = Nothing
    End Sub

    Private Sub btnRetry_Click(sender As Object, e As RoutedEventArgs)
        If viewModel.CurrentLook Is Nothing Then
            'viewModel.CurrentLook = New LookViewModel(viewModel, viewModel.CaptureCommand, Nothing, lookModelEnum.large_capture)
        ElseIf viewModel.CurrentLook.LookViewState = lookModelEnum.large_save Or
               viewModel.CurrentLook.LookViewState = lookModelEnum.small_view Then
            viewModel.CurrentLook.LookViewState = lookModelEnum.large_capture
        End If
        imageElement2.Visibility = Visibility.Collapsed
        CapturePanel.Visibility = Visibility.Visible
        AcceptPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub StackPanel_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim a As String = "aa"
        viewModel.setCurrent(CType(CType(sender, StackPanel).Tag, String))
        viewModel.CurrentLook.LookViewState = lookModelEnum.large_save
        imageElement2.Source = viewModel.CurrentLook.LookImage
        ImmediateCapture.DataContext = viewModel.CurrentLook
        CapturePanel.Visibility = Visibility.Collapsed
        AcceptPanel.Visibility = Visibility.Visible
        imageElement2.Visibility = Visibility.Visible
    End Sub

    Private Sub NextPair_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Complete_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
