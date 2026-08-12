using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucText
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Text", "text-align,vertical-align,text-justify,letter-spacing,line-height,direction,text-indent")]
public partial class ucText : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    #region Preset of possible values

    /// <summary>
    /// The _ text align
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _textAlign = [];
    /// <summary>
    /// The _ vertical align
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _verticalAlign = [];
    /// <summary>
    /// The _ text justify
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _textJustify = [];
    /// <summary>
    /// The _ letter spacing
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _letterSpacing = [];
    /// <summary>
    /// The _ line height
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _lineHeight = [];
    /// <summary>
    /// The _ direction
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _direction = [];

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ucText"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    public ucText(Dictionary<string, string> dict)
    {
        _dict = dict;

        #region Initialize presets
        _textAlign.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _textAlign.Add(new KeyValuePair<string, string>("Left", "left"));
        _textAlign.Add(new KeyValuePair<string, string>("Center", "center"));
        _textAlign.Add(new KeyValuePair<string, string>("Right", "right"));
        _textAlign.Add(new KeyValuePair<string, string>("Justified", "justify"));

        _verticalAlign.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _verticalAlign.Add(new KeyValuePair<string, string>("baseline", "baseline"));
        _verticalAlign.Add(new KeyValuePair<string, string>("sub", "sub"));
        _verticalAlign.Add(new KeyValuePair<string, string>("super", "super"));
        _verticalAlign.Add(new KeyValuePair<string, string>("top", "top"));
        _verticalAlign.Add(new KeyValuePair<string, string>("text-top", "text-top"));
        _verticalAlign.Add(new KeyValuePair<string, string>("middle", "middle"));
        _verticalAlign.Add(new KeyValuePair<string, string>("bottom", "bottom"));
        _verticalAlign.Add(new KeyValuePair<string, string>("text-bottom", "text-bottom"));

        _textJustify.Add(new KeyValuePair<string, string>("", ""));
        _textJustify.Add(new KeyValuePair<string, string>("Auto", "auto"));
        _textJustify.Add(new KeyValuePair<string, string>("Space words", "inter-word"));
        _textJustify.Add(new KeyValuePair<string, string>("Newspaper style", "newspaper"));
        _textJustify.Add(new KeyValuePair<string, string>("Distribute spacing", "distribute"));
        _textJustify.Add(new KeyValuePair<string, string>("Distribute all lines", "dibtribute-all-lines"));
        _textJustify.Add(new KeyValuePair<string, string>("Inter-cluster", "inter-cluster"));
        _textJustify.Add(new KeyValuePair<string, string>("Inter-ideograph", "inter-ideograph"));
        _textJustify.Add(new KeyValuePair<string, string>("Kashida", "kashida"));

        _letterSpacing.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _letterSpacing.Add(new KeyValuePair<string, string>("Normal", "normal"));
        _letterSpacing.Add(new KeyValuePair<string, string>("Custom", ""));

        _lineHeight.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _lineHeight.Add(new KeyValuePair<string, string>("Normal", "normal"));
        _lineHeight.Add(new KeyValuePair<string, string>("Custom", ""));

        _direction.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _direction.Add(new KeyValuePair<string, string>("Left to right", "ltr"));
        _direction.Add(new KeyValuePair<string, string>("Right to left", "rtl"));
        #endregion

        InitializeComponent();
    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        // remove previous entries
        _dict.Remove("text-align");
        _dict.Remove("vertical-align");
        _dict.Remove("text-justify");

        _dict.Remove("letter-spacing");
        _dict.Remove("line-height");

        _dict.Remove("direction");
        _dict.Remove("text-indent");

        // save form's data
        _dict["text-align"] = (string)CbAlHorizontal.SelectedValue;
        if (CbAlHorizontal.SelectedIndex == 4)
            _dict["text-justify"] = (string)CbAlJustification.SelectedValue;
        _dict["vertical-align"] = (string)CbAlVertical.SelectedValue;

        _dict["letter-spacing"] = CbSpacingLetters.SelectedIndex != 2 ?
            (string)CbSpacingLetters.SelectedValue :
            TbSpacingLetters.Text + CbSpacingLettersCustom.Text;

        _dict["line-height"] = CbSpacingLines.SelectedIndex != 2 ?
            (string)CbSpacingLines.SelectedValue :
            TbSpacingLines.Text + CbSpacingLinesCustom.Text;

        if (TbTextFlowIndentation.Text.Trim().Length > 0)
            _dict["text-indent"] = TbTextFlowIndentation.Text + CbTextFlowCustom.Text;

        _dict["direction"] = (string)CbTextFlowDirection.SelectedValue;
    }

    /// <summary>
    /// Handles the Loaded event of the ucText control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucText_Loaded(object sender, RoutedEventArgs e)
    {
        #region set data sources
        CbAlHorizontal.ItemsSource = _textAlign;
        CbAlHorizontal.DisplayMemberPath = "Key";
        CbAlHorizontal.SelectedValuePath = "Value";
        CbAlHorizontal.SelectedIndex = 0;

        CbAlVertical.ItemsSource = _verticalAlign;
        CbAlVertical.DisplayMemberPath = "Key";
        CbAlVertical.SelectedValuePath = "Value";
        CbAlVertical.SelectedIndex = 0;

        CbAlJustification.ItemsSource = _textJustify;
        CbAlJustification.DisplayMemberPath = "Key";
        CbAlJustification.SelectedValuePath = "Value";
        CbAlJustification.SelectedIndex = 0;

        CbSpacingLetters.ItemsSource = _letterSpacing;
        CbSpacingLetters.DisplayMemberPath = "Key";
        CbSpacingLetters.SelectedValuePath = "Value";
        CbSpacingLetters.SelectedIndex = 0;

        CbSpacingLines.ItemsSource = _lineHeight;
        CbSpacingLines.DisplayMemberPath = "Key";
        CbSpacingLines.SelectedValuePath = "Value";
        CbSpacingLines.SelectedIndex = 0;

        CbTextFlowDirection.ItemsSource = _direction;
        CbTextFlowDirection.DisplayMemberPath = "Key";
        CbTextFlowDirection.SelectedValuePath = "Value";
        CbTextFlowDirection.SelectedIndex = 0;
        #endregion

        #region parse alignment
        if (_dict.TryGetValue("vertical-align", out var value))
        {
            for (int i = 0, n = _verticalAlign.Count; i < n; ++i)
                if (string.Equals(value, _verticalAlign[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAlVertical.SelectedIndex = i;
                    break;
                }
        }

        if (_dict.TryGetValue("text-justify", out value))
        {
            for (int i = 0, n = _textJustify.Count; i < n; ++i)
                if (string.Equals(value, _textJustify[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAlJustification.SelectedIndex = i;
                    break;
                }
        }

        if (_dict.TryGetValue("text-align", out value))
        {
            for (int i = 0, n = _textAlign.Count; i < n; ++i)
                if (string.Equals(value, _textAlign[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAlHorizontal.SelectedIndex = i;
                    break;
                }
        }
        #endregion

        #region parse spacing
        if (_dict.TryGetValue("letter-spacing", out value))
        {
            bool handled = false;

            for (int i = 0, n = _letterSpacing.Count; i < n && !handled; ++i)
                if (value.Equals(_letterSpacing[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbSpacingLetters.SelectedIndex = i;
                    handled = true;
                }

            if (!handled)
            {
                for (int i = 0, n = CbSpacingLettersCustom.Items.Count; i < n; ++i)
                {
                    if ((CbSpacingLettersCustom.Items[i] as ComboBoxItem)?.Content is string lettersCustom && value.EndsWith(lettersCustom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbSpacingLetters.Text = value.Substring(0, value.Length - (lettersCustom.Length));
                        CbSpacingLettersCustom.SelectedIndex = i;
                        CbSpacingLetters.SelectedIndex = 2;
                        break;
                    }
                }
            }
        }

        if (_dict.TryGetValue("line-height", out value))
        {
            bool handled = false;

            for (int i = 0, n = _lineHeight.Count; i < n && !handled; ++i)
                if (value.Equals(_lineHeight[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbSpacingLines.SelectedIndex = i;
                    handled = true;
                }

            if (!handled)
            {
                for (int i = 0, n = CbSpacingLinesCustom.Items.Count; i < n; ++i)
                {
                    if ((CbSpacingLinesCustom.Items[i] as ComboBoxItem)?.Content is string linesCustom && value.EndsWith(linesCustom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbSpacingLines.Text = value.Substring(0, value.Length - (linesCustom.Length));
                        CbSpacingLinesCustom.SelectedIndex = i;
                        CbSpacingLines.SelectedIndex = 2;
                        break;
                    }
                }
            }
        }
        #endregion

        #region parse text flow
        if (_dict.TryGetValue("text-indent", out value))
        {
            for (int i = 0, n = CbTextFlowCustom.Items.Count; i < n; ++i)
            {
                if ((CbTextFlowCustom.Items[i] as ComboBoxItem)?.Content is string textFlowCustom && value.EndsWith(textFlowCustom, StringComparison.InvariantCultureIgnoreCase))
                {
                    TbTextFlowIndentation.Text = value.Substring(0, value.Length - (textFlowCustom.Length));
                    CbTextFlowCustom.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("direction", out value))
        {
            for (int i = 0, n = _direction.Count; i < n; ++i)
                if (value.Equals(_direction[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbTextFlowDirection.SelectedIndex = i;
                    break;
                }
        }
        #endregion
    }

    #region UI handling
    /// <summary>
    /// Handles the SelectionChanged event of the CbAlHorizontal control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbAlHorizontal_SelectionChanged(object sender, RoutedEventArgs e)
    {
        CbAlJustification.IsEnabled = CbAlHorizontal.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the cbSpacingLetters control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbSpacingLetters_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbSpacingLetters.IsEnabled = CbSpacingLettersCustom.IsEnabled = CbSpacingLetters.SelectedIndex == 2;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the cbSpacingLines control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbSpacingLines_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbSpacingLines.IsEnabled = CbSpacingLinesCustom.IsEnabled = CbSpacingLines.SelectedIndex == 2;
    }
    #endregion
}