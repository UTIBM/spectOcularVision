'********************************************************
'
' Copyright (c) Microsoft. All rights reserved.
' THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
' ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
' IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
' PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
'
'*********************************************************

Imports System.Collections.Generic
Imports System
Imports MediaCapture

Namespace Global.SDKTemplate
    Partial Public Class MainPage
        Inherits SDKTemplate.Common.LayoutAwarePage

        ' Change the string below to reflect the name of your sample.
        ' This is used on the main page as the title of the sample.
        Public Const FEATURE_NAME As String = "Media Capture Sample (VB)"

        ' Change the array below to reflect the name of your scenarios.
        ' This will be used to populate the list of scenarios on the main page with
        ' which the user will choose the specific scenario that they are interested in.
        ' These should be in the form: "Navigating to a web page".
        ' The code in MainPage will take care of turning this into: "1) Navigating to a web page"
        Private scenariosList As New List(Of Scenario)() From {
           New Scenario() With {.Title = "Enumerate cameras and add a video effect", .ClassType = GetType(AdvancedCapture)},
            New Scenario() With {.Title = "Audio Capture", .ClassType = GetType(AudioCapture)},
            New Scenario() With {.Title = "Select Visitor", .ClassType = GetType(VisitorSelectView)},
            New Scenario() With {.Title = "Capture Look", .ClassType = GetType(LookCapture)},
            New Scenario() With {.Title = "Visitor Details", .ClassType = GetType(VisitorView)}
        }
        Private CaptureCommands As New List(Of String) From {"Smile", "Inquisitive", "Turn Left", "Turn Right"}
    End Class

    Public Class Scenario
        Public Property Title() As String

        Public Property ClassType() As Type

        Public Overrides Function ToString() As String
            Return Title
        End Function
    End Class
End Namespace
