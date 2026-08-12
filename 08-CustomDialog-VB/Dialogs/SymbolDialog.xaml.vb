Imports System
Imports System.Windows
Imports System.Windows.Input
Imports SpiceLogic.HtmlEditor.WPF.Models.BOs.EditorEventArgs
Imports SpiceLogic.HtmlEditor.WPF.Controls
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports SpiceLogic.HtmlEditor.WPF.EditorEventArgs

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class SymbolDialog
    ''' </summary>
    Partial Public Class SymbolDialog
        Implements ISymbolDialog

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SymbolDialog" /> class.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            BuildButtons()
        End Sub

        ''' <summary>
        ''' Occurs when [symbol button clicked].
        ''' </summary>
        Public Event SymbolButtonClicked As EventHandler(Of SymbolEventArgBase) Implements ISymbolDialogBase.SymbolButtonClicked

        ''' <summary>
        ''' Occurs when [dialog closed].
        ''' </summary>
        Public Event DialogClosed As EventHandler(Of EventArgs) Implements ISymbolDialogBase.DialogClosed


        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub

        Public Overloads Function ShowDialog() As Boolean? Implements IDialogBase.ShowDialog
            Return MyBase.ShowDialog()
        End Function

        Public Property IDialog_Owner As Window Implements IDialog.Owner
            Get
                Return MyBase.Owner
            End Get
            Set(value As Window)
                MyBase.Owner = value
            End Set
        End Property


        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub


        ''' <summary>
        ''' Builds the buttons.
        ''' </summary>
        Private Sub BuildButtons()
            BuildButton(" ", "nbsp") ' "#0020");
            BuildButton("""", "quot") '"#22");
            BuildButton(ChrW(&HA1), "iexcl") '"#00A1");
            BuildButton(ChrW(&HA2), "cent") ' "#00A2");
            BuildButton(ChrW(&HA3), "pound") '"#00A3");
            BuildButton(ChrW(&HA4), "curren") ' "#00A4");
            BuildButton(ChrW(&HA5), "yen") ' "#00A5");
            BuildButton(ChrW(&HA6), "brvbar") '"#00A6");
            BuildButton(ChrW(&HA7), "sect") ' "#00A7");
            BuildButton(ChrW(&HA8), "uml") ' "#00A8");
            BuildButton(ChrW(&HA9), "copy") ' "#00A9");
            BuildButton(ChrW(&HAA), "ordf") ' "#00AA");
            BuildButton(ChrW(&HAB), "laquo") ' "#00AB");
            BuildButton(ChrW(&HAC), "not") ' "#00AC");
            'empty
            BuildButton(ChrW(&HAE), "reg") ' "#00AE");
            BuildButton(ChrW(&HAF), "macr") ' "#00AF");
            BuildButton(ChrW(&HB0), "deg") ' "#00B0");
            BuildButton(ChrW(&HB1), "plusmn") ' "#00B1");
            BuildButton(ChrW(&HB2), "sup2") ' "#00B2");
            BuildButton(ChrW(&HB3), "sup3") ' "#00B3");
            BuildButton(ChrW(&HB4), "acute") ' "#00B4");
            BuildButton(ChrW(&HB5), "micro") ' "#00B5");
            BuildButton(ChrW(&HB6), "para") ' "#00B6");
            BuildButton(ChrW(&HB7), "middot") ' "#00B7");
            BuildButton(ChrW(&HB8), "cedil") ' "#00B8");
            BuildButton(ChrW(&HB9), "sup1") ' "#00B9");
            BuildButton(ChrW(&HBA), "ordm") ' "#00BA");
            BuildButton(ChrW(&HBB), "raquo") ' "#00BB");
            BuildButton(ChrW(&HBC), "frac14") ' "#00BC");
            BuildButton(ChrW(&HBD), "frac12") ' "#00BD");
            BuildButton(ChrW(&HBE), "frac34") ' "#00BE");
            BuildButton(ChrW(&HBF), "iquest") ' "#00BF");
            BuildButton(ChrW(&HD7), "times") ' "#00D7");
            BuildButton(ChrW(&HD8), "Oslash") ' "#00D8");
            BuildButton(ChrW(&HF7), "divide") ' "#00F7");
            BuildButton(ChrW(&HF8), "oslash") ' "#00F8");
            BuildButton(ChrW(&H192), "fnof") ' "#00192");
            BuildButton(ChrW(&H2C6), "circ") ' "#002C6");
            BuildButton(ChrW(&H2DC), "tilde") ' "#002DC");
            BuildButton(ChrW(&H2013), "ndash") ' "#002013");
            BuildButton(ChrW(&H2014), "mdash") ' "#002014");
            BuildButton(ChrW(&H2018), "lsquo") ' "#002018");
            BuildButton(ChrW(&H2019), "rsquo") ' "#002019");
            BuildButton(ChrW(&H201A), "sbquo") ' "#00201A");
            BuildButton(ChrW(&H201C), "ldquo") ' "#00201C");
            BuildButton(ChrW(&H201D), "rdquo") ' "#00201D");
            BuildButton(ChrW(&H201E), "bdquo") ' "#00201E");
            BuildButton(ChrW(&H2020), "dagger") ' "#002020");
            BuildButton(ChrW(&H2021), "Dagger") ' "#002021");
            BuildButton(ChrW(&H2022), "bull") ' "#002022");
            BuildButton(ChrW(&H2026), "hellip") ' "#002026");
            BuildButton(ChrW(&H2030), "permil") ' "#002030");
            BuildButton(ChrW(&H2039), "lsaquo") ' "#002039");
            BuildButton(ChrW(&H203A), "rsaquo") ' "#00203A");
            BuildButton(ChrW(&H20AC), "euro") ' "#0020AC");
            BuildButton(ChrW(&H2122), "trade") ' "#002122");
            BuildButton(ChrW(&HC0), "Agrave") ' "#00C0");
            BuildButton(ChrW(&HC1), "Aacute") ' "#00C1");
            BuildButton(ChrW(&HC2), "Acirc")
            BuildButton(ChrW(&HC3), "Atilde") ' "#00C3");
            BuildButton(ChrW(&HC4), "Auml") ' "#00C4");
            BuildButton(ChrW(&HC5), "Aring") ' "#00C5");
            BuildButton(ChrW(&HC6), "AElig") ' "#00C6");
            BuildButton(ChrW(&HC7), "Ccedil") ' "#00C7");
            BuildButton(ChrW(&HC8), "Egrave") ' "#00C8");
            BuildButton(ChrW(&HC9), "Eacute") ' "#00C9");

            BuildButton(ChrW(&HCA), "Ecirc") ' "#00CA");
            BuildButton(ChrW(&HCB), "Euml") ' "#00CB");
            BuildButton(ChrW(&HCC), "Igrave") ' "#00CC");
            BuildButton(ChrW(&HCD), "Iacute") ' "#00CD");
            BuildButton(ChrW(&HCE), "Icirc") ' "#00CE");
            BuildButton(ChrW(&HCF), "Iuml") ' "#00CF");
            BuildButton(ChrW(&HD0), "ETH") ' "#00D0");
            BuildButton(ChrW(&HD1), "Ntilde") ' "#00D1");
            BuildButton(ChrW(&HD2), "Ograve") ' "#00D2");
            BuildButton(ChrW(&HD3), "Oacute") ' "#00D3");
            BuildButton(ChrW(&HD4), "Ocirc") ' "#00D4");
            BuildButton(ChrW(&HD5), "Otilde") ' "#00D5");
            BuildButton(ChrW(&HD6), "Ouml") ' "#00D6");
            BuildButton(ChrW(&HD7), "times") ' "#00D7");
            BuildButton(ChrW(&HD8), "Oslash") ' "#00D8");
            BuildButton(ChrW(&HD9), "Ugrave") ' "#00D9");
            BuildButton(ChrW(&HDA), "Uacute") ' "#00DA");
            BuildButton(ChrW(&HDB), "Ucirc") ' "#00DB");
            BuildButton(ChrW(&HDC), "Uuml") ' "#00DC");
            BuildButton(ChrW(&HDD), "Yacute") ' "#00DD");
            BuildButton(ChrW(&HDE), "THORN") ' "#00DE");
            BuildButton(ChrW(&HDF), "szlig") ' "#00DF");
            BuildButton(ChrW(&HE0), "agrave") ' "#00E0");
            BuildButton(ChrW(&HE1), "aacute") ' "#00E1");
            BuildButton(ChrW(&HE2), "acirc") ' "#00E2");
            BuildButton(ChrW(&HE3), "atilde") ' "#00E3");
            BuildButton(ChrW(&HE4), "auml") ' "#00E4");
            BuildButton(ChrW(&HE5), "aring") ' "#00E5");
            BuildButton(ChrW(&HE6), "aelig") ' "#00E6");
            BuildButton(ChrW(&HE7), "ccedil") ' "#00E7");
            BuildButton(ChrW(&HE8), "egrave") ' "#00E8");
            BuildButton(ChrW(&HE9), "eacute") ' "#00E9");
            BuildButton(ChrW(&HEA), "ecirc") ' "#00EA");
            BuildButton(ChrW(&HEB), "euml") ' "#00EB");
            BuildButton(ChrW(&HEC), "igrave") ' "#00EC");
            BuildButton(ChrW(&HED), "iacute") ' "#00ED");
            BuildButton(ChrW(&HEE), "icirc") ' "#00EE");
            BuildButton(ChrW(&HEF), "iuml") ' "#00EF");
            BuildButton(ChrW(&HF0), "eth") ' "#00F0");
            BuildButton(ChrW(&HF1), "ntilde") ' "#00F1");
            BuildButton(ChrW(&HF2), "ograve") ' "#00F2");
            BuildButton(ChrW(&HF3), "oacute") ' "#00F3");
            BuildButton(ChrW(&HF4), "ocirc") ' "#00F4");
            BuildButton(ChrW(&HF5), "otilde") ' "#00F5");
            BuildButton(ChrW(&HF6), "ouml") ' "#00F6");
            BuildButton(ChrW(&HF7), "divide") ' "#00F7");
            BuildButton(ChrW(&HF8), "oslash") ' "#00F8");
            BuildButton(ChrW(&HF9), "ugrave") ' "#00F9");
            BuildButton(ChrW(&HFA), "uacute") ' "#00FA");
            BuildButton(ChrW(&HFB), "ucirc") ' "#00FB");
            BuildButton(ChrW(&HFC), "uuml") ' "#00FC");
            BuildButton(ChrW(&HFD), "yacute") ' "#00FD");
            BuildButton(ChrW(&HFE), "thorn") ' "#00FE");
            BuildButton(ChrW(&HFF), "yuml") ' "#00FF");
            BuildButton(ChrW(&H152), "OElig") ' "#0152");
            BuildButton(ChrW(&H153), "oelig") ' "#0153");
            BuildButton(ChrW(&H160), "Scaron") ' "#0160");
            BuildButton(ChrW(&H161), "scaron") ' "#0161");
            BuildButton(ChrW(&H178), "Yuml") ' "#0178");
            BuildButton(ChrW(&HA2), "cent") ' "#00A2");
            BuildButton("@", "#64") ' "#0040");
            BuildButton(ChrW(&H3A9), "Omega") ' "#03A9");

        End Sub

        ''' <summary>
        ''' Builds the button.
        ''' </summary>
        ''' <param name="displayText">The display text.</param>
        ''' <param name="value">The value.</param>
        Private Sub BuildButton(displayText As String, value As Object)
            Dim btnNew As New SymbolBlock With {
                .SymbolChar = displayText,
                .Cursor = Cursors.Hand,
                .Width = 34,
                .Height = 33,
                .Margin = New Thickness(3)
            }
            SymbolPanel.Children.Add(btnNew)
            AddHandler btnNew.LeftMouseDown, Sub(__, ___) InsertSymbol($"&{value};")
        End Sub

        ''' <summary>
        ''' Inserts the symbol.
        ''' </summary>
        ''' <param name="symbol">The symbol.</param>
        Private Sub InsertSymbol(symbol As String)
            RaiseEvent SymbolButtonClicked(Me, New SymbolEventArg(symbol))
        End Sub

        ''' <summary>
        ''' Handles the Closed event of the SymbolDialog control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub SymbolDialog_Closed(sender As Object, eventArgs As EventArgs)
            RaiseEvent DialogClosed(Me, eventArgs)
        End Sub

        ''' <summary>
        ''' Handles the LeftButtonClicked and RightButtonClicked events of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub CloseButtonClicked(sender As Object, e As RoutedEventArgs)
            Close()
        End Sub
    End Class
End Namespace
