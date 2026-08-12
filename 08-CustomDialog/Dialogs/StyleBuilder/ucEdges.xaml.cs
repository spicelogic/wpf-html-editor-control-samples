using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;
using SpiceLogic.HtmlEditor.WPF.Extensions;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.ToolbarModule;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucEdges
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Edges",
    "margin-top,margin-bottom,margin-left,margin-right,padding-top,padding-bottom,padding-left,padding-right,border-top-style,border-bottom-style,border-left-style,border-right-style")]
public partial class ucEdges : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    #region Preset of possible value

    /// <summary>
    /// The _ border top style
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _borderTopStyle = [];

    /// <summary>
    /// The _ border bottom style
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _borderBottomStyle;

    /// <summary>
    /// The _ border left style
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _borderLeftStyle;

    /// <summary>
    /// The _ border right style
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _borderRightStyle;

    #endregion

    /// <summary>
    /// A method creating the color dialog
    /// </summary>
    private readonly DialogFactoryDelegates.CreateColorPickerDialogDelegate _createColorDialogMethod;


    /// <summary>
    /// Initializes a new instance of the <see cref="ucEdges"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    /// <param name="createColorDialogMethod">A method creating the color dialog</param>
    public ucEdges(Dictionary<string, string> dict,
        DialogFactoryDelegates.CreateColorPickerDialogDelegate
            createColorDialogMethod) //the argument order is important (Activator.CreateInstance uses it)
    {
        _dict = dict;
        _createColorDialogMethod = createColorDialogMethod;

        #region Initialize presets

        _borderTopStyle.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _borderTopStyle.Add(new KeyValuePair<string, string>("None", "none"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Dotted", "dotted"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Dashed", "dashed"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Solid line", "solid"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Double line", "double"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Groove", "groove"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Ridge", "ridge"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Inset", "inset"));
        _borderTopStyle.Add(new KeyValuePair<string, string>("Outset", "outset"));

        _borderBottomStyle = new List<KeyValuePair<string, string>>(_borderTopStyle);
        _borderLeftStyle = new List<KeyValuePair<string, string>>(_borderTopStyle);
        _borderRightStyle = new List<KeyValuePair<string, string>>(_borderTopStyle);

        #endregion

        InitializeComponent();
    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("margin-top");
        _dict.Remove("margin-bottom");
        _dict.Remove("margin-left");
        _dict.Remove("margin-right");
        _dict.Remove("padding-top");
        _dict.Remove("padding-bottom");
        _dict.Remove("padding-left");
        _dict.Remove("padding-right");

        _dict.Remove("border-top-style");
        _dict.Remove("border-bottom-style");
        _dict.Remove("border-left-style");
        _dict.Remove("border-right-style");

        if (TbMTop.Text.Trim().Length > 0)
            _dict["margin-top"] = string.Concat(TbMTop.Text, CbMTopType.Text);
        if (TbMBottom.Text.Trim().Length > 0)
            _dict["margin-bottom"] = string.Concat(TbMBottom.Text, CbMBottomType.Text);
        if (TbMLeft.Text.Trim().Length > 0)
            _dict["margin-left"] = string.Concat(TbMLeft.Text, CbMLeftType.Text);
        if (TbMRight.Text.Trim().Length > 0)
            _dict["margin-right"] = string.Concat(TbMRight.Text, CbMRightType.Text);

        if (TbPTop.Text.Trim().Length > 0)
            _dict["padding-top"] = string.Concat(TbPTop.Text, CbPTopType.Text);
        if (TbPBottom.Text.Trim().Length > 0)
            _dict["padding-bottom"] = string.Concat(TbPBottom.Text, CbPBottomType.Text);
        if (TbPLeft.Text.Trim().Length > 0)
            _dict["padding-left"] = string.Concat(TbPLeft.Text, CbPLeftType.Text);
        if (TbPRight.Text.Trim().Length > 0)
            _dict["padding-right"] = string.Concat(TbPRight.Text, CbPRightType.Text);

        {
            // left border
            var sb = new StringBuilder();
            if (CbLeftStyle.SelectedIndex > 0)
            {
                sb.Append((string)CbLeftStyle.SelectedValue);
                sb.Append(' ');

                if (CbLeftWidth.SelectedIndex > 0)
                {
                    if (CbLeftWidth.SelectedIndex == 4)
                    {
                        if (TbLeftWidth.Text.Trim().Length > 0)
                        {
                            sb.Append(string.Concat(TbLeftWidth.Text.Trim(), CbLeftWidthType.Text));
                            sb.Append(' ');
                        }
                    }
                    else
                    {
                        sb.Append(CbLeftWidth.Text.ToLowerInvariant());
                        sb.Append(' ');
                    }
                }

                sb.Append(CbLeftColor.Text.ToLowerInvariant());
            }

            _dict["border-left-style"] = sb.ToString();
        }

        {
            // right border
            var sb = new StringBuilder();
            if (CbRightStyle.SelectedIndex > 0)
            {
                sb.Append((string)CbRightStyle.SelectedValue);
                sb.Append(' ');

                if (CbRightWidth.SelectedIndex > 0)
                {
                    if (CbRightWidth.SelectedIndex == 4)
                    {
                        if (TbRightWidth.Text.Trim().Length > 0)
                        {
                            sb.Append(string.Concat(TbRightWidth.Text.Trim(), CbRightWidthType.Text));
                            sb.Append(' ');
                        }
                    }
                    else
                    {
                        sb.Append(CbRightWidth.Text.ToLowerInvariant());
                        sb.Append(' ');
                    }
                }

                sb.Append(CbRightColor.Text.ToLowerInvariant());
            }

            _dict["border-right-style"] = sb.ToString();
        }

        {
            // top border
            var sb = new StringBuilder();
            if (CbTopStyle.SelectedIndex > 0)
            {
                sb.Append((string)CbTopStyle.SelectedValue);
                sb.Append(' ');

                if (CbTopWidth.SelectedIndex > 0)
                {
                    if (CbTopWidth.SelectedIndex == 4)
                    {
                        if (TbTopWidth.Text.Trim().Length > 0)
                        {
                            sb.Append(string.Concat(TbTopWidth.Text.Trim(), CbTopWidthType.Text));
                            sb.Append(' ');
                        }
                    }
                    else
                    {
                        sb.Append(CbTopWidth.Text.ToLowerInvariant());
                        sb.Append(' ');
                    }
                }

                sb.Append(CbTopColor.Text.ToLowerInvariant());
            }

            _dict["border-top-style"] = sb.ToString();
        }

        {
            // bottom border
            var sb = new StringBuilder();
            if (CbBottomStyle.SelectedIndex > 0)
            {
                sb.Append((string)CbBottomStyle.SelectedValue);
                sb.Append(' ');

                if (CbBottomWidth.SelectedIndex > 0)
                {
                    if (CbBottomWidth.SelectedIndex == 4)
                    {
                        if (TbBottomWidth.Text.Trim().Length > 0)
                        {
                            sb.Append(string.Concat(TbBottomWidth.Text.Trim(), CbBottomWidthType.Text));
                            sb.Append(' ');
                        }
                    }
                    else
                    {
                        sb.Append(CbBottomWidth.Text.ToLowerInvariant());
                        sb.Append(' ');
                    }
                }

                sb.Append(CbBottomColor.Text.ToLowerInvariant());
            }

            _dict["border-bottom-style"] = sb.ToString();
        }
    }

    /// <summary>
    /// Handles the Loaded event of the ucEdges control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucEdges_Loaded(object sender, RoutedEventArgs e)
    {
        #region set data sources

        CbLeftStyle.ItemsSource = _borderLeftStyle;
        CbLeftStyle.DisplayMemberPath = "Key";
        CbLeftStyle.SelectedValuePath = "Value";
        CbLeftStyle.SelectedIndex = 0;

        CbRightStyle.ItemsSource = _borderRightStyle;
        CbRightStyle.DisplayMemberPath = "Key";
        CbRightStyle.SelectedValuePath = "Value";
        CbRightStyle.SelectedIndex = 0;

        CbTopStyle.ItemsSource = _borderTopStyle;
        CbTopStyle.DisplayMemberPath = "Key";
        CbTopStyle.SelectedValuePath = "Value";
        CbTopStyle.SelectedIndex = 0;

        CbBottomStyle.ItemsSource = _borderBottomStyle;
        CbBottomStyle.DisplayMemberPath = "Key";
        CbBottomStyle.SelectedValuePath = "Value";
        CbBottomStyle.SelectedIndex = 0;

        CbLeftWidth.SelectedIndex = 0;
        CbRightWidth.SelectedIndex = 0;
        CbTopWidth.SelectedIndex = 0;
        CbBottomWidth.SelectedIndex = 0;

        #endregion

        #region parse margins

        if (_dict.TryGetValue("margin-top", out var value))
            for (int i = 0, n = CbMTopType.Items.Count; i < n; ++i)
            {
                var topType = (CbMTopType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbMTopType.SelectedIndex = i;
                    TbMTop.Text = value.Substring(0, value.Length - (topType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("margin-bottom", out value))
            for (int i = 0, n = CbMBottomType.Items.Count; i < n; ++i)
            {
                var bottomType = (CbMBottomType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbMBottomType.SelectedIndex = i;
                    TbMBottom.Text = value.Substring(0, value.Length - (bottomType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("margin-left", out value))
            for (int i = 0, n = CbMLeftType.Items.Count; i < n; ++i)
            {
                var leftType = (CbMLeftType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbMLeftType.SelectedIndex = i;
                    TbMLeft.Text = value.Substring(0, value.Length - (leftType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("margin-right", out value))
            for (int i = 0, n = CbMRightType.Items.Count; i < n; ++i)
            {
                var rightType = (CbMRightType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbMRightType.SelectedIndex = i;
                    TbMRight.Text = value.Substring(0, value.Length - (rightType.Length));
                    break;
                }
            }

        #endregion

        #region parse padding

        if (_dict.TryGetValue("padding-top", out value))
            for (int i = 0, n = CbPTopType.Items.Count; i < n; ++i)
            {
                var topType = (CbPTopType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPTopType.SelectedIndex = i;
                    TbPTop.Text = value.Substring(0, value.Length - (topType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("padding-bottom", out value))
            for (int i = 0, n = CbPBottomType.Items.Count; i < n; ++i)
            {
                var bottomType = (CbPBottomType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPBottomType.SelectedIndex = i;
                    TbPBottom.Text = value.Substring(0, value.Length - (bottomType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("padding-left", out value))
            for (int i = 0, n = CbPLeftType.Items.Count; i < n; ++i)
            {
                var leftType = (CbPLeftType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPLeftType.SelectedIndex = i;
                    TbPLeft.Text = value.Substring(0, value.Length - (leftType.Length));
                    break;
                }
            }

        if (_dict.TryGetValue("padding-right", out value))
            for (int i = 0, n = CbPRightType.Items.Count; i < n; ++i)
            {
                var rightType = (CbPRightType.Items[i] as ComboBoxItem).Content as string;
                if (value.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPRightType.SelectedIndex = i;
                    TbPRight.Text = value.Substring(0, value.Length - (rightType.Length));
                    break;
                }
            }

        #endregion

        #region parse left border

        if (_dict.TryGetValue("border-left-style", out value))
        {
            var values = new List<string>(value.Split(' '));
            // Filter empty
            for (var i = 0; i < values.Count; ++i)
                if (values[i].Trim().Length == 0)
                {
                    values.RemoveAt(i);
                    --i;
                }

            var styleFound = false;

            for (var valI = 0; valI < values.Count && !styleFound; ++valI)
            {
                value = values[valI];
                for (int i = 0, n = _borderLeftStyle.Count; i < n && !styleFound; ++i)
                    if (string.Equals(value, _borderLeftStyle[i].Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbLeftStyle.SelectedIndex = i;
                        values.RemoveAt(valI);
                        styleFound = true;
                    }
            }

            if (styleFound && CbLeftStyle.SelectedIndex >= 2)
            {
                var widthFound = false;

                for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                {
                    value = values[valI];
                    for (var i = 1; i < 4 && !widthFound; ++i)
                    {
                        var leftWidth = (CbLeftWidth.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, leftWidth, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbLeftWidth.SelectedIndex = i;
                            values.RemoveAt(valI);
                            widthFound = true;
                        }
                    }
                }

                if (!widthFound)
                    for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                    {
                        value = values[valI];
                        for (int i = 0, n = CbLeftWidthType.Items.Count; i < n && !widthFound; ++i)
                        {
                            var cbLeftWidthType = (CbLeftWidthType.Items[i] as ComboBoxItem).Content as string;
                            if (value.EndsWith(cbLeftWidthType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                CbLeftWidth.SelectedIndex = 4;
                                CbLeftWidthType.SelectedIndex = i;
                                TbLeftWidth.Text = value.Substring(0, value.Length - (cbLeftWidthType.Length));
                                values.RemoveAt(valI);
                                widthFound = true;
                            }
                        }
                    }

                var colorFound = false;

                for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                {
                    value = values[valI];
                    for (int i = 0, n = CbLeftColor.Items.Count; i < n && !colorFound; ++i)
                    {
                        var leftColor = (CbLeftColor.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, leftColor, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbLeftColor.SelectedIndex = i;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
                }

                if (!colorFound)
                    for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                    {
                        value = values[valI];
                        if (value.StartsWith("#"))
                        {
                            CbLeftColor.Text = value;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
            }
        }

        #endregion

        #region parse Right border

        if (_dict.TryGetValue("border-right-style", out value))
        {
            var values = new List<string>(value.Split(' '));
            // Filter empty
            for (var i = 0; i < values.Count; ++i)
                if (values[i].Trim().Length == 0)
                {
                    values.RemoveAt(i);
                    --i;
                }

            var styleFound = false;

            for (var valI = 0; valI < values.Count && !styleFound; ++valI)
            {
                value = values[valI];
                for (int i = 0, n = _borderRightStyle.Count; i < n && !styleFound; ++i)
                    if (string.Equals(value, _borderRightStyle[i].Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbRightStyle.SelectedIndex = i;
                        values.RemoveAt(valI);
                        styleFound = true;
                    }
            }

            if (styleFound && CbRightStyle.SelectedIndex >= 2)
            {
                var widthFound = false;

                for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                {
                    value = values[valI];
                    for (var i = 1; i < 4 && !widthFound; ++i)
                    {
                        var rightWidth = (CbRightWidth.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, rightWidth, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbRightWidth.SelectedIndex = i;
                            values.RemoveAt(valI);
                            widthFound = true;
                        }
                    }
                }

                if (!widthFound)
                    for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                    {
                        value = values[valI];
                        for (int i = 0, n = CbRightWidthType.Items.Count; i < n && !widthFound; ++i)
                        {
                            var widthType = (CbRightWidthType.Items[i] as ComboBoxItem).Content as string;
                            if (value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                CbRightWidth.SelectedIndex = 4;
                                CbRightWidthType.SelectedIndex = i;
                                TbRightWidth.Text = value.Substring(0, value.Length - (widthType.Length));
                                values.RemoveAt(valI);
                                widthFound = true;
                            }
                        }
                    }

                var colorFound = false;

                for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                {
                    value = values[valI];
                    for (int i = 0, n = CbRightColor.Items.Count; i < n && !colorFound; ++i)
                    {
                        var rightColor = (CbRightColor.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, rightColor, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbRightColor.SelectedIndex = i;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
                }

                if (!colorFound)
                    for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                    {
                        value = values[valI];
                        if (value.StartsWith("#"))
                        {
                            CbRightColor.Text = value;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
            }
        }

        #endregion

        #region parse Top border

        if (_dict.TryGetValue("border-top-style", out value))
        {
            var values = new List<string>(value.Split(' '));
            // Filter empty
            for (var i = 0; i < values.Count; ++i)
                if (values[i].Trim().Length == 0)
                {
                    values.RemoveAt(i);
                    --i;
                }

            var styleFound = false;

            for (var valI = 0; valI < values.Count && !styleFound; ++valI)
            {
                value = values[valI];
                for (int i = 0, n = _borderTopStyle.Count; i < n && !styleFound; ++i)
                    if (string.Equals(value, _borderTopStyle[i].Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbTopStyle.SelectedIndex = i;
                        values.RemoveAt(valI);
                        styleFound = true;
                    }
            }

            if (styleFound && CbTopStyle.SelectedIndex >= 2)
            {
                var widthFound = false;

                for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                {
                    value = values[valI];
                    for (var i = 1; i < 4 && !widthFound; ++i)
                    {
                        var topWidth = (CbTopWidth.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, topWidth, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbTopWidth.SelectedIndex = i;
                            values.RemoveAt(valI);
                            widthFound = true;
                        }
                    }
                }

                if (!widthFound)
                    for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                    {
                        value = values[valI];
                        for (int i = 0, n = CbTopWidthType.Items.Count; i < n && !widthFound; ++i)
                        {
                            var widthType = (CbTopWidthType.Items[i] as ComboBoxItem).Content as string;
                            if (value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                CbTopWidth.SelectedIndex = 4;
                                CbTopWidthType.SelectedIndex = i;
                                TbTopWidth.Text = value.Substring(0, value.Length - (widthType.Length));
                                values.RemoveAt(valI);
                                widthFound = true;
                            }
                        }
                    }

                var colorFound = false;

                for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                {
                    value = values[valI];
                    for (int i = 0, n = CbTopColor.Items.Count; i < n && !colorFound; ++i)
                    {
                        var topColor = (CbTopColor.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, topColor, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbTopColor.SelectedIndex = i;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
                }

                if (!colorFound)
                    for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                    {
                        value = values[valI];
                        if (value.StartsWith("#"))
                        {
                            CbTopColor.Text = value;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
            }
        }

        #endregion

        #region parse Bottom border

        if (_dict.TryGetValue("border-bottom-style", out value))
        {
            var values = new List<string>(value.Split(' '));
            // Filter empty
            for (var i = 0; i < values.Count; ++i)
                if (values[i].Trim().Length == 0)
                {
                    values.RemoveAt(i);
                    --i;
                }

            var styleFound = false;

            for (var valI = 0; valI < values.Count && !styleFound; ++valI)
            {
                value = values[valI];
                for (int i = 0, n = _borderBottomStyle.Count; i < n && !styleFound; ++i)
                    if (string.Equals(value, _borderBottomStyle[i].Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CbBottomStyle.SelectedIndex = i;
                        values.RemoveAt(valI);
                        styleFound = true;
                    }
            }

            if (styleFound && CbBottomStyle.SelectedIndex >= 2)
            {
                var widthFound = false;

                for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                {
                    value = values[valI];
                    for (var i = 1; i < 4 && !widthFound; ++i)
                    {
                        var bottomWidth = (CbBottomWidth.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, bottomWidth, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbBottomWidth.SelectedIndex = i;
                            values.RemoveAt(valI);
                            widthFound = true;
                        }
                    }
                }

                if (!widthFound)
                    for (var valI = 0; valI < values.Count && !widthFound; ++valI)
                    {
                        value = values[valI];
                        for (int i = 0, n = CbBottomWidthType.Items.Count; i < n && !widthFound; ++i)
                        {
                            var widthType = (CbBottomWidthType.Items[i] as ComboBoxItem).Content as string;
                            if (value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                CbBottomWidth.SelectedIndex = 4;
                                CbBottomWidthType.SelectedIndex = i;
                                TbBottomWidth.Text = value.Substring(0, value.Length - (widthType.Length));
                                values.RemoveAt(valI);
                                widthFound = true;
                            }
                        }
                    }

                var colorFound = false;

                for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                {
                    value = values[valI];
                    for (int i = 0, n = CbBottomColor.Items.Count; i < n && !colorFound; ++i)
                    {
                        var bottomColor = (CbBottomColor.Items[i] as ComboBoxItem).Content as string;
                        if (string.Equals(value, bottomColor, StringComparison.InvariantCultureIgnoreCase))
                        {
                            CbBottomColor.SelectedIndex = i;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
                }

                if (!colorFound)
                    for (var valI = 0; valI < values.Count && !colorFound; ++valI)
                    {
                        value = values[valI];
                        if (value.StartsWith("#"))
                        {
                            CbBottomColor.Text = value;
                            values.RemoveAt(valI);
                            colorFound = true;
                        }
                    }
            }
        }

        #endregion
    }

    #region left border edge handlers

    /// <summary>
    /// Handles the SelectionChanged event of the CbLeftWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbLeftWidth_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbLeftWidth.IsEnabled = CbLeftWidthType.IsEnabled = CbLeftWidth.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the Click event of the BtLeftColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btLeftColor_Click(object sender, RoutedEventArgs e)
    {
        using var colorDialog = _createColorDialogMethod();
        if (colorDialog.ShowDialog() == true)
            CbLeftColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbLeftStyle control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbLeftStyle_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (CbLeftStyle.SelectedIndex >= 2)
        {
            CbLeftWidth.IsEnabled = TbLeftWidth.IsEnabled =
                CbLeftWidthType.IsEnabled = CbLeftColor.IsEnabled = BtLeftColor.IsEnabled = true;
            cbLeftWidth_SelectionChanged(this, new RoutedEventArgs());
        }
        else
        {
            CbLeftWidth.IsEnabled = TbLeftWidth.IsEnabled =
                CbLeftWidthType.IsEnabled = CbLeftColor.IsEnabled = BtLeftColor.IsEnabled = false;
        }
    }

    #endregion

    #region Right border edge handlers

    /// <summary>
    /// Handles the SelectionChanged event of the CbRightWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbRightWidth_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbRightWidth.IsEnabled = CbRightWidthType.IsEnabled = CbRightWidth.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the Click event of the BtRightColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btRightColor_Click(object sender, RoutedEventArgs e)
    {
        using var colorDialog = _createColorDialogMethod();
        if (colorDialog.ShowDialog() == true)
            CbRightColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbRightStyle control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbRightStyle_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (CbRightStyle.SelectedIndex >= 2)
        {
            CbRightWidth.IsEnabled = TbRightWidth.IsEnabled =
                CbRightWidthType.IsEnabled = CbRightColor.IsEnabled = BtRightColor.IsEnabled = true;
            cbRightWidth_SelectionChanged(this, new RoutedEventArgs());
        }
        else
        {
            CbRightWidth.IsEnabled = TbRightWidth.IsEnabled =
                CbRightWidthType.IsEnabled = CbRightColor.IsEnabled = BtRightColor.IsEnabled = false;
        }
    }

    #endregion

    #region Top border edge handlers

    /// <summary>
    /// Handles the SelectionChanged event of the CbTopWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbTopWidth_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbTopWidth.IsEnabled = CbTopWidthType.IsEnabled = CbTopWidth.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the Click event of the BtTopColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btTopColor_Click(object sender, RoutedEventArgs e)
    {
        using var colorDialog = _createColorDialogMethod();
        if (colorDialog.ShowDialog() == true)
            CbTopColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbTopStyle control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbTopStyle_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (CbTopStyle.SelectedIndex >= 2)
        {
            CbTopWidth.IsEnabled = TbTopWidth.IsEnabled =
                CbTopWidthType.IsEnabled = CbTopColor.IsEnabled = BtTopColor.IsEnabled = true;
            cbTopWidth_SelectionChanged(this, new RoutedEventArgs());
        }
        else
        {
            CbTopWidth.IsEnabled = TbTopWidth.IsEnabled =
                CbTopWidthType.IsEnabled = CbTopColor.IsEnabled = BtTopColor.IsEnabled = false;
        }
    }

    #endregion

    #region Bottom border edge handlers

    /// <summary>
    /// Handles the SelectionChanged event of the CbBottomWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbBottomWidth_SelectionChanged(object sender, RoutedEventArgs e)
    {
        TbBottomWidth.IsEnabled = CbBottomWidthType.IsEnabled = CbBottomWidth.SelectedIndex == 4;
    }

    /// <summary>
    /// Handles the Click event of the BtBottomColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btBottomColor_Click(object sender, RoutedEventArgs e)
    {
        using var colorDialog = _createColorDialogMethod();
        if (colorDialog.ShowDialog() == true)
            CbBottomColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the CbBottomStyle control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void cbBottomStyle_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (CbBottomStyle.SelectedIndex >= 2)
        {
            CbBottomWidth.IsEnabled = TbBottomWidth.IsEnabled =
                CbBottomWidthType.IsEnabled = CbBottomColor.IsEnabled = BtBottomColor.IsEnabled = true;
            cbBottomWidth_SelectionChanged(this, new RoutedEventArgs());
        }
        else
        {
            CbBottomWidth.IsEnabled = TbBottomWidth.IsEnabled =
                CbBottomWidthType.IsEnabled = CbBottomColor.IsEnabled = BtBottomColor.IsEnabled = false;
        }
    }

    #endregion
}