# 02 - MVVM data binding

The flagship sample. Five focused windows, each demonstrating one WPF binding mode against the
editor's `BodyHtml` dependency property: OneWay, TwoWay with the LostFocus default, TwoWay with an
Explicit trigger, TwoWay with a PropertyChanged trigger, and the editor used as an ElementName
binding source. The TwoWay, Explicit scenario (an email compose window) carries the one gotcha
every WPF user of this control runs into: the bound property is only as fresh as the last binding
update, so call `editor.UpdateBindings()` before reading it on demand if you cannot guarantee the
editor already lost focus.

**Key API used**

- `BodyHtml="{Binding ..., Mode=TwoWay, UpdateSourceTrigger=LostFocus}"` (or `Default`, `Explicit`,
  `PropertyChanged`) - every standard WPF binding mode works against `BodyHtml`.
- `editor.UpdateBindings()`, called before reading a bound view model value on demand.

**Run it**

```
dotnet run --project 02-MvvmDataBinding-CS
```

A VB.NET version of this sample sits alongside in `02-MvvmDataBinding-VB`.
