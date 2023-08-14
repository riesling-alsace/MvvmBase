using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Riesling.Library.Mvvm {
	[Serializable]
	public class ModelBase : INotifyPropertyChanged {

		#region Instances

		[NonSerialized]
		protected internal PropertyChangedEventHandler _PropertyChanged;

		#endregion

		#region Handler

		public event PropertyChangedEventHandler PropertyChanged {
			add => _PropertyChanged += value;
			remove => _PropertyChanged -= value;
		}

		#endregion

		#region Events

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
			RaisePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void RaisePropertiesChanged(params string[] propertyNames) {
			RaisePropertiesChanged(propertyNames);
		}

		protected virtual void RaisePropertiesChanged(IEnumerable<string> propertyNames) {
			foreach (var propertyName in propertyNames) {
				RaisePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) {
			_PropertyChanged?.Invoke(sender, e);
		}

		#endregion

		#region Setter

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
			if (EqualityComparer<T>.Default.Equals(storage, value)) {
				return false;
			}

			storage = value;
			RaisePropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// プロパティの値をセットし、内容が変化していた場合は指定のコールバック関数を呼び出す。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="storage"></param>
		/// <param name="value"></param>
		/// <param name="onChanged">void callback()</param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null) {
			if (EqualityComparer<T>.Default.Equals(storage, value)) {
				return false;
			}

			storage = value;
			onChanged?.Invoke();
			RaisePropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// プロパティの値をセットし、内容が変化していた場合は指定のコールバック関数を呼び出す。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="storage"></param>
		/// <param name="value"></param>
		/// <param name="onChanged">void callback(T newItems, T oldItems)</param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected virtual bool SetProperty<T>(ref T storage, T value, Action<T, T> onChanged, [CallerMemberName] string propertyName = null) {
			if (EqualityComparer<T>.Default.Equals(storage, value)) {
				return false;
			}

			var oldValue = storage;

			storage = value;
			onChanged?.Invoke(value, oldValue);
			RaisePropertyChanged(propertyName);
			return true;
		}

		#endregion

	}
}
