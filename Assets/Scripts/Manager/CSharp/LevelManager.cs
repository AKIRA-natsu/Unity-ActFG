using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    public class LevelManager : Singleton<LevelManager>
    {
        private int level = 0;
        /// <summary>
        /// <para>关卡</para>
        /// <para>表现上关卡+1</para>
        /// </summary>
        /// <value></value>
        public int Level {
            get => level;
            set {
                level = value;
                onLevelChange?.Invoke(level);
                LevelKey.Save(level);
            }
        }

        // 存储键
        public const string LevelKey = "Level";
        // 关卡改变事件
        public Action<int> onLevelChange;

        private LevelManager() {
            level = LevelKey.GetInt(0);
        }

        /// <summary>
        /// 注册关卡改变事件
        /// </summary>
        /// <param name="onLevelChange"></param>
        public void RegistOnLevelChangeAction(Action<int> onLevelChange) {
            onLevelChange?.Invoke(level);
            this.onLevelChange += onLevelChange;
        }

        /// <summary>
        /// 下一关
        /// </summary>
        public void NextLevel() {
            Level++;
        }

        /// <summary>
        /// <para>上一关</para>
        /// <para>关卡为0不进行变化</para>
        /// </summary>
        public void LastLevel() {
            if (Level == 0)
                return;
            Level--;
        }


    }
}
