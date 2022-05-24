using System;
using System.Collections;
using System.Collections.Generic;
using AKIRA.UIFramework;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 金钱管理
    /// 此游戏金钱有小数
    /// </summary>
    public class MoneyManager : Singleton<MoneyManager>
    {
        private float money = 0;
        /// <summary>
        /// <para>金钱</para>
        /// </summary>
        /// <value></value>
        public float Money {
            get => money;
            private set {
                money = value < 0 ? 0 : value;
                onMoneyChange?.Invoke(money);
                MoneyKey.Save(money);
            }
        }

        // 存储键
        public const string MoneyKey = "Money";
        // 金钱事件
        public Action<float> onMoneyChange;

        private MoneyManager() {
            money = MoneyKey.GetFloat(0);
        }

        /// <summary>
        /// 注册金钱改变事件
        /// </summary>
        /// <param name="onMoneyChange"></param>
        public void RegistOnMoneyChangeAction(Action<float> onMoneyChange) {
            onMoneyChange?.Invoke(money);
            this.onMoneyChange += onMoneyChange;
        }

        /// <summary>
        /// 赚取金钱
        /// </summary>
        /// <param name="value"></param>
        public void Gain(float value) {
            Money += value;
        }
    }
}
