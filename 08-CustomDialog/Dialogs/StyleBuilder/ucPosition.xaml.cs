using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucPosition
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Position", "position,top,left,width,height,z-index")]
public partial class ucPosition : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    #region Preset of possible values

    /// <summary>
    /// The _ position
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _position = [];
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ucPosition"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    public ucPosition(Dictionary<string, string> dict)
    {
        _dict = dict;

        #region Initialize presets
        _position.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _position.Add(new KeyValuePair<string, string>("Position in normal flow", "static"));
        _position.Add(new KeyValuePair<string, string>("Offset from normal flow", "relative"));
        _position.Add(new KeyValuePair<string, string>("Absolutely position", "absolute"));
        #endregion

        InitializeComponent();
    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("position");
        _dict.Remove("top");
        _dict.Remove("left");
        _dict.Remove("width");
        _dict.Remove("height");
        _dict.Remove("z-index");

        if (CbPositionMode.SelectedIndex >= 2)
        {
            if (TbLeft.Text.Trim().Length > 0)
                _dict["left"] = TbLeft.Text + CbLeftType.Text;
            if (TbTop.Text.Trim().Length > 0)
                _dict["top"] = TbTop.Text + CbTopType.Text;

            if (CbPositionMode.SelectedIndex == 3)
                _dict["z-index"] = TbZIndex.Text;
        }

        if (TbHeight.Text.Trim().Length > 0)
            _dict["height"] = TbHeight.Text + CbHeightType.Text;
        if (TbWidth.Text.Trim().Length > 0)
            _dict["width"] = TbWidth.Text + CbWidthType.Text;

        _dict["position"] = (string)CbPositionMode.SelectedValue;
    }

    /// <summary>
    /// Handles the Loaded event of the ucPosition control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucPosition_Loaded(object sender, RoutedEventArgs e)
    {
        #region set data sources
        CbPositionMode.ItemsSource = _position;
        CbPositionMode.DisplayMemberPath = "Key";
        CbPositionMode.SelectedValuePath = "Value";
        CbPositionMode.SelectedIndex = 0;
        #endregion

        #region parse
        if (_dict.TryGetValue("left", out var value))
        {
            for (int i = 0, n = CbLeftType.Items.Count; i < n; ++i)
            {
                if ((CbLeftType.Items[i] as ComboBoxItem)?.Content is string leftType && value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase))
                {
                    TbLeft.Text = value.Substring(0, value.Length - (leftType.Length));
                    CbLeftType.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("top", out value))
        {
            for (int i = 0, n = CbTopType.Items.Count; i < n; ++i)
            {
                if ((CbTopType.Items[i] as ComboBoxItem)?.Content is string topType && value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase))
                {
                    TbTop.Text = value.Substring(0, value.Length - (topType.Length));
                    CbTopType.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("width", out value))
        {
            for (int i = 0, n = CbWidthType.Items.Count; i < n; ++i)
            {
                if ((CbWidthType.Items[i] as ComboBoxItem)?.Content is string widthType && value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase))
                {
                    TbWidth.Text = value.Substring(0, value.Length - (widthType.Length));
                    CbWidthType.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("height", out value))
        {
            for (int i = 0, n = CbHeightType.Items.Count; i < n; ++i)
            {
                if ((CbHeightType.Items[i] as ComboBoxItem)?.Content is string heightType && value.EndsWith(heightType, StringComparison.InvariantCultureIgnoreCase))
                {
                    TbHeight.Text = value.Substring(0, value.Length - (heightType.Length));
                    CbHeightType.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_dict.TryGetValue("z-index", out value))
            TbZIndex.Text = value;

        if (_dict.TryGetValue("position", out value))
        {
            for (int i = 0, n = _position.Count; i < n; ++i)
                if (string.Equals(value, _position[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPositionMode.SelectedIndex = i;
                    break;
                }
        }
        #endregion
    }

    #region UI handlers
    /// <summary>
    /// Handles the SelectionChanged event of the CbPositionMode control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbPositionMode_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbLeft.IsEnabled = CbLeftType.IsEnabled = TbTop.IsEnabled = CbTopType.IsEnabled = CbPositionMode.SelectedIndex >= 2;

        TbZIndex.IsEnabled = CbPositionMode.SelectedIndex == 3;
    }
    #endregion
}