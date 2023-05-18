using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using AKIRA.Data;

namespace AKIRA.Manager.Audio {
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager : Singleton<AudioManager> {
        /// <summary>
        /// 本地存储键值：音乐保存名称
        /// </summary>
        public const string MusicEnableKey = "MusicEnable";
        private bool audioEnabled = true;
        /// <summary>
        /// 音乐是否激活
        /// </summary>
        public bool AudioEnabled {
            get => audioEnabled;
            set {
                if (audioEnabled == value)
                    return;
                audioEnabled = value;
                MusicEnableKey.Save(value ? 1 : 0);
            }
        }

        // 音频配置文件
        private AudioResourceConfig config;
        // 场景中正在播放的音乐
        private List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
        // 字典保存，方便下一次快速找到
        private Dictionary<string, AudioClip> ClipMap = new();

        protected AudioManager() {
            config = GameData.Path.AudioConfig.Load<AudioResourceConfig>();
            audioEnabled = MusicEnableKey.GetInt(1) == 1 ? true : false;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="position">位置，影响音效声音大小</param>
        /// <param name="looping">是否循环</param>
        public AudioPlayer Play(string tag, Vector3 position, bool looping) {
            if (TryCreatePlayer(tag, out (AudioPlayer player, AudioClip clip) audio)) {
                audio.player.transform.position = position;
                audio.player.Play(audio.clip, true, looping);
                return audio.player;
            } else {
                return default;
            }
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="looping">是否循环</param>
        public AudioPlayer Play(string tag, bool looping) {
            if (TryCreatePlayer(tag, out (AudioPlayer player, AudioClip clip) audio)) {
                audio.player.Play(audio.clip, false, looping);
                return audio.player;
            } else {
                return default;
            }
        }

        /// <summary>
        /// 自动播放/回收
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="position">位置，影响音效声音大小</param>
        public void Play(string tag, Vector3 position) {
            if (TryCreatePlayer(tag, out (AudioPlayer player, AudioClip clip) audio)) {
                audio.player.transform.position = position;
                var time = audio.player.Play(audio.clip, true);
                AutoPlay(audio.player, time);
            }
        }

        /// <summary>
        /// 自动播放/回收
        /// </summary>
        /// <param name="tag"></param>
        public void Play(string tag) {
            if (TryCreatePlayer(tag, out (AudioPlayer player, AudioClip clip) audio)) {
                var time = audio.player.Play(audio.clip, false);
                AutoPlay(audio.player, time);
            }
        }

        /// <summary>
        /// 自动播放/回收
        /// </summary>
        /// <param name="player"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private async void AutoPlay(AudioPlayer player, float time) {
            await UniTask.Delay(Mathf.RoundToInt(time * 1000));
            Stop(player);
        }

        /// <summary>
        /// 尝试获得播放器
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="audio"></param>
        /// <returns></returns>
        private bool TryCreatePlayer(string tag, out (AudioPlayer player, AudioClip clip) audio) {
            audio = default;
            if (!audioEnabled || String.IsNullOrEmpty(tag))
                return false;

            audio.clip = FindAudio(tag);
            if (audio.clip == null) {
                $"音频： Tag => {tag} 不存在".Log(GameData.Log.Warn);
                return false;
            } else {
                audio.player = ObjectPool.Instance.Instantiate<AudioPlayer>(AudioPlayer.DefaultPath);
                audioPlayers.Add(audio.player);
                return true;
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="player"></param>
        public void Stop(AudioPlayer player) {
            // 已经停掉不在列表中
            if (!audioPlayers.Contains(player))
                return;

            player.Stop();
            ObjectPool.Instance.Destory(player);
            audioPlayers.Remove(player);
        }

        /// <summary>
        /// 停止并回收全部音乐
        /// </summary>
        public void Stop() {
            foreach (var player in audioPlayers) {
                if (player.IsPlaying())
                    player.Stop();
                ObjectPool.Instance.Destory(player);
            }
            audioPlayers.Clear();
        }

        /// <summary>
        /// Resources/Audio/下查找音乐
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private AudioClip FindAudio(string tag) {
            if (ClipMap.ContainsKey(tag))
                return ClipMap[tag];
            
            var clip = config.FindClip(tag);
            if (clip != null) {
                ClipMap.Add(tag, clip);
            }
            return clip;
        }
    }
}