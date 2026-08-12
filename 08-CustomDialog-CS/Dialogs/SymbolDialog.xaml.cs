using System;
using System.Windows;
using System.Windows.Input;
using SpiceLogic.HtmlEditor.WPF.Models.BOs.EditorEventArgs;
using SpiceLogic.HtmlEditor.WPF.Controls;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.EditorEventArgs;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class SymbolDialog
/// </summary>
public partial class SymbolDialog : ISymbolDialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SymbolDialog" /> class.
    /// </summary>
    public SymbolDialog()
    {
        InitializeComponent();

        BuildButtons();
    }

    /// <summary>
    /// Occurs when [symbol button clicked].
    /// </summary>
    public event EventHandler<SymbolEventArgBase> SymbolButtonClicked;

    /// <summary>
    /// Occurs when [dialog closed].
    /// </summary>
    public event EventHandler<EventArgs> DialogClosed;


    public void Dispose()
    {
    }


    /// <summary>
    /// Handles the MouseLeftButtonDown event of the DialogHeader control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }


    /// <summary>
    /// Builds the buttons.
    /// </summary>
    private void BuildButtons()
    {
        BuildButton(" ", "nbsp");// "#0020");
        BuildButton("\"", "quot");//"#22");
        BuildButton("¡", "iexcl");//"#00A1");
        BuildButton("¢", "cent");// "#00A2");
        BuildButton("£", "pound");//"#00A3");
        BuildButton("¤", "curren");// "#00A4");
        BuildButton("¥", "yen");// "#00A5");
        BuildButton("¦", "brvbar");//"#00A6");
        BuildButton("§", "sect");// "#00A7");
        BuildButton("¨", "uml");// "#00A8");
        BuildButton("©", "copy");// "#00A9");
        BuildButton("ª", "ordf");// "#00AA");
        BuildButton("«", "laquo");// "#00AB");
        BuildButton("¬", "not");// "#00AC");
        //empty
        BuildButton("®", "reg");// "#00AE");
        BuildButton("¯", "macr");// "#00AF");
        BuildButton("°", "deg");// "#00B0");
        BuildButton("±", "plusmn");// "#00B1");
        BuildButton("²", "sup2");// "#00B2");
        BuildButton("³", "sup3");// "#00B3");
        BuildButton("´", "acute");// "#00B4");
        BuildButton("µ", "micro");// "#00B5");
        BuildButton("¶", "para");// "#00B6");
        BuildButton("·", "middot");// "#00B7");
        BuildButton("¸", "cedil");// "#00B8");
        BuildButton("¹", "sup1");// "#00B9");
        BuildButton("º", "ordm");// "#00BA");
        BuildButton("»", "raquo");// "#00BB");
        BuildButton("¼", "frac14");// "#00BC");
        BuildButton("½", "frac12");// "#00BD");
        BuildButton("¾", "frac34");// "#00BE");
        BuildButton("¿", "iquest");// "#00BF");
        BuildButton("×", "times");// "#00D7");
        BuildButton("Ø", "Oslash");// "#00D8");
        BuildButton("÷", "divide");// "#00F7");
        BuildButton("ø", "oslash");// "#00F8");
        BuildButton("ƒ", "fnof");// "#00192");
        BuildButton("ˆ", "circ");// "#002C6");
        BuildButton("˜", "tilde");// "#002DC");
        BuildButton("–", "ndash");// "#002013");
        BuildButton("—", "mdash");// "#002014");
        BuildButton("‘", "lsquo");// "#002018");
        BuildButton("’", "rsquo");// "#002019");
        BuildButton("‚", "sbquo");// "#00201A");
        BuildButton("“", "ldquo");// "#00201C");
        BuildButton("”", "rdquo");// "#00201D");
        BuildButton("„", "bdquo");// "#00201E");
        BuildButton("†", "dagger");// "#002020");
        BuildButton("‡", "Dagger");// "#002021");
        BuildButton("•", "bull");// "#002022");
        BuildButton("…", "hellip");// "#002026");
        BuildButton("‰", "permil");// "#002030");
        BuildButton("‹", "lsaquo");// "#002039");
        BuildButton("›", "rsaquo");// "#00203A");
        BuildButton("€", "euro");// "#0020AC");
        BuildButton("™", "trade");// "#002122");
        BuildButton("À", "Agrave");// "#00C0");
        BuildButton("Á", "Aacute");// "#00C1");
        BuildButton("Â", "Acirc");
        BuildButton("Ã", "Atilde");// "#00C3");
        BuildButton("Ä", "Auml");// "#00C4");
        BuildButton("Å", "Aring");// "#00C5");
        BuildButton("Æ", "AElig");// "#00C6");
        BuildButton("Ç", "Ccedil");// "#00C7");
        BuildButton("È", "Egrave");// "#00C8");
        BuildButton("É", "Eacute");// "#00C9");

        BuildButton("Ê", "Ecirc");// "#00CA");
        BuildButton("Ë", "Euml");// "#00CB");
        BuildButton("Ì", "Igrave");// "#00CC");
        BuildButton("Í", "Iacute");// "#00CD");
        BuildButton("Î", "Icirc");// "#00CE");
        BuildButton("Ï", "Iuml");// "#00CF");
        BuildButton("Ð", "ETH");// "#00D0");
        BuildButton("Ñ", "Ntilde");// "#00D1");
        BuildButton("Ò", "Ograve");// "#00D2");
        BuildButton("Ó", "Oacute");// "#00D3");
        BuildButton("Ô", "Ocirc");// "#00D4");
        BuildButton("Õ", "Otilde");// "#00D5");
        BuildButton("Ö", "Ouml");// "#00D6");
        BuildButton("×", "times");// "#00D7");
        BuildButton("Ø", "Oslash");// "#00D8");
        BuildButton("Ù", "Ugrave");// "#00D9");
        BuildButton("Ú", "Uacute");// "#00DA");
        BuildButton("Û", "Ucirc");// "#00DB");
        BuildButton("Ü", "Uuml");// "#00DC");
        BuildButton("Ý", "Yacute");// "#00DD");
        BuildButton("Þ", "THORN");// "#00DE");
        BuildButton("ß", "szlig");// "#00DF");
        BuildButton("à", "agrave");// "#00E0");
        BuildButton("á", "aacute");// "#00E1");
        BuildButton("â", "acirc");// "#00E2");
        BuildButton("ã", "atilde");// "#00E3");
        BuildButton("ä", "auml");// "#00E4");
        BuildButton("å", "aring");// "#00E5");
        BuildButton("æ", "aelig");// "#00E6");
        BuildButton("ç", "ccedil");// "#00E7");
        BuildButton("è", "egrave");// "#00E8");
        BuildButton("é", "eacute");// "#00E9");
        BuildButton("ê", "ecirc");// "#00EA");
        BuildButton("ë", "euml");// "#00EB");
        BuildButton("ì", "igrave");// "#00EC");
        BuildButton("í", "iacute");// "#00ED");
        BuildButton("î", "icirc");// "#00EE");
        BuildButton("ï", "iuml");// "#00EF");
        BuildButton("ð", "eth");// "#00F0");
        BuildButton("ñ", "ntilde");// "#00F1");
        BuildButton("ò", "ograve");// "#00F2");
        BuildButton("ó", "oacute");// "#00F3");
        BuildButton("ô", "ocirc");// "#00F4");
        BuildButton("õ", "otilde");// "#00F5");
        BuildButton("ö", "ouml");// "#00F6");
        BuildButton("÷", "divide");// "#00F7");
        BuildButton("ø", "oslash");// "#00F8");
        BuildButton("ù", "ugrave");// "#00F9");
        BuildButton("ú", "uacute");// "#00FA");
        BuildButton("û", "ucirc");// "#00FB");
        BuildButton("ü", "uuml");// "#00FC");
        BuildButton("ý", "yacute");// "#00FD");
        BuildButton("þ", "thorn");// "#00FE");
        BuildButton("ÿ", "yuml");// "#00FF");
        BuildButton("Œ", "OElig");// "#0152");
        BuildButton("œ", "oelig");// "#0153");
        BuildButton("Š", "Scaron");// "#0160");
        BuildButton("š", "scaron");// "#0161");
        BuildButton("Ÿ", "Yuml");// "#0178");
        BuildButton("¢", "cent");// "#00A2");
        BuildButton("@", "#64");// "#0040");
        BuildButton("Ω", "Omega");// "#03A9");

    }

    /// <summary>
    /// Builds the button.
    /// </summary>
    /// <param name="displayText">The display text.</param>
    /// <param name="value">The value.</param>
    private void BuildButton(string displayText, object value)
    {
        SymbolBlock btnNew = new SymbolBlock
        {
            SymbolChar = displayText,
            Cursor = Cursors.Hand,
            Width = 34,
            Height = 33,
            Margin = new Thickness(3)
        };
        SymbolPanel.Children.Add(btnNew);
        btnNew.LeftMouseDown += (_, _) => InsertSymbol($"&{value};");
    }

    /// <summary>
    /// Inserts the symbol.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    private void InsertSymbol(string symbol)
    {
        SymbolButtonClicked?.Invoke(this, new SymbolEventArg(symbol));
    }

    /// <summary>
    /// Handles the Closed event of the SymbolDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void SymbolDialog_Closed(object sender, EventArgs eventArgs)
    {
        DialogClosed?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Handles the LeftButtonClicked and RightButtonClicked events of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void CloseButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}