using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using AKIRA.UIFramework;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 指引管理器
    /// </summary>
    public class GuideManager : MonoSingleton<GuideManager> {
        // 路径
        public static string GuideDataPath { get; } = Path.Combine(Application.streamingAssetsPath, "GuideXML.xml");

        // 指引列表转换
        private List<GuideInfo> infos = new List<GuideInfo>();
        // 上一个指引类型
        private GuideCompleteType lastType = GuideCompleteType.None;

        // 当前指引键值
        private int currentIndex = 0;
        // 存储名称
        private const string GuideIndexKey = "GuideIndexKey";

        // 指引结束事件
        private Action onGuideFinish;
        // UI指引恢复
        private Action onGuideUIResume;
        // UI指引暂停
        private Action onGuideUIPause;
        // 3D指引恢复
        private Action onGuide3DResume;
        // 3D指引暂停
        private Action onGuide3DPause;

        [CNName("跳过指引")]
        [SerializeField]
        private bool skip = false;

        /// <summary>
        /// 当前指引接口
        /// </summary>
        /// <value></value>
        public IGuide CurrentIGuide { get; private set; }
        
        protected override void Awake() {
            base.Awake();
            currentIndex = GuideIndexKey.GetInt();
            if (File.Exists(GuideDataPath)) {
                XML xml = new XML(GuideDataPath);
                xml.Read((x) => {
                    var nodes= x.SelectSingleNode("Data").ChildNodes;
                    foreach (XmlElement node in nodes) {
                        infos.Add(new GuideInfo() {
                            ID = node.GetAttribute(GuideInfoName.ID).TryParseInt(),
                            completeType = (GuideCompleteType)node.GetAttribute(GuideInfoName.GuideCompleteType).TryParseInt(),
                            isShowBg = node.GetAttribute(GuideInfoName.IsShowBg).TryParseInt() == 1,
                            dialog = node.GetAttribute(GuideInfoName.Dialog),
                            dialogDirection = (GuideDialogDirection)node.GetAttribute(GuideInfoName.DialogDirection).TryParseInt(),
                            arrowTarget = GameObject.Find(node.GetAttribute(GuideInfoName.ArrowTargetPath)),
                            reachDistance = node.GetAttribute(GuideInfoName.ReachDistance).TryParseFloat(),
                            controlByIGuide = node.GetAttribute(GuideInfoName.ControlByIGuide).TryParseInt() == 1,
                        });
                    }
                });
            }
        }

        private void Start() {
            if (skip)
                return;

            if (currentIndex == infos.Count)
                return;
            // UI初始化完成后开始指引
            UIManager.Instance.RegistAfterUIIInitAction(() => StartCoroutine(StartGuide(currentIndex)));
        }

        /// <summary>
        /// 开始指引
        /// </summary>
        private IEnumerator StartGuide(int index) {
            var info = infos[index];
            
            // 如果是IGuide控制，while直到解锁为止
            if (info.controlByIGuide) {
                CurrentIGuide = info.arrowTarget.GetComponent<IGuide>();
                // 暂停事件
                onGuideUIPause?.Invoke();
                onGuide3DPause?.Invoke();
                // 循环判断
                while (!CurrentIGuide.UnlockCondition())
                    yield return null;
            }

            if (info.completeType == GuideCompleteType.UIWorld) {
                // 向UIGuidePanel发送
                UIManager.Instance.Get<GuidePanel>().ReceiveGuideInfo(info);
                if (lastType == GuideCompleteType.TDWorld) {
                    onGuideUIResume?.Invoke();
                    onGuide3DPause?.Invoke();
                } else if (info.controlByIGuide) {
                    onGuideUIResume?.Invoke();
                }
            } else {
                // 向3D指引系统发送
                Guide3DSystem.Instance.ReceiveGuideInfo(info);
                if (lastType == GuideCompleteType.UIWorld) {
                    onGuide3DResume?.Invoke();
                    onGuideUIPause?.Invoke();
                } else if (info.controlByIGuide) {
                    onGuide3DResume?.Invoke();
                }
            }

            // 重新赋值
            lastType = info.completeType;
        }

        /// <summary>
        /// 下一个指引
        /// </summary>
        public void NextGuide(float waitTime = 0f) {
            CurrentIGuide = null;
            GuideIndexKey.Save(++currentIndex);
            if (currentIndex >= infos.Count) {
                onGuideFinish?.Invoke();
                return;
            }
            
            this.Delay(() => StartCoroutine(StartGuide(currentIndex)), waitTime);
        }

        /// <summary>
        /// 注册指引结束事件
        /// </summary>
        /// <param name="onGuideFinish"></param>
        public void RegistOnGuideFinishAction(Action onGuideFinish) {
            this.onGuideFinish += onGuideFinish;
        }

        /// <summary>
        /// 注册UI指引恢复
        /// </summary>
        /// <param name="onGuideUIResume"></param>
        public void RegistOnGuideUIResumeAction(Action onGuideUIResume) {
            this.onGuideUIResume += onGuideUIResume;
        }

        /// <summary>
        /// 注册UI指引中断
        /// </summary>
        /// <param name="onGuideUIPause"></param>
        public void RegistOnGuideUIPauseAction(Action onGuideUIPause) {
            this.onGuideUIPause += onGuideUIPause;
        }

        /// <summary>
        /// 注册3D指引恢复
        /// </summary>
        /// <param name="onGuide3DResume"></param>
        public void RegistOnGuide3DResumeAction(Action onGuide3DResume) {
            this.onGuide3DResume += onGuide3DResume;
        }

        /// <summary>
        /// 注册3D指引中断
        /// </summary>
        /// <param name="onGuide3DPause"></param>
        public void RegistOnGuide3DPauseAction(Action onGuide3DPause) {
            this.onGuide3DPause += onGuide3DPause;
        }

#if UNITY_EDITOR
        [ContextMenu("DeleteGuideKey")]
#endif
        private void DeleteGuideKey() {
            GuideIndexKey.Delete();
        }
    }

    /// <summary>
    /// 3D世界指引
    /// </summary>
    internal class Guide3DSystem : Singleton<Guide3DSystem>, IUpdate {
        // 玩家
        private Transform player;
        // 距离目标
        private Transform target;
        // 距离
        private float distance;

        // 箭头路径
        internal const string Arrow3DPath = "Prefabs/Arrow";
        internal const string Arrow2DPath = "Prefabs/Arrow2D";
        // 箭头
        private GameObject arrow3D;
        // 玩家身下的箭头
        private Arrow arrow2D;
        // 箭头高度差值
        private float heightOffset = 0.5f;

        /// <summary>
        /// 玩家标签
        /// </summary>
        private const string PlayerTag = "Player";

        protected Guide3DSystem() {
            // 寻找玩家Tag
            player = GameObject.FindWithTag(PlayerTag)?.transform;
            if (player is null)
                $"不存在GameObject Tag: {PlayerTag}".Error();
            
            // 为场景添加Arrow箭头
            arrow3D = Arrow3DPath.Load<GameObject>().Instantiate();
            arrow3D.SetParent(GuideManager.Instance.gameObject);
            arrow3D.SetActive(false);
            arrow2D = Arrow2DPath.Load<Arrow>().Instantiate();
            arrow2D.SetParent(GuideManager.Instance);
            arrow2D.Init(player);
            
            // 设置箭头高度
            if (arrow3D.TryGetComponent<MoveSelf>(out MoveSelf move))
                heightOffset += move.moveRadius;

            // 指引结束回收箭头
            GuideManager.Instance.RegistOnGuideFinishAction(() => {
                GameObject.Destroy(arrow3D);
                GameObject.Destroy(arrow2D.gameObject);
            });
        }

        /// <summary>
        /// 接收指引
        /// </summary>
        /// <param name="info"></param>
        public void ReceiveGuideInfo(in GuideInfo info) {
            target = info.arrowTarget.transform;
            distance = info.reachDistance;
            // 设置箭头位置
            var position = target.position;
            position.y += heightOffset;
            arrow3D.transform.position = position;
            arrow3D.SetActive(true);
            // 设置2D箭头
            arrow2D.SetTarget(target);
            // 开始更新
            this.Regist();
        }

        public void GameUpdate() {
            arrow2D.UpdateArrow();

            if (GuideManager.Instance.CurrentIGuide == null) {
                var dis = Vector3.Distance(player.position, target.position);
                if (dis <= distance)
                    EndGuide();
            } else {
                if (GuideManager.Instance.CurrentIGuide.FinishCondition())
                    EndGuide();
            }
        }

        /// <summary>
        /// 结束一个阶段的指引
        /// </summary>
        private void EndGuide() {
            this.Remove();
            arrow3D.SetActive(false);
            arrow2D.ClearTarget();
            GuideManager.Instance.NextGuide(1f);
        }
    }
}