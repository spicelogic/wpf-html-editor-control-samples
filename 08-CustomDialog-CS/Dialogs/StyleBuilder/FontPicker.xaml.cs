using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Interaction logic for FontPicker.xaml
/// </summary>
public partial class FontPicker
{
    /// <summary>
    /// List of font names
    /// it is synchronized with the list box of selected fonts
    /// </summary>
    readonly ObservableCollection<string> _lSelectedFonts = [];

    #region Exposed methods
    /// <summary>
    /// Initializes a new instance of the <see cref="FontPicker"/> class.
    /// </summary>
    /// <param name="initFontList">The init font list.</param>
    public FontPicker(string initFontList)
    {
        // handle selected fonts
        string[] arrFonts = initFontList.Split(',');
        foreach (string aFontName in arrFonts)
        {
            string candidate = aFontName.Trim();
            if (candidate.Length >= 2 && candidate[0] == '\'' && candidate[candidate.Length - 1] == '\'')
                candidate = candidate.Substring(1, candidate.Length - 2);
            if (candidate.Length == 0)
                continue;
            _lSelectedFonts.Add(candidate);
        }

        InitializeComponent();
    }

    /// <summary>
    /// Comma-separeted list of fonts
    /// </summary>
    /// <value>The selected font list.</value>
    public string SelectedFontList
    {
        get
        {
            // Build from list
            StringBuilder sb = new StringBuilder();
            bool first = true;                              // A flag indicating we're going to write the first entry

            // Iterate through selected fonts
            foreach (string aFontname in _lSelectedFonts)
            {
                // Handle first entry
                if (!first)
                    sb.Append(", ");
                else
                    first = false;

                // Append font's name
                if (aFontname.Contains(" "))
                {
                    sb.Append('\'');
                    sb.Append(aFontname);
                    sb.Append('\'');
                }
                else
                    sb.Append(aFontname);
            }

            // done
            return sb.ToString();
        }
    }
    #endregion

    /// <summary>
    /// Handles the Loaded event of the FontPicker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void fontPicker_Loaded(object sender, RoutedEventArgs e)
    {
        // populate list of selected fonts
        LbSelectedFonts.ItemsSource = _lSelectedFonts;

        // populate list of installed fonts
        LbInstalledFonts.ItemsSource = System.Drawing.FontFamily.Families.Select(fam => fam.Name).OrderBy(f => f.ToString());

        // set some buttons' enable status
        lbSelectedFonts_SelectedChanged(this, new RoutedEventArgs());
        tbCustomFont_TextChanged(this, new RoutedEventArgs());
    }

    #region Adding fonts to the selected list

    /// <summary>
    /// Handles the Click event of the BtAddInstalledFont control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btAddInstalledFont_Click(object sender, RoutedEventArgs e)
    {
        string sFont = (string) LbInstalledFonts.SelectedItem;
        if (!string.IsNullOrEmpty(sFont))
        {
            _lSelectedFonts.Add(sFont);
            LbInstalledFonts.SelectedItem = null;
        }
    }

    /// <summary>
    /// Handles the Click event of the BtAddGenericFont control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btAddGenericFont_Click(object sender, RoutedEventArgs e)
    {
        string sFont = CbGenericFonts.Text;
        if (!string.IsNullOrEmpty(sFont))
        {
            _lSelectedFonts.Add(sFont);
            CbGenericFonts.SelectedItem = null;
        }
    }

    /// <summary>
    /// Handles the Click event of the BrAddCustomFont control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void brAddCustomFont_Click(object sender, RoutedEventArgs e)
    {
        _lSelectedFonts.Add(TbCustomFont.Text);
    }
    #endregion

    #region Selected fonts handling
    /// <summary>
    /// Handles the Click event of the BtMoveUp control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btMoveUp_Click(object sender, RoutedEventArgs e)
    {
        int sel = LbSelectedFonts.SelectedIndex;
        if (sel > 0)
        {
            // exchange items in the list box
            string oCurrent = (string)LbSelectedFonts.SelectedItem;
            _lSelectedFonts.RemoveAt(sel);
            _lSelectedFonts.Insert(sel - 1, oCurrent);
            LbSelectedFonts.SelectedIndex = sel - 1;
        }
    }

    /// <summary>
    /// Handles the Click event of the BtMoveDown control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btMoveDown_Click(object sender, RoutedEventArgs e)
    {
        int sel = LbSelectedFonts.SelectedIndex;
        if (sel != -1 && sel < _lSelectedFonts.Count - 1)
        {
            // exchange items in the list box
            string oCurrent = (string)LbSelectedFonts.SelectedItem;
            _lSelectedFonts.RemoveAt(sel);
            _lSelectedFonts.Insert(sel + 1, oCurrent);
            LbSelectedFonts.SelectedIndex = sel + 1;
        }
    }

    /// <summary>
    /// Handles the Click event of the BtRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btRemove_Click(object sender, RoutedEventArgs e)
    {
        int sel = LbSelectedFonts.SelectedIndex;
        if (sel != -1) // precaution
        {
            // remove from local list
            _lSelectedFonts.RemoveAt(sel);
        }
    }

    /// <summary>
    /// Handles the SelectedChanged event of the LbSelectedFonts control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void lbSelectedFonts_SelectedChanged(object sender, RoutedEventArgs e)
    {
        int sel = LbSelectedFonts.SelectedIndex;
        if (sel != -1)
        {
            BtRemove.IsEnabled = true;
            BtMoveUp.IsEnabled = sel != 0;
            BtMoveDown.IsEnabled = sel != _lSelectedFonts.Count - 1;
        }
        else // sel == -1 => disable some buttons
        {
            BtMoveUp.IsEnabled = BtMoveDown.IsEnabled = BtRemove.IsEnabled = false;
        }
    }
    #endregion

    /// <summary>
    /// Handles the Click event of the BtOk control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btOk_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }

    /// <summary>
    /// Handles the Click event of the BtCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    /// <summary>
    /// Handles the TextChanged event of the TbCustomFont control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void tbCustomFont_TextChanged(object sender, RoutedEventArgs e)
    {
        BtAddCustomFont.IsEnabled = TbCustomFont.Text.Trim().Length > 0 && !TbCustomFont.Text.Contains(",");
    }
}