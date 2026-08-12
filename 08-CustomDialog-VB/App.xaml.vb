Imports System
Imports System.Windows

' No license key set, so the editor runs in trial mode. To activate a purchased license,
' set SpiceLogic.HtmlEditor.WPF.WpfHtmlEditor.LicenseKey with the key emailed to you.

Namespace Global.CustomDialog

    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Partial Public Class App
        Inherits System.Windows.Application
    End Class

    Friend Module Program
        <STAThread>
        Public Sub Main()
            Dim app As New App()
            Dim resourceLocator As New Uri("/08-CustomDialog-VB;component/App.xaml", UriKind.Relative)
            System.Windows.Application.LoadComponent(app, resourceLocator)
            app.StartupUri = New Uri("MainWindow.xaml", UriKind.Relative)
            app.Run()
        End Sub
    End Module

End Namespace
