Imports System
Imports System.Text.RegularExpressions

Namespace Global.Quickstart

    Partial Public Class MainWindow

        Private Const DocumentStyleElementId As String = "quickstart_document_style"

        ''' <summary>
        ''' The document stylesheet. The table carries a fixed width rather than 100% because a
        ''' percentage width is resolved against the viewport, which leaves the document a few
        ''' pixels wider than the visible area once the vertical scrollbar appears.
        ''' </summary>
        Private Const DocumentStyle As String =
            "body { font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif; font-size: 17px; line-height: 1.7; color: #1f2933; margin: 28px 34px; }" &
            "h1 { font-size: 34px; font-weight: 600; color: #0f2540; margin: 0 0 4px 0; }" &
            "h2 { font-size: 22px; font-weight: 600; color: #0f2540; margin: 30px 0 12px 0; padding-bottom: 7px; border-bottom: 1px solid #e2e8f0; }" &
            "p { margin: 0 0 14px 0; }" &
            "a { color: #1a6fd4; text-decoration: none; border-bottom: 1px solid #bcd6f5; }" &
            "ul { margin: 0 0 16px 0; padding-left: 22px; }" &
            "li { margin-bottom: 8px; }" &
            ".eyebrow { font-size: 13px; font-weight: 600; color: #1a6fd4; letter-spacing: 1.3px; margin: 0 0 6px 0; }" &
            ".lede { font-size: 19px; color: #52606d; margin: 0 0 24px 0; }" &
            ".callout { background-color: #f2f7fd; border-left: 4px solid #1a6fd4; padding: 15px 20px; margin: 0 0 24px 0; }" &
            ".callout p { margin: 0; }" &
            "table { border-collapse: collapse; width: 890px; margin: 4px 0 20px 0; font-size: 16px; }" &
            "th { background-color: #0f2540; color: #ffffff; text-align: left; font-weight: 600; padding: 11px 15px; }" &
            "td { border-bottom: 1px solid #e2e8f0; padding: 11px 15px; }" &
            "tr.alt td { background-color: #f7f9fc; }" &
            ".badge { display: inline-block; padding: 3px 11px; font-size: 13px; font-weight: 600; border-radius: 11px; }" &
            ".badge-live { background-color: #dff3e4; color: #1b6b3a; }" &
            ".badge-review { background-color: #fdf0d5; color: #8a5a00; }" &
            ".badge-planned { background-color: #eceff3; color: #52606d; }" &
            "blockquote { margin: 0 0 16px 0; padding: 2px 0 2px 20px; border-left: 3px solid #cfd8e3; color: #52606d; font-style: italic; }" &
            ".muted { color: #7b8794; font-size: 14px; }"

        Private Const StarterDocumentHtml As String =
            "<p class=""eyebrow"">Client brief</p>" &
            "<h1>Northwind Traders platform rollout</h1>" &
            "<p class=""lede"">A phased migration of order capture, invoicing, and partner reporting onto the new " &
            "platform, delivered market by market with no interruption to trading.</p>" &
            "<div class=""callout"">" &
            "<p><strong>Steering review:</strong> 4 September &nbsp;&middot;&nbsp; <strong>Go-live window:</strong> " &
            "22 September to 6 October &nbsp;&middot;&nbsp; <strong>Owner:</strong> Alicia Kwan</p>" &
            "</div>" &
            "<h2>What is in scope</h2>" &
            "<ul>" &
            "<li><strong>Order capture</strong> for all direct and partner channels, including the bulk import path.</li>" &
            "<li><strong>Invoicing</strong> with the revised tax rules for each regional entity.</li>" &
            "<li><strong>Partner reporting</strong>, replacing the nightly export with live figures.</li>" &
            "<li>Historical data for the trailing twenty-four months, reconciled before cutover.</li>" &
            "</ul>" &
            "<h2>Coverage by market</h2>" &
            "<table>" &
            "<tr><th>Market</th><th>Status</th><th>Owner</th><th>Go-live</th></tr>" &
            "<tr><td>Northeast</td><td><span class=""badge badge-live"">Live</span></td><td>Alicia Kwan</td><td>Complete</td></tr>" &
            "<tr class=""alt""><td>Midwest</td><td><span class=""badge badge-review"">In review</span></td><td>Diego Ferreira</td><td>22 September</td></tr>" &
            "<tr><td>West coast</td><td><span class=""badge badge-planned"">Planned</span></td><td>Priya Natarajan</td><td>6 October</td></tr>" &
            "<tr class=""alt""><td>Canada</td><td><span class=""badge badge-planned"">Planned</span></td><td>Priya Natarajan</td><td>Q4</td></tr>" &
            "</table>" &
            "<h2>Commercial terms</h2>" &
            "<blockquote>Fees are fixed for the initial three markets. Any market added after the September " &
            "steering review is quoted separately against the same rate card.</blockquote>" &
            "<p>The full rate card and the support commitments are published on the " &
            "<a href=""https://www.spicelogic.com"">partner portal</a>.</p>" &
            "<p class=""muted"">Prepared by the delivery team &nbsp;&middot;&nbsp; Revision 4</p>"

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.

            ' HeaderStyleContent writes a <style> block into the document head, so the content
            ' is styled by real CSS rather than inline formatting - exactly how you would brand
            ' documents your end users author.
            Editor.HeaderStyleContentElementID = DocumentStyleElementId
            Editor.HeaderStyleContent = DocumentStyle

            Editor.BodyHtml = StarterDocumentHtml
        End Sub

        Private Sub Editor_HtmlChanged(sender As Object, e As EventArgs)
            ' BodyHtml reflects the editor's current content whenever it is read from inside
            ' a HtmlChanged handler, so no extra step is needed here. For reading BodyHtml from
            ' outside an editor event, for example from a data-bound view model, see the
            ' 02-MvvmDataBinding sample and its UpdateBindings() call.
            Dim currentHtml As String = Editor.BodyHtml
            Dim plainText As String = Regex.Replace(currentHtml, "<.*?>", String.Empty)
            CharacterCountText.Text = $"{plainText.Length} characters"
        End Sub

    End Class

End Namespace
