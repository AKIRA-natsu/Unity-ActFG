using UnityEngine;

namespace AKIRA.Manager.Audio {
    /// <summary>
    /// 音乐播放实体
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IPool {
        /// <summary>
        /// 音频脚本
        /// </summary>
        private AudioSource musicSource;
        /// <summary>
        /// 默认路径
        /// </summary>
        public const string DefaultPath = "Prefabs/AudioPlayer";

        public void Wake(object data = null) {}
        public void Recycle(object data = null) {
            if (IsPlaying())
                Stop();
        }

        private void Awake() {
            musicSource = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="spatialBlend">是否根据距离音效声音大小</param>
        /// <param name="looping"></param>
        public void Play(AudioClip clip, bool spatialBlend, bool looping) {
            musicSource.clip = clip;
            musicSource.loop = looping;
            musicSource.spatialBlend = spatialBlend ? 1 : 0;
            musicSource.Play();
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="spatialBlend">是否根据距离音效声音大小</param>
        /// <returns></returns>
        public float Play(AudioClip clip, bool spatialBlend) {
            musicSource.clip = clip;
            musicSource.loop = false;
            musicSource.spatialBlend = spatialBlend ? 1 : 0;
            musicSource.Play();

            return clip.length;
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