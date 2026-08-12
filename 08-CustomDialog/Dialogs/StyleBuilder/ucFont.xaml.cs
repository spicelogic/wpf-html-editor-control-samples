using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;
using SpiceLogic.HtmlEditor.WPF.Extensions;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.ToolbarModule;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucFont
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Font", "font;font-family;font-size;text-decoration;font-weight;text-transform;color;font-style;font-variant")]
public partial class UcFont : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    /// <summary>
    /// A method creating the color dialog
    /// </summary>
    private readonly DialogFactoryDelegates.CreateColorPickerDialogDelegate _createColorDialogMethod;

    #region Preset of possible value

    /// <summary>
    /// The _ system fonts
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _systemFonts = [];
    /// <summary>
    /// The _ font style
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _fontStyle = [];
    /// <summary>
    /// The _ font variant
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _fontVariant = [];
    /// <summary>
    /// The _ bold absolute
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _boldAbsolute = [];
    /// <summary>
    /// The _ bold relative
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _boldRelative = [];
    /// <summary>
    /// The _ text transform
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _textTransform = [];
    #endregion

    /// <summary>
    /// Creates the lists.
    /// </summary>
    private void CreateLists()
    {
        #region Initialize presets
        _systemFonts.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _systemFonts.Add(new KeyValuePair<string, string>("Window caption", "caption"));
        _systemFonts.Add(new KeyValuePair<string, string>("ToolWindow caption", "small-caption"));
        _systemFonts.Add(new KeyValuePair<string, string>("Dialog text", "message-box"));
        _systemFonts.Add(new KeyValuePair<string, string>("Icon labels", "icon"));
        _systemFonts.Add(new KeyValuePair<string, string>("Menu text", "menu"));
        _systemFonts.Add(new KeyValuePair<string, string>("Tooltip text", "status-bar"));

        _fontStyle.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _fontStyle.Add(new KeyValuePair<string, string>("Normal", "normal"));
        _fontStyle.Add(new KeyValuePair<string, string>("Italic", "italic"));

        _fontVariant.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _fontVariant.Add(new KeyValuePair<string, string>("Normal", "normal"));
        _fontVariant.Add(new KeyValuePair<string, string>("Small Caps", "small-caps"));

        _boldAbsolute.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _boldAbsolute.Add(new KeyValuePair<string, string>("Normal", "normal"));
        _boldAbsolute.Add(new KeyValuePair<string, string>("Bold", "bold"));

        _boldRelative.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _boldRelative.Add(new KeyValuePair<string, string>("Lighter", "lighter"));
        _boldRelative.Add(new KeyValuePair<string, string>("Bolder", "bolder"));

        _textTransform.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _textTransform.Add(new KeyValuePair<string, string>("None", "none"));
        _textTransform.Add(new KeyValuePair<string, string>("Initial Cap", "capitalize"));
        _textTransform.Add(new KeyValuePair<string, string>("lowercase", "lowercase"));
        _textTransform.Add(new KeyValuePair<string, string>("UPPERCASE", "uppercase"));
        #endregion
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UcFont"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    /// <param name="createColorDialogMethod">A method creating the color dialog</param>
    public UcFont(Dictionary<string, string> dict, DialogFactoryDelegates.CreateColorPickerDialogDelegate createColorDialogMethod) //the argument order is important (Activator.CreateInstance uses it)
    {
        _dict = dict;
        _createColorDialogMethod = createColorDialogMethod;
        CreateLists();
        InitializeComponent();

    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("font-family");
        _dict.Remove("font");
        _dict.Remove("color");
        _dict.Remove("font-style");
        _dict.Remove("font-variant");
        _dict.Remove("font-weight");
        _dict.Remove("font-size");
        _dict.Remove("text-decoration");

        {   // handle text decoration
            StringBuilder sb = new StringBuilder();
            if (CbEffectNone.IsChecked == true)
                sb.Append(" none");
            if (CbEffectUnderline.IsChecked == true)
                sb.Append(" underline");
            if (CbEffectStrikethrough.IsChecked == true)
                sb.Append(" line-through");
            if (CbEffectOverline.IsChecked == true)
                sb.Append(" overline");
            _dict["text-decoration"] = sb.ToString();
        }

        _dict["text-transform"] = (string)CbCapitalization.SelectedValue;

        if (RbFamily.IsChecked == true)
        {
            _dict["font-family"] = TbFontFamily.Text;
            _dict["color"] = WpfColorTranslator.ToHtml(((SolidColorBrush)TxtForeColor.Background).Color); // cbColor.Text.ToLowerInvariant();
            _dict["font-style"] = (string)CbFontStyle.SelectedValue;
            _dict["font-variant"] = (string)CbFontVariant.SelectedValue;

            // font-size
            if (RbSizeSpecific.IsChecked == true)
            {
                if (TbSpecificSize.Text.Trim().Length > 0)
                    _dict["font-size"] = TbSpecificSize.Text + CbSpecificSizeType.Text;
            }
            else if (RbSizeAbsolute.IsChecked == true)
                _dict["font-size"] = CbAbsoluteSize.Text.ToLowerInvariant();
            else
                _dict["font-size"] = CbRelativeSize.Text.ToLowerInvariant();

            // font-weight
            if (RbBoldAbsolute.IsChecked == true)
                _dict["font-weight"] = (string)CbBoldAbsolute.SelectedValue;
            else
                _dict["font-weight"] = (string)CbBoldRelative.SelectedValue;
        }

        if (RbSystemFont.IsChecked == true)
            _dict["font"] = (string)CbSystemFont.SelectedValue;
    }

    /// <summary>
    /// Handles the Loaded event of the ucFont control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucFont_Loaded(object sender, RoutedEventArgs e)
    {
        #region set data sources
        CbSystemFont.ItemsSource = _systemFonts;
        CbSystemFont.DisplayMemberPath = "Key";
        CbSystemFont.SelectedValuePath = "Value";
        CbSystemFont.SelectedIndex = 0;

        CbFontStyle.ItemsSource = _fontStyle;
        CbFontStyle.DisplayMemberPath = "Key";
        CbFontStyle.SelectedValuePath = "Value";
        CbFontStyle.SelectedIndex = 0;

        CbFontVariant.ItemsSource = _fontVariant;
        CbFontVariant.DisplayMemberPath = "Key";
        CbFontVariant.SelectedValuePath = "Value";
        CbFontVariant.SelectedIndex = 0;

        CbBoldAbsolute.ItemsSource = _boldAbsolute;
        CbBoldAbsolute.DisplayMemberPath = "Key";
        CbBoldAbsolute.SelectedValuePath = "Value";
        CbBoldAbsolute.SelectedIndex = 0;

        CbBoldRelative.ItemsSource = _boldRelative;
        CbBoldRelative.DisplayMemberPath = "Key";
        CbBoldRelative.SelectedValuePath = "Value";
        CbBoldRelative.SelectedIndex = 0;

        CbCapitalization.ItemsSource = _textTransform;
        CbCapitalization.DisplayMemberPath = "Key";
        CbCapitalization.SelectedValuePath = "Value";
        CbCapitalization.SelectedIndex = 0;
        #endregion

        // all radio-group defaults (RbFamily,
        // RbSizeSpecific, RbBoldAbsolute) are declared in the XAML
        // (IsChecked="True"). No runtime seeding -- a host customizer who
        // picks a different default in the XAML is honored.

        #region parse

        if (_dict.TryGetValue("color", out var value))
            TxtForeColor.Background = new SolidColorBrush(WpfColorTranslator.FromHtml(value));

        if (_dict.TryGetValue("font-style", out value))
        {
            for (int i = 0, n = _fontStyle.Count; i < n; ++i)
            {
                if (string.Equals(value, _fontStyle[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbFontStyle.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("font-size", out value))
        {
            bool handled = false;

            for (int i = 0, n = CbAbsoluteSize.Items.Count; i < n && !handled; ++i)
                if (string.Equals(value, (CbAbsoluteSize.Items[i] as ComboBoxItem)?.Content as string, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAbsoluteSize.SelectedIndex = i;
                    RbSizeAbsolute.IsChecked = true;
                    handled = true;
                }

            for (int i = 0, n = CbRelativeSize.Items.Count; i < n && !handled; ++i)
                if (string.Equals(value, (CbRelativeSize.Items[i] as ComboBoxItem)?.Content as string, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbRelativeSize.SelectedIndex = i;
                    RbSizeRelative.IsChecked = true;
                    handled = true;
                }

            if (!handled)
            {
                RbSizeSpecific.IsChecked = true;
                for (int i = 0, n = CbSpecificSizeType.Items.Count; i < n && !handled; ++i)
                {
                    string cbSpecificSizeTypeItem = (CbSpecificSizeType.Items[i] as ComboBoxItem)?.Content as string;
                    if (!string.IsNullOrEmpty(cbSpecificSizeTypeItem) && value.EndsWith(cbSpecificSizeTypeItem, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbSpecificSizeType.SelectedIndex = i;
                        TbSpecificSize.Text = value.Substring(0, value.Length - (cbSpecificSizeTypeItem.Length));
                        handled = true;
                    }
                }
            }
        }

        if (_dict.TryGetValue("font-weight", out value))
        {
            bool handled = false;

            for (int i = 0, n = _boldAbsolute.Count; i < n && !handled; ++i)
            {
                if (string.Equals(value, _boldAbsolute[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbBoldAbsolute.SelectedIndex = i;
                    RbBoldAbsolute.IsChecked = true;
                    handled = true;
                }
            }

            for (int i = 0, n = _boldRelative.Count; i < n && !handled; ++i)
            {
                if (string.Equals(value, _boldRelative[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbBoldRelative.SelectedIndex = i;
                    RbBoldRelative.IsChecked = true;
                    handled = true;
                }
            }
        }

        if (_dict.TryGetValue("text-decoration", out value))
        {
            string loValue = value.ToLowerInvariant();
            CbEffectUnderline.IsChecked = loValue.Contains("underline");
            CbEffectStrikethrough.IsChecked = loValue.Contains("line-through") || loValue.Contains("linethrough");
            CbEffectOverline.IsChecked = loValue.Contains("overline");
            CbEffectNone.IsChecked = loValue.Contains("none");
        }

        if (_dict.TryGetValue("text-transform", out value))
        {
            for (int i = 0, n = _textTransform.Count; i < n; ++i)
                if (string.Equals(value, _textTransform[i].Value, StringComparison.InvariantCultureIgnoreCase))
                    CbCapitalization.SelectedIndex = i;
        }

        if (_dict.TryGetValue("font-family", out value))
        {
            RbFamily.IsChecked = true;
            TbFontFamily.Text = value;
        }

        if (_dict.TryGetValue("font", out value))
        {
            RbSystemFont.IsChecked = true;

            for (int i = 0, n = _systemFonts.Count; i < n; ++i)
            {
                if (string.Equals((value ?? "").Replace("-", "").Trim(), _systemFonts[i].Value.Replace("-", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    CbSystemFont.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion
    }

    #region UI handlers
    /// <summary>
    /// Handles the Click event of the BtFontFamilySelect control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btFontFamilySelect_Click(object sender, RoutedEventArgs e)
    {
        FontPicker subForm = new FontPicker(TbFontFamily.Text);
        if (subForm.ShowDialog() == true)
            TbFontFamily.Text = subForm.SelectedFontList;
    }

    /// <summary>
    /// Fonts the type changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void FontTypeChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        TbFontFamily.IsEnabled = btFontFamilySelect.IsEnabled = RbFamily.IsChecked == true;
        CbSystemFont.IsEnabled = !RbFamily.IsChecked == true;

        GbSize.IsEnabled = GbBold.IsEnabled = CbFontStyle.IsEnabled = CbFontVariant.IsEnabled = RbFamily.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the RbSizeSpecific control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void rbSizeSpecific_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        TbSpecificSize.IsEnabled = CbSpecificSizeType.IsEnabled = true;
        CbRelativeSize.IsEnabled = CbAbsoluteSize.IsEnabled = false;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the RbSizeAbsolute control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void rbSizeAbsolute_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        CbAbsoluteSize.IsEnabled = true;
        TbSpecificSize.IsEnabled = CbSpecificSizeType.IsEnabled = CbRelativeSize.IsEnabled = false;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the RbSizeRelative control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void rbSizeRelative_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        CbRelativeSize.IsEnabled = true;
        TbSpecificSize.IsEnabled = CbSpecificSizeType.IsEnabled = CbAbsoluteSize.IsEnabled = false;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the cbEffectNone control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbEffectNone_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        if (CbEffectNone.IsChecked == true)
        {
            CbEffectOverline.IsChecked = CbEffectStrikethrough.IsChecked = CbEffectUnderline.IsChecked = false;
            CbEffectOverline.IsEnabled = CbEffectStrikethrough.IsEnabled = CbEffectUnderline.IsEnabled = false;
        }
        else
        {
            CbEffectOverline.IsEnabled = CbEffectStrikethrough.IsEnabled = CbEffectUnderline.IsEnabled = true;
        }
    }

    /// <summary>
    /// Rbs the bold radio button changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void RbBoldRadioButtonChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        CbBoldAbsolute.IsEnabled = RbBoldAbsolute.IsChecked == true;
        CbBoldRelative.IsEnabled = RbBoldRelative.IsChecked == true;
    }
    #endregion

        
    /// <summary>
    /// Handles the PreviewMouseLeftButtonUp events of the TxtForeColor control.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void TxtForeColor_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (TxtForeColor.Background is SolidColorBrush solidColorBrush)
        {
            using IColorPickerDialog colorDialog = _createColorDialogMethod();
            colorDialog.StartingColor = solidColorBrush.Color;
            if (colorDialog.ShowDialog() == true)
                TxtForeColor.Background = new SolidColorBrush(colorDialog.SelectedColor);
        }
    }
}