# 02 - MVVM data binding

The flagship sample. A `MainViewModel` implementing `INotifyPropertyChanged` exposes an `Html`
string property, two-way bound to the editor's `BodyHtml`. A "Save to view model" button
demonstrates the one gotcha every WPF user of this control runs into: the bound property is only
as fresh as the last binding update, so you must call `editor.UpdateBindings()` before reading it
if you cannot guarantee the editor already lost focus.

**Key API used**

- `BodyHtml="{Binding Html, Mode=TwoWay, UpdateSourceTrigger=LostFocus}"`.
- `editor.UpdateBindings()`, called before reading the bound view model value.

**Run it**

```
dotnet run --project 02-MvvmDataBinding
```
