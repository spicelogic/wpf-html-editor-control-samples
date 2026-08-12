Imports System
Imports System.Windows

Namespace Global.CustomSpellChecker

    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    ''' <remarks>
    ''' Declared explicitly (rather than letting PresentationBuildTasks generate it from
    ''' App.xaml) because the VB.NET WPF code generator emits a partial Application class
    ''' that fails to fulfil the IComponentConnector contract in SDK-style projects
    ''' (long-standing BC30149 / BC30420 bug). App.xaml is treated as a Page and the
    ''' Application is bootstrapped from the manual Sub Main below.
    ''' </remarks>
    Public Class App
        Inherits Application
    End Class

    Friend Module Program
        <STAThread>
        Public Sub Main()
            Dim app As New App()
            Dim resourceLocator As New Uri(
                "/07-CustomSpellChecker-VB;component/App.xaml", UriKind.Relative)
            Application.LoadComponent(app, resourceLocator)
            app.StartupUri = New Uri("MainWindow.xaml", UriKind.Relative)
            app.Run()
        End Sub
    End Module

End Namespace
