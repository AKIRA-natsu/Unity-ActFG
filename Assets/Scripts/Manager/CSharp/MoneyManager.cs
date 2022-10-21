using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    public class MoneyManager : Singleton<MoneyManager>
    {
        private int money = 0;
        /// <summary>
        /// <para>关卡</para>
        /// <para>表现上关卡+1</para>
        /// </summary>
        /// <value></value>
        public int Money {
            get => money;
            set {
                money = value;
                onMoneyChange?.Invoke(money);
                // FIXME: 钱调用过多
                MoneyKey.Save(money);
            }
        }

        // 存储键
        private const string MoneyKey = "Money";
        // 金钱事件
        private Action<int> onMoneyChange;

        private MoneyManager() {
            money = MoneyKey.GetInt(0);
        }

        /// <summary>
        /// 注册金钱改变事件
        /// </summary>
        /// <param name="onMoneyChange"></param>
        public void RegistOnMoneyChangeAction(Action<int> onMoneyChange) {
            onMoneyChange?.Invoke(money);
            this.onMoneyChange += onMoneyChange;
        }

        /// <summary>
        /// 赚取金钱
        /// </summary>
        /// <param name="value"></param>
        public void Gain(int value) {
            Money += value;
        }
    }
}
