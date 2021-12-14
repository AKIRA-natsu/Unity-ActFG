using System;
using System.ComponentModel;

namespace ActFG.Util.Listener {
    /// <summary>
    /// 监听 Value 变化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueChange<T> : INotifyPropertyChanged where T : IComparable {
        public event PropertyChangedEventHandler PropertyChanged;
        private T value;

        public T Value {
            get => value;
            set {
                if (CompareTo(this.value, value)) {
                    this.value = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CompareTo(T t1, T t2) {
            if (t1.CompareTo(t2) != 0) {
                // 数值变了
                return true;
            } else {
                return false;
            }           
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName  = "") {
            PropertyChangedEventHandler pc = this.PropertyChanged;
            if (pc != null) pc(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
