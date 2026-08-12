Imports System
Imports System.Windows

Namespace Global.LocalizationVB

    Public Class App
        Inherits Application
    End Class

    ' VB's WPF markup compiler does not implicitly satisfy IComponentConnector.InitializeComponent
    ' for an Application-rooted x:Class the way C# does, so App.xaml carries no x:Class and is
    ' loaded manually here instead of through the usual ApplicationDefinition/StartupUri codegen.
    Friend Module Program
        <STAThread>
        Public Sub Main()
            Dim app As New App()
            Dim resourceLocator As New Uri("/06-Localization-VB;component/App.xaml", UriKind.Relative)
            Application.LoadComponent(app, resourceLocator)
            app.StartupUri = New Uri("MainWindow.xaml", UriKind.Relative)
            app.Run()
        End Sub
    End Module

End Namespace
