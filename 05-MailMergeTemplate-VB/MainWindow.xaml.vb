Imports System.Collections.Generic
Imports System.Windows
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.MailMerge

Namespace Global.MailMergeTemplate

    Partial Public Class MainWindow

        ' A sample record used to substitute merge tokens for the preview. In a real
        ' application this would come from your database, CRM, or invoicing system.
        Private Shared ReadOnly SampleRecord As New Dictionary(Of String, String) From {
            {"{{FirstName}}", "Jordan"},
            {"{{LastName}}", "Avery"},
            {"{{Company}}", "Example Corp"},
            {"{{InvoiceNumber}}", "INV-1042"},
            {"{{DueDate}}", "September 1, 2026"}
        }

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            Dim fields As New List(Of PlaceholderField) From {
                New PlaceholderField("First name", "{{FirstName}}"),
                New PlaceholderField("Last name", "{{LastName}}"),
                New PlaceholderField("Company", "{{Company}}"),
                New PlaceholderField("Invoice number", "{{InvoiceNumber}}"),
                New PlaceholderField("Due date", "{{DueDate}}")
            }

            Editor.Content.MailMerge.PlaceholderFields = fields
            Editor.ShowPlaceholderToolbar = True

            Editor.BodyHtml =
                "<p>Dear {{FirstName}} {{LastName}},</p>" &
                "<p>This is a reminder that invoice {{InvoiceNumber}} for {{Company}} is due on {{DueDate}}.</p>"
        End Sub

        Private Sub PreviewButton_Click(sender As Object, e As RoutedEventArgs)
            ' The editor hosts a live document, so pull the latest edits into BodyHtml before
            ' reading it - otherwise the most recent typing is not there yet.
            Editor.UpdateBindings()

            Dim merged As String = Editor.BodyHtml
            For Each pair As KeyValuePair(Of String, String) In SampleRecord
                merged = merged.Replace(pair.Key, pair.Value)
            Next

            PreviewEditor.BodyHtml = merged
        End Sub

    End Class

End Namespace
