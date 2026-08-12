using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;
using SpiceLogic.HtmlEditor.WPF.Extensions;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.ToolbarModule;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucBackground
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Background", "background-color,background-image,background-repeat,background-attachment,background-position-x,background-position-y")]
public partial class ucBackground : IEditorStylePage
{
    #region Preset of possible values

    /// <summary>
    /// The _ bg repeat
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _bgRepeat = [];
    /// <summary>
    /// The _ bg attachment
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _bgAttachment = [];
    /// <summary>
    /// The _ bg position X
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _bgPositionX = [];
    /// <summary>
    /// The _ bg position Y
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _bgPositionY = [];
    #endregion

    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    /// <summary>
    /// A method creating the color dialog
    /// </summary>
    private readonly DialogFactoryDelegates.CreateColorPickerDialogDelegate _createColorDialogMethod;

    /// <summary>
    /// Initializes a new instance of the <see cref="ucBackground"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    /// <param name="createColorDialogMethod">A method creating the color dialog</param>
    public ucBackground(Dictionary<string, string> dict, DialogFactoryDelegates.CreateColorPickerDialogDelegate createColorDialogMethod) //the argument order is important (Activator.CreateInstance uses it)
    {
        #region Initialize presets
        _bgRepeat.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _bgRepeat.Add(new KeyValuePair<string, string>("Tile in horizontal direction", "repeat-x"));
        _bgRepeat.Add(new KeyValuePair<string, string>("Tile in vertical direction", "repeat-y"));
        _bgRepeat.Add(new KeyValuePair<string, string>("Tile in both directions", "repeat"));
        _bgRepeat.Add(new KeyValuePair<string, string>("Do not tile", "no-repeat"));

        _bgAttachment.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _bgAttachment.Add(new KeyValuePair<string, string>("Scrolling background", "scroll"));
        _bgAttachment.Add(new KeyValuePair<string, string>("Fixed background", "fixed"));

        _bgPositionX.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _bgPositionX.Add(new KeyValuePair<string, string>("Left", "left"));
        _bgPositionX.Add(new KeyValuePair<string, string>("Center", "center"));
        _bgPositionX.Add(new KeyValuePair<string, string>("Right", "right"));
        _bgPositionX.Add(new KeyValuePair<string, string>("Custom", ""));

        _bgPositionY.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _bgPositionY.Add(new KeyValuePair<string, string>("Top", "top"));
        _bgPositionY.Add(new KeyValuePair<string, string>("Center", "center"));
        _bgPositionY.Add(new KeyValuePair<string, string>("Bottom", "bottom"));
        _bgPositionY.Add(new KeyValuePair<string, string>("Custom", ""));
        #endregion

        _dict = dict;
        _createColorDialogMethod = createColorDialogMethod;

        InitializeComponent();
    }

    /// <summary>
    /// Handles the Loaded event of the ucBackground control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucBackground_Loaded(object sender, RoutedEventArgs e)
    {
        CbTiling.ItemsSource = _bgRepeat;
        CbTiling.DisplayMemberPath = "Key";
        CbTiling.SelectedValuePath = "Value";
        CbTiling.SelectedIndex = 0;

        CbScrolling.ItemsSource = _bgAttachment;
        CbScrolling.DisplayMemberPath = "Key";
        CbScrolling.SelectedValuePath = "Value";
        CbScrolling.SelectedIndex = 0;

        CbHorizontal.ItemsSource = _bgPositionX;
        CbHorizontal.DisplayMemberPath = "Key";
        CbHorizontal.SelectedValuePath = "Value";
        CbHorizontal.SelectedIndex = 0;

        CbVertical.ItemsSource = _bgPositionY;
        CbVertical.DisplayMemberPath = "Key";
        CbVertical.SelectedValuePath = "Value";
        CbVertical.SelectedIndex = 0;

        if (_dict.TryGetValue("background-color", out var value))
        {
            if ((value ?? "").Trim().Equals("transparent", StringComparison.InvariantCultureIgnoreCase))
                CbBgColorTransparent.IsChecked = true;
            else
            {
                TxtBgColor.Background = new SolidColorBrush(WpfColorTranslator.FromHtml(value));
                CbBgColorTransparent.IsChecked = false;
            }
        }

        if (_dict.TryGetValue("background-repeat", out value))
        {
            string val = value.Trim().ToLowerInvariant();
            for (int i = 0; i < _bgRepeat.Count; ++i)
            {
                if (string.Equals(val, _bgRepeat[i].Value))
                {
                    CbTiling.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("background-attachment", out value))
        {
            string val = value.Trim().ToLowerInvariant();
            for (int i = 0; i < _bgAttachment.Count; ++i)
            {
                if (string.Equals(val, _bgAttachment[i].Value))
                {
                    CbScrolling.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("background-position-x", out value))
        {
            bool handled = false;

            for (int i = 0; i < _bgPositionX.Count && !handled; ++i)
                if (string.Equals(value, _bgPositionX[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbHorizontal.SelectedIndex = i;
                    handled = true;
                }

            if (!handled)
            {
                for (int i = 0; i < CbHorCustType.Items.Count; ++i)
                {
                    if ((CbHorCustType.Items[i] as ComboBoxItem)?.Content is string customMeasureItems && value.EndsWith(customMeasureItems, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbHorizontal.SelectedIndex = 4; // Custom
                        CbHorCustType.SelectedIndex = i;
                        TbHorCust.Text = value.Substring(0, value.Length - (customMeasureItems.Length));
                        break;
                    }
                }
            }
        }

        if (_dict.TryGetValue("background-position-y", out value))
        {
            bool handled = false;

            for (int i = 0; i < _bgPositionY.Count && !handled; ++i)
            {
                if (string.Equals(value, _bgPositionY[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbVertical.SelectedIndex = i;
                    handled = true;
                }
            }

            if (!handled)
            {
                for (int i = 0; i < CbVerCustType.Items.Count; ++i)
                {
                    if ((CbVerCustType.Items[i] as ComboBoxItem)?.Content is string customMeasureItems && value.EndsWith(customMeasureItems, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbVertical.SelectedIndex = 4; // Custom
                        CbVerCustType.SelectedIndex = i;
                        TbVerCust.Text = value.Substring(0, value.Length - (customMeasureItems.Length));
                        break;
                    }
                }
            }
        }

        if (_dict.TryGetValue("background-image", out value))
        {
            if ((value ?? "").Trim().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                CbDoNotUseBackground.IsChecked = true;
            else
            {
                CbDoNotUseBackground.IsChecked = false;
                try
                {
                    if (value != null)
                        TbBgImage.Text = Regex.Match(value, @"url\((.+?)\)", RegexOptions.IgnoreCase).Groups[1].Value;
                }
                catch
                {
                    TbBgImage.Text = value;
                }
            }
        }
        else
            tbBgImage_TextChanged(this, new RoutedEventArgs());
    }

    #region UI handling
        
    /// <summary>
    /// Handles the Checked and Unchecked event of the CbBgColorTransparent control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbBgColorTransparent_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        TxtBgColor.IsEnabled = !CbBgColorTransparent.IsChecked == true;
    }

    /// <summary>
    /// Handles the TextChanged event of the TbBgImage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void tbBgImage_TextChanged(object sender, RoutedEventArgs e)
    {
        GbPosition.IsEnabled = CbTiling.IsEnabled = CbScrolling.IsEnabled = TbBgImage.Text.Trim().Length > 0;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbHorizontal control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbHorizontal_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbHorCust.IsEnabled = CbHorCustType.IsEnabled = CbHorizontal.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbVertical control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbVertical_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbVerCust.IsEnabled = CbVerCustType.IsEnabled = CbVertical.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the CbDoNotUseBackground control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbDoNotUseBackground_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        if (CbDoNotUseBackground.IsChecked == true)
            GbPosition.IsEnabled = TbBgImage.IsEnabled = BtChooseBgImage.IsEnabled = CbTiling.IsEnabled = CbScrolling.IsEnabled = false;
        else
        {
            GbPosition.IsEnabled = TbBgImage.IsEnabled = BtChooseBgImage.IsEnabled = CbTiling.IsEnabled = CbScrolling.IsEnabled = true;
            tbBgImage_TextChanged(this, new RoutedEventArgs());
        }
    }

    /// <summary>
    /// Handles the Click event of the BtChooseBgImage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btChooseBgImage_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlgBgImage = new OpenFileDialog();
            
        if (dlgBgImage.ShowDialog() == true)
            TbBgImage.Text = dlgBgImage.FileName;
    }

    #endregion

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("background-color");
        _dict.Remove("background-image");
        _dict.Remove("background-repeat");
        _dict.Remove("background-attachment");
        _dict.Remove("background-position-x");
        _dict.Remove("background-position-y");

        if (CbBgColorTransparent.IsChecked == true)
        {
            _dict["background-color"] = "transparent";
        }
        else
        {
            _dict["background-color"] = CbBgColorTransparent.IsChecked == true ? "transparent" : WpfColorTranslator.ToHtml(((SolidColorBrush)TxtBgColor.Background).Color);
        }

        if (CbDoNotUseBackground.IsChecked == true)
            _dict["background-image"] = "none";
        else
        {
            _dict["background-image"] = string.IsNullOrEmpty(TbBgImage.Text.Trim()) ? string.Empty : $"url({TbBgImage.Text.Trim()})";
            _dict["background-repeat"] = (string)CbTiling.SelectedValue;
            _dict["background-attachment"] = (string)CbScrolling.SelectedValue;

            if (CbHorizontal.SelectedIndex != 4)
                _dict["background-position-x"] = (string)CbHorizontal.SelectedValue;
            else
                _dict["background-position-x"] = TbHorCust.Text + CbHorCustType.Text;

            if (CbVertical.SelectedIndex != 4)
                _dict["background-position-y"] = (string)CbVertical.SelectedValue;
            else
                _dict["background-position-y"] = TbVerCust.Text + CbVerCustType.Text;
        }
    }

    /// <summary>
    /// Handles the PreviewMouseDown event of the TxtBgColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void TxtBgColor_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (TxtBgColor.Background is SolidColorBrush solidColorBrush)
        {
            using IColorPickerDialog colorDialog = _createColorDialogMethod();
            colorDialog.StartingColor = solidColorBrush.Color;
            if (colorDialog.ShowDialog() == true)
                TxtBgColor.Background = new SolidColorBrush(colorDialog.SelectedColor);
        }
    }
}