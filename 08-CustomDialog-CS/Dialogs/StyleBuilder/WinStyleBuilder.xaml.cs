using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using System.Linq;
using SpiceLogic.HtmlEditor.WPF.ToolbarModule;

namespace CustomDialog.Dialogs.StyleBuilder;

/// <summary>
/// Interaction logic for WinStyleBuilder.xaml
/// </summary>
public partial class WinStyleBuilder : IStyleBuilderDialog
{
    private bool _tabChanged = false;
        
    /// <summary>
    /// User control currently displayed on the right
    /// </summary>
    private Control _selectedPage;

    /// <summary>
    /// Parsed style string
    /// </summary>
    private readonly Dictionary<string, string> _propertiesDict = new();

    /// <summary>
    /// A method creating the color dialog
    /// </summary>
    private readonly DialogFactoryDelegates.CreateColorPickerDialogDelegate _createColorDialogMethod;

    #region -------- Constructors ----------

    /// <summary>
    /// Initializes a new instance of the <see cref="WinStyleBuilder" /> class.
    /// </summary>
    /// <param name="createColorDialogMethod">A method creating the color dialog</param>
    public WinStyleBuilder(DialogFactoryDelegates.CreateColorPickerDialogDelegate createColorDialogMethod)
    {
        _createColorDialogMethod = createColorDialogMethod;

        InitializeComponent();
    }

    #endregion


    /// <summary>
    /// Parses the style argument.
    /// </summary>
    /// <param name="styleArg">The style arg.</param>
    /// <param name="parsingTolerance">The parsing tolerance.</param>
    /// <exception cref="System.ArgumentException">
    /// </exception>
    public void ParseStyleArgument(string styleArg, ParsingTolerance parsingTolerance)
    {
        try
        {
            int position = 0;

            if (string.IsNullOrEmpty(styleArg))
                styleArg = string.Empty;
            styleArg = styleArg.Trim();

            // Handle enclosed quotes
            if (styleArg.Length >= 2 && styleArg[0] == '\"' && styleArg[styleArg.Length - 1] == '\"')
                styleArg = styleArg.Substring(1, styleArg.Length - 2).Trim();

            while (position < styleArg.Length)
            {
                int separatorI = styleArg.IndexOf(':', position);
                if (separatorI != -1) // ':' found
                {
                    // localize tag
                    string tag = styleArg.Substring(position, separatorI - position).Trim().ToLowerInvariant();

                    // localize parameters
                    int nextSemicolonI = styleArg.IndexOf(';', separatorI + 1);
                    if (nextSemicolonI == -1)
                        nextSemicolonI = styleArg.Length;
                    string parameters = styleArg.Substring(separatorI + 1, nextSemicolonI - separatorI - 1).Trim();

                    // store in dictionary
                    _propertiesDict[tag] = parameters;

                    // move starting position
                    position = nextSemicolonI + 1;
                }
                else                 // ':' not found
                {
                    string reminder = styleArg.Substring(position).Trim();
                    if (reminder.Length != 0 && parsingTolerance != ParsingTolerance.TolerateUnknownTags)
                        throw new ArgumentException($"Tag {reminder} has no specification");

                    break;           // no exception was thrown - end parsing
                }
            }

            if (parsingTolerance == ParsingTolerance.NoTolerance)
            {
                // create list of handling classes
                List<FormSelectorPageAttribute> handlingClasses = [];
                Assembly aThisAssembly = Assembly.GetAssembly(this.GetType());
                if (aThisAssembly != null)
                    foreach (Type t in aThisAssembly.GetTypes())
                    {
                        Attribute attr = Attribute.GetCustomAttribute(t, typeof(FormSelectorPageAttribute));
                        if (attr != null)
                            handlingClasses.Add((FormSelectorPageAttribute)attr);
                    }

                // iterate through tags
                foreach (KeyValuePair<string, string> kvp in _propertiesDict)
                {
                    bool tagHandled = handlingClasses.Any(attr => attr.HandlesTag(kvp.Key));
                    // search for a class to handle this attribute

                    if (!tagHandled)
                        throw new ArgumentException($"Tag {kvp.Key} can't be handled.");
                }
            }
        }
        catch (Exception err)
        {
            MessageBox.Show(err.Message, @"Error parsing");
        }
    }

    public void Dispose()
    {
    }


    #region ------- Exposed Getters ----------------

    /// <summary>
    /// Gets a value indicating whether [remove style].
    /// </summary>
    /// <value><c>true</c> if [remove style]; otherwise, <c>false</c>.</value>
    public bool RemoveStyle
    {
        get => ChkRemoveStyle.IsChecked == true;
        set => ChkRemoveStyle.IsChecked = value;
    }

    /// <summary>
    /// Gets the CSS text value.
    /// </summary>
    /// <value>The CSS text value.</value>
    public string CSSTextValue
    {
        get
        {
            // iterate through properties and construct string representation
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _propertiesDict)
            {
                if (kvp.Value != null && kvp.Value.Trim().Length > 0)
                {
                    sb.Append(kvp.Key);
                    sb.Append(": ");
                    sb.Append(kvp.Value);
                    sb.Append("; ");
                }
            }
            return sb.ToString().Trim();
        }
    }

    #endregion


    /// <summary>
    /// Selects first page on Loaded event
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void winStyleBuilder_Loaded(object sender, RoutedEventArgs e)
    {
        TabControl.SelectedIndex = 0;
    }

    /// <summary>
    /// Updates the currently visible page
    /// Internally this method looks up current assembly using reflection for the appropriate page control to be shown
    /// The page control should be marked with FormSelectorPageAttribute and have a constructor which accepts Dictionary&lt;string, string&gt;
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //ignore SelectionChanged events of other selector controls
        if (!Equals(e.OriginalSource, TabControl))
        {
            return;
        }
            
        // Dispose currently selector control if any
        if (_selectedPage != null)
        {
            ((IEditorStylePage)_selectedPage).FlushContent();

            if (TabControl.Items[TabControl.SelectedIndex] is TabItem selectedTabItem)
                selectedTabItem.Content = null;

            _selectedPage = null;
        }

        // Check selection and look for the control
        if (TabControl.SelectedItem != null)
        {
            // Name of the page
            string sName = (string)((TabItem)TabControl.SelectedItem).Header;

            // Search this assembly for the appropriate control
            Assembly aThisAssembly = Assembly.GetAssembly(this.GetType());
            if (aThisAssembly != null)
                foreach (Type t in aThisAssembly.GetTypes())
                {
                    Attribute attr = Attribute.GetCustomAttribute(t, typeof(FormSelectorPageAttribute));
                    if (attr != null)
                    {
                        FormSelectorPageAttribute pageAttr = (FormSelectorPageAttribute)attr;
                        if (string.Compare(pageAttr.PageName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ConstructorInfo constructorInfo = t.GetConstructors().FirstOrDefault();
                            if (constructorInfo != null)
                            {
                                if (constructorInfo.GetParameters().Any(q =>
                                        q.ParameterType ==
                                        typeof(DialogFactoryDelegates.CreateColorPickerDialogDelegate)))
                                {
                                    //the constructors need DialogFactoryDelegates.CreateColorPickerDialogDelegate
                                    _selectedPage = (Control)Activator.CreateInstance(t, _propertiesDict,
                                        _createColorDialogMethod);
                                }
                                else
                                {
                                    _selectedPage = (Control)Activator.CreateInstance(t, _propertiesDict);
                                }
                            }

                            break;
                        }
                    }
                }

            // Dummy check
            if (_selectedPage == null)
                throw new NotImplementedException($"Page {sName} is not implemented in Style Page Selector.");

            // add it to the form
            if (TabControl.Items[TabControl.SelectedIndex] is TabItem selectedTabItem)
                selectedTabItem.Content = _selectedPage;

        }
    }

    /// <summary>
    /// Handles the Closed event of the frmFontSelector control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void winStyleBuilder_Closed(object sender, EventArgs eventArgs)
    {
        if (_selectedPage != null)
        {
            ((IEditorStylePage)_selectedPage).FlushContent();

            if (TabControl.Items[TabControl.SelectedIndex] is TabItem selectedTabItem)
                selectedTabItem.Content = null;

            _selectedPage = null;
        }
    }

    /// <summary>
    /// Handles the MouseLeftButtonDown event of the DialogHeader control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    /// <summary>
    /// Handles the OnRightButtonClicked event of the TwoButtonPanel control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void TwoButtonPanel_OnRightButtonClicked(object sender, RoutedEventArgs e)
    {
        //cancel button

        DialogResult = false;
    }

    /// <summary>
    /// Handles the OnLeftButtonClicked event of the TwoButtonPanel control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void TwoButtonPanel_OnLeftButtonClicked(object sender, RoutedEventArgs e)
    {
        //ok button

        DialogResult = true;
    }
}