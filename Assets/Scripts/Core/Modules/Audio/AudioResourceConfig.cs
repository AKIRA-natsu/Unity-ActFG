using System.Linq;
using UnityEngine;

namespace AKIRA.Manager.Audio {
    [CreateAssetMenu(fileName = "AudioResourceConfig", menuName = "Framework/UIFramework/AudioResourceConfig", order = 0)]
    public class AudioResourceConfig : ScriptableObject {
        /// <summary>
        /// 默认路径
        /// </summary>
        public const string DefaultPath = "Config/AudioResourceConfig";

        /// <summary>
        /// 音频数据
        /// </summary>
        [System.Serializable]
        public struct AudioData {
            /// <summary>
            /// 标签
            /// </summary>
            public string tag;
            /// <summary>
            /// 音频
            /// </summary>
            public AudioClip audio;
        }

        [SerializeField]
        private AudioData[] datas;

        /// <summary>
        /// 通过Tag拿到音频
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public AudioClip FindClip(string tag) {
            return datas.SingleOrDefault(data => data.tag.Equals(tag)).audio ?? null;
        }
    }
}