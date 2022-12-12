using UnityEngine;

namespace AKIRA.Manager.Audio {
    public class AudioPlayer : MonoBehaviour, IPool {
        [SerializeField]
        private AudioSource musicSource;
        /// <summary>
        /// 默认路径
        /// </summary>
        public const string DefaultPath = "Audio/AudioPlayer";

        public void Wake() {}
        public void Recycle() {}

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="looping"></param>
        public void Play(AudioClip clip, bool looping) {
            musicSource.clip = clip;
            musicSource.loop = looping;
            musicSource.Play();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Stop() {
            musicSource.Stop();
            musicSource.clip = null;
        }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying() => musicSource.clip != null && musicSource.isPlaying;
    }
}