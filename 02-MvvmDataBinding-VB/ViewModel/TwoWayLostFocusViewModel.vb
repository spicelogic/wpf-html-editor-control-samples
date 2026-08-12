Namespace Global.MvvmDataBinding.ViewModel

    ''' <summary>
    ''' Backs the TwoWay, LostFocus trigger scenario. A plain property is enough here: WPF's TwoWay
    ''' binding writes straight through the setter for the editor-to-view-model direction this
    ''' scenario demonstrates, so no INotifyPropertyChanged is required.
    ''' </summary>
    Public Class TwoWayLostFocusViewModel
        Public Property BodyHtml As String
    End Class

End Namespace
