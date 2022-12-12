using UnityEngine;
using System.Collections.Generic;

namespace AKIRA.Manager.Audio {
    public class AudioManager : Singleton<AudioManager> {
        // 场景中的音乐
        private List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
        // 音效路径
        private const string AudioPath = "Audio/";
        // 保存音乐
        private Dictionary<AudioID, AudioClip> AudioMap = new Dictionary<AudioID, AudioClip>();
        // 父节点
        private Transform root;

        protected AudioManager() {
            root = new GameObject("[AudioManager]").DontDestory().transform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="id"></param>
        /// <param name="looping"></param>
        /// <returns></returns>
        private AudioPlayer Play(AudioPlayer player, AudioID id, bool looping) {
            player.Play(FindAudio(id), looping);
            audioPlayers.Add(player);
            return player;
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="looping">是否循环</param>
        public AudioPlayer Play(AudioID id, bool looping = false) {
            AudioPlayer player = null;
            foreach (var p in audioPlayers)
                if (!p.IsPlaying()) {
                    player = p;
                    break;
                }

            if (player == null)
                player = ObjectPool.Instance.Instantiate<AudioPlayer>(AudioPlayer.DefaultPath, root);
            
            return Play(player, id, looping);
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="player"></param>
        public void Stop(AudioPlayer player) {
            player.Stop();
        }

        /// <summary>
        /// 停止并回收全部音乐
        /// </summary>
        public void Stop() {
            foreach (var player in audioPlayers) {
                player.Stop();
                ObjectPool.Instance.Destory(player);
            }
            audioPlayers.Clear();
        }

        /// <summary>
        /// Resources/Audio/下查找音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private AudioClip FindAudio(AudioID id) {
            if (AudioMap.ContainsKey(id))
                return AudioMap[id];
            var clip = $"{AudioPath}{id}".Load<AudioClip>();
            AudioMap.Add(id, clip);
            return clip;
        }
    }
}