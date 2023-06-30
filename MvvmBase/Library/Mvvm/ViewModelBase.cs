using System.ComponentModel;

namespace Riesling.Library.Mvvm {
	public class ViewModelBase : ModelBase {

		#region Events

		protected override void RaisePropertiesChanged(IEnumerable<string> propertyNames) {
			if (MainThread.IsMainThread) {
				base.RaisePropertiesChanged(propertyNames);
			} else {
				MainThread.BeginInvokeOnMainThread(() => {
					lock (this) {
						base.RaisePropertiesChanged(propertyNames);
					}
				});
			}
		}

		#endregion

	}

	public abstract class ViewModelBase<TModelBase> : ViewModelBase
		where TModelBase : ModelBase {

		#region Instances

		protected TModelBase _Model;

		#endregion

		#region Properties

		public TModelBase Model {
			get => _Model;
			set => SetProperty(ref _Model, value, Model_Changed);
		}

		protected virtual void Model_Changed(TModelBase newModel, TModelBase oldModel) {
			if (oldModel != null) {
				oldModel.PropertyChanged -= Model_PropertyChanged;
			}
			if (newModel != null) {
				newModel.PropertyChanged += Model_PropertyChanged;
			}
		}

		#endregion

		#region Constructor

		public ViewModelBase(TModelBase model) {
			Model = model;
		}

		protected abstract void Model_PropertyChanged(object sender, PropertyChangedEventArgs e);

		#endregion

	}
}
