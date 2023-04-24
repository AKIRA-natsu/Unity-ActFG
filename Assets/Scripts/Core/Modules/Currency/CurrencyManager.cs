using System;

namespace AKIRA.Manager {
    /// <summary>
    /// 货币
    /// </summary>
    public abstract class CurrencyManager<T> : Singleton<T> where T : class {
        protected int currency;
        /// <summary>
        /// 货币
        /// </summary>
        /// <value></value>
        public int Currency {
            get => currency;
            private set {
                currency = value;
                onCurrencyChange?.Invoke(currency);
                Key.Save(currency);
            }
        }

        // 存储键值
        protected abstract string Key { get; }
        // 货币数量改变事件
        private Action<int> onCurrencyChange;

        protected CurrencyManager() {}

        /// <summary>
        /// 注册金钱改变事件
        /// </summary>
        /// <param name="onCurrencyChange"></param>
        public void RegistOnCurrencyChangeAction(Action<int> onCurrencyChange) {
            onCurrencyChange?.Invoke(currency);
            this.onCurrencyChange += onCurrencyChange;
        }

        /// <summary>
        /// 赚取金钱
        /// </summary>
        /// <param name="value"></param>
        public void Earn(int value) {
            Currency += value;
        }

    }
}