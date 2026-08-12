using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Class ucLists
/// </summary>
[ToolboxItem(false)]
[FormSelectorPage("List Position", "list-style-position")]
public partial class ucListPosition : IEditorStylePage
{
    /// <summary>
    /// The _dict
    /// </summary>
    private readonly Dictionary<string, string> _dict;

    #region Preset of possible values

    /// <summary>
    /// The _ list style position
    /// </summary>
    readonly List<KeyValuePair<string, string>> _listStylePosition = [];
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ucListPosition"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    public ucListPosition(Dictionary<string, string> dict)
    {
        _dict = dict;

        #region Initialize presets
        _listStylePosition.Add(new KeyValuePair<string, string>("<Not Set>", ""));
        _listStylePosition.Add(new KeyValuePair<string, string>("Outside (text is indented in)", "outside"));
        _listStylePosition.Add(new KeyValuePair<string, string>("Inside (text is not indented)", "inside"));
        #endregion

        InitializeComponent();
    }

    /// <summary>
    /// Flushes the content of the user control back to the dictionary
    /// </summary>
    public void FlushContent()
    {
        _dict.Remove("list-style-position");
        _dict["list-style-position"] = (string)CbBulletPosition.SelectedValue;
    }

    /// <summary>
    /// Handles the Loaded event of the ucLists control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ucLists_Loaded(object sender, RoutedEventArgs e)
    {
        #region set data sources
        CbBulletPosition.ItemsSource = _listStylePosition;
        CbBulletPosition.DisplayMemberPath = "Key";
        CbBulletPosition.SelectedValuePath = "Value";
        CbBulletPosition.SelectedIndex = 0;
        #endregion

        #region parse
        if (_dict.TryGetValue("list-style-position", out var value))
        {
            for (int i = 0, n = _listStylePosition.Count; i < n; ++i)
                if (string.Equals(value, _listStylePosition[i].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    CbBulletPosition.SelectedIndex = i;
                    break;
                }
        }

        #endregion
    }
}