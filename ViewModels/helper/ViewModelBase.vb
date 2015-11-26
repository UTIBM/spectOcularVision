Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Threading
Imports System.Windows.Input

Public Class ViewModelBase
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Private myServiceLocator As New ServiceLocator
    Protected myCaptureCommands As New CaptureCommands


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Protected Sub OnPropertyChanged(ByVal strPropertyName As String)
        'If Me.PropertyChangedEvent IsNot Nothing Then
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(strPropertyName))
        'End If
    End Sub

    Protected Sub OnCollectionChanged()
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace))
    End Sub

    Private privateThrowOnInvalidPropertyName As Boolean

    Protected Overridable Property ThrowOnInvalidPropertyName() As Boolean
        Get
            Return privateThrowOnInvalidPropertyName
        End Get
        Set(ByVal value As Boolean)
            privateThrowOnInvalidPropertyName = value
        End Set
    End Property

    <Conditional("DEBUG"), DebuggerStepThrough()>
    Public Sub VerifyPropertyName(ByVal propertyName As String)
        ' Verify that the property name matches a real,
        ' public, instance property on this object.
        'If ComponentModel.TypeDescriptor.GetProperties(Me)(propertyName) Is Nothing Then
        '    Dim msg As String = "Invalid property name: " & propertyName

        '    If Me.ThrowOnInvalidPropertyName Then
        '        Throw New Exception(msg)
        '    Else
        '        Debug.WriteLine(msg)
        '    End If
        'End If
    End Sub

    Private privateDisplayName As String

    Public Overridable ReadOnly Property CaptureCommand As String
        Get
            If myCaptureCommands.current IsNot Nothing Then
                Return myCaptureCommands.current
            Else
                Return "All Done"
            End If
        End Get
    End Property
    Public Overridable Property DisplayName() As String
        Get
            Return privateDisplayName
        End Get

        Protected Set(ByVal value As String)
            privateDisplayName = value
        End Set
    End Property

    Public Function ServiceLocator() As ServiceLocator
        Return Me.myServiceLocator
    End Function

    Public Function GetService(Of T)() As T
        Return myServiceLocator.GetService(Of T)()
    End Function
    Public Sub FirstCaptureCommand()
        myCaptureCommands.first()
        OnPropertyChanged("CaptureCommand")

    End Sub
    Public Sub NextCaptureCommand()
        myCaptureCommands.next()
        OnPropertyChanged("CaptureCommand")


    End Sub
    Public Sub NextLookCaptureCommand()
        myCaptureCommands.nextLoop()
        OnPropertyChanged("CaptureCommand")

    End Sub
End Class
