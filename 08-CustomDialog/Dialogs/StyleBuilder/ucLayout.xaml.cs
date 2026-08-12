using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucLayout
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("Layout", "clip,visibility,display,float,clear,overflow,page-break-before,page-break-after")]
public partial class ucLayout : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    #region Preset of possible value

    /// <summary>
    /// The _ visibility
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _visibility = [];

    /// <summary>
    /// The _ display
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _display = [];

    /// <summary>
    /// The _ float
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _float = [];

    /// <summary>
    /// The _ clear
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _clear = [];

    /// <summary>
    /// The _ overflow
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _overflow = [];

    /// <summary>
    /// The _ page break before
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _pageBreakBefore = [];

    /// <summary>
    /// The _ page break after
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _pageBreakAfter;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ucLayout"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    public ucLayout(Dictionary<string, string> dict)
    {
        _dict = dict;

        #region Fill lists

        _visibility.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _visibility.Add(new KeyValuePair<string, string>("Hidden", "hidden"));
        _visibility.Add(new KeyValuePair<string, string>("Visible", "visible"));

        _display.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _display.Add(new KeyValuePair<string, string>("Do not display", "none"));
        _display.Add(new KeyValuePair<string, string>("As a block element", "block"));
        _display.Add(new KeyValuePair<string, string>("As an inflow element", "inline"));

        _float.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _float.Add(new KeyValuePair<string, string>("Don't allow text on sides", "none"));
        _float.Add(new KeyValuePair<string, string>("To the right", "right"));
        _float.Add(new KeyValuePair<string, string>("To the left", "left"));

        _clear.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _clear.Add(new KeyValuePair<string, string>("On either side", "none"));
        _clear.Add(new KeyValuePair<string, string>("Only on right", "right"));
        _clear.Add(new KeyValuePair<string, string>("Only on left", "left"));
        _clear.Add(new KeyValuePair<string, string>("Do not allow", "both"));

        _overflow.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _overflow.Add(new KeyValuePair<string, string>("Use scrollbars if needed", "auto"));
        _overflow.Add(new KeyValuePair<string, string>("Always use scrollbars", "scroll"));
        _overflow.Add(new KeyValuePair<string, string>("Content is not clipped", "visible"));
        _overflow.Add(new KeyValuePair<string, string>("Content is clipped", "hidden"));

        _pageBreakBefore.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _pageBreakBefore.Add(new KeyValuePair<string, string>("Auto", "auto"));
        _pageBreakBefore.Add(new KeyValuePair<string, string>("Force a page break", "always"));
        _pageBreakBefore.Add(new KeyValuePair<string, string>("No page break", "avoid"));
        _pageBreakBefore.Add(new KeyValuePair<string, string>("Until a blank left page", "left"));
        _pageBreakBefore.Add(new KeyValuePair<string, string>("Until a blank right page", "right"));

        _pageBreakAfter = new List<KeyValuePair<string, string>>(_pageBreakBefore);

        #endregion

        InitializeComponent();
    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("clip");

        _dict["visibility"] = (string)CbVisibility.SelectedValue;
        _dict["display"] = (string)CbDisplay.SelectedValue;
        _dict["float"] = (string)CbAllowFloatingObject.SelectedValue;
        _dict["clear"] = (string)CbAllowTextToFlow.SelectedValue;
        _dict["overflow"] = (string)CbOverflow.SelectedValue;
        _dict["page-break-before"] = (string)CbPbBefore.SelectedValue;
        _dict["page-break-after"] = (string)CbPbAfter.SelectedValue;

        {
            // clip
            var top = TbTop.Text.Trim();
            var right = TbRight.Text.Trim();
            var bottom = TbBottom.Text.Trim();
            var left = TbLeft.Text.Trim();

            if (top.Length + right.Length + bottom.Length + left.Length > 0)
            {
                // fix values if any
                top = top.Length == 0 ? "auto" : top + CbTopType.Text;

                right = right.Length == 0 ? "auto" : right + CbRightType.Text;

                bottom = bottom.Length == 0 ? "auto" : bottom + CbBottomType.Text;

                left = left.Length == 0 ? "auto" : left + CbLeftType.Text;

                // store to the dictionary
                _dict["clip"] = $"rect({top} {right} {bottom} {left})";
            }
        }
    }

    /// <summary>
    /// Handles the Loaded event of the ucLayout control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ucLayout_Loaded(object sender, EventArgs e)
    {
        #region set data sources

        CbVisibility.ItemsSource = _visibility;
        CbVisibility.DisplayMemberPath = "Key";
        CbVisibility.SelectedValuePath = "Value";
        CbVisibility.SelectedIndex = 0;

        CbDisplay.ItemsSource = _display;
        CbDisplay.DisplayMemberPath = "Key";
        CbDisplay.SelectedValuePath = "Value";
        CbDisplay.SelectedIndex = 0;

        CbAllowTextToFlow.ItemsSource = _float;
        CbAllowTextToFlow.DisplayMemberPath = "Key";
        CbAllowTextToFlow.SelectedValuePath = "Value";
        CbAllowTextToFlow.SelectedIndex = 0;

        CbAllowFloatingObject.ItemsSource = _clear;
        CbAllowFloatingObject.DisplayMemberPath = "Key";
        CbAllowFloatingObject.SelectedValuePath = "Value";
        CbAllowFloatingObject.SelectedIndex = 0;

        CbOverflow.ItemsSource = _overflow;
        CbOverflow.DisplayMemberPath = "Key";
        CbOverflow.SelectedValuePath = "Value";
        CbOverflow.SelectedIndex = 0;

        CbPbBefore.ItemsSource = _pageBreakBefore;
        CbPbBefore.DisplayMemberPath = "Key";
        CbPbBefore.SelectedValuePath = "Value";
        CbPbBefore.SelectedIndex = 0;

        CbPbAfter.ItemsSource = _pageBreakAfter;
        CbPbAfter.DisplayMemberPath = "Key";
        CbPbAfter.SelectedValuePath = "Value";
        CbPbAfter.SelectedIndex = 0;

        #endregion

        #region parse dictionary's values

        if (_dict.TryGetValue("overflow", out var value))
            for (int i = 0, n = _overflow.Count; i < n; ++i)
                if (string.Equals(value, _overflow[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbOverflow.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("visibility", out value))
            for (int i = 0, n = _visibility.Count; i < n; ++i)
                if (string.Equals(value, _visibility[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbVisibility.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("display", out value))
            for (int i = 0, n = _display.Count; i < n; ++i)
                if (string.Equals(value, _display[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbDisplay.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("float", out value))
            for (int i = 0, n = _float.Count; i < n; ++i)
                if (string.Equals(value, _float[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAllowTextToFlow.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("clear", out value))
            for (int i = 0, n = _clear.Count; i < n; ++i)
                if (string.Equals(value, _clear[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbAllowFloatingObject.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("page-break-before", out value))
            for (int i = 0, n = _pageBreakBefore.Count; i < n; ++i)
                if (string.Equals(value, _pageBreakBefore[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPbBefore.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("page-break-after", out value))
            for (int i = 0, n = _pageBreakAfter.Count; i < n; ++i)
                if (string.Equals(value, _pageBreakAfter[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbPbAfter.SelectedIndex = i;
                    break;
                }

        if (_dict.TryGetValue("clip", out value) &&
            value.StartsWith("rect(", StringComparison.InvariantCultureIgnoreCase) && value.EndsWith(")"))
        {
            var inner = value.Substring(5, value.Length - 6);
            var parts = inner.Split(' ');
            if (parts.Length >= 4)
            {
                var top = parts[0];
                var right = parts[1];
                var bottom = parts[2];
                var left = parts[3];

                for (int i = 0, n = CbTopType.Items.Count; i < n; ++i)
                    if ((CbTopType.Items[i] as ComboBoxItem)?.Content is string topType &&
                        top.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbTop.Text = top.Substring(0, top.Length - (topType.Length));
                        CbTopType.SelectedIndex = i;
                        break;
                    }

                for (int i = 0, n = CbRightType.Items.Count; i < n; ++i)
                    if ((CbRightType.Items[i] as ComboBoxItem)?.Content is string rightType &&
                        right.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbRight.Text = right.Substring(0, right.Length - (rightType.Length));
                        CbRightType.SelectedIndex = i;
                        break;
                    }

                for (int i = 0, n = CbBottomType.Items.Count; i < n; ++i)
                    if ((CbBottomType.Items[i] as ComboBoxItem)?.Content is string bottomType &&
                        bottom.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbBottom.Text = bottom.Substring(0, bottom.Length - (bottomType.Length));
                        CbBottomType.SelectedIndex = i;
                        break;
                    }

                for (int i = 0, n = CbLeftType.Items.Count; i < n; ++i)
                {
                    var leftType = (CbLeftType.Items[i] as ComboBoxItem).Content as string;
                    if (left.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        TbLeft.Text = left.Substring(0, left.Length - (leftType.Length));
                        CbLeftType.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        #endregion
    }
}