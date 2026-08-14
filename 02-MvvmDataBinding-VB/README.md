# 02 - MVVM data binding (VB.NET)

The VB.NET counterpart of `02-MvvmDataBinding-CS`. See that folder's README for the full description
of the five binding scenarios; the behavior is identical, only the language differs.

Because of a long-standing VB WPF code-generation bug (BC30149 / BC30420) with `Application`
classes in SDK-style projects, `App.xaml` is compiled as a `Page` rather than an
`ApplicationDefinition`, and `App.xaml.vb` bootstraps the application from a manual `Sub Main`.

**Run it**

```
dotnet run --project 02-MvvmDataBinding-VB
```

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
