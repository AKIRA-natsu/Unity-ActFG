using System;
using AKIRA.Data;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 计时器
    /// </summary>
    public class Timer : IUpdate, IPool {
        // 时间
        private float time;
        // 事件
        private Action<Timer> onTimerChanged;

        public void GameUpdate() {
            onTimerChanged?.Invoke(this);
            if (time > 0) {
                time -= Time.deltaTime * GameTimeManager.GameTimeSpace;
            } else {
                ReferencePool.Instance.Destory(this);
            }
        }

        public void Recycle(object data = null) {
            this.Remove(GameData.Group.Timer);
            onTimerChanged = null;
        }

        public void Wake(object data = null) {
            var args = ((float time, Action<Timer> onTimerChanged))data;
            time = args.time;
            onTimerChanged = args.onTimerChanged;
            this.Regist(GameData.Group.Timer);
        }

        /// <summary>
        /// 获得时间
        /// </summary>
        /// <returns></returns>
        public float GetTime() => time;

        /// <summary>
        /// 获得时间 hh:mm:ss
        /// </summary>
        /// <returns></returns>
        public string GetTimeToString() {
            return TimeSpan.FromSeconds(Convert.ToDouble(this.time)).ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// 立刻停止计时
        /// </summary>
        public void Stop() {
            time = -1;
        }
    }

    /// <summary>
    /// 游戏事件管理
    /// </summary>
    public class GameTimeManager : Singleton<GameTimeManager>, IUpdate {
        /// <summary>
        /// <para>秒倍速</para>
        /// <para><see cref="GameTimeSpace" />秒 == 现实1s</para>
        /// </summary>
        public const float GameTimeSpace = 20f;

        /// <summary>
        /// 现实系统时间
        /// </summary>
        public DateTime SystemTime => DateTime.Now;
        /// <summary>
        /// 现实系统时间 tostring
        /// </summary>
        public string SystemTimeString => SystemTime.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 游戏内时间
        /// </summary>
        public DateTime GameTime { get; private set; }
        /// <summary>
        /// 游戏内时间 tostring
        /// </summary>
        public string GameTimeString => GameTime.ToString("MM-dd HH:mm:ss");

        protected GameTimeManager() {
            // 第一次使用时获得系统时间，时间相关在此时间基础上进行
            GameTime = DateTime.Now;
            // 更新
            this.Regist(GameData.Group.Timer);
        }

        public void GameUpdate() {
            GameTime = GameTime.AddSeconds(Convert.ToDouble(Time.deltaTime * GameTimeSpace));
        }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="time">单位：秒</param>
        /// <param name="onTimerChanged">计时器事件</param>
        /// <returns></returns>
        public Timer CreateTimer(float time, Action<Timer> onTimerChanged) {
            var timer = ReferencePool.Instance.Instantiate<Timer>((time, onTimerChanged));
            return timer;
        }
    }
}
