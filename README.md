# Description

MVVMの基礎クラスとして`INotifyPropertyChanged`から派生させた、`BindableBase`(汎用)と`ViewModelBase`(ViewModel用)を実装しました。

`ViewModelBase`はマルチスレッドも対応しています。通知を送る際にサブスレッドであればメインスレッドで通知を呼び出すようになっています。

## ModelBase

```CS
public class SampleModel : BindableBase {
  private string _Title;
  public string Title {
    get => _Title;
    set => SetProperty(ref _Title, value, TitleChanged);
  }

  protected void TitleChanged(string newValue, string oldValue) {
    // TODO
  }
}
```

## ViewModelBase(WPF/MAUI)

```CS
public class SampleViewModel : ViewModelBase<SampleModel> {
  public string Title {
    get => Model.Title;
    set => Model.Title = value;
  }

  protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e) {
    switch (e.PropertyName) {
    case nameof(SampleModel.Title):
      RaisePropertyChanged(nameof(Title));
      break;
    }
  }
}
```
