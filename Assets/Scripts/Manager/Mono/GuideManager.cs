using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using AKIRA.UIFramework;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 指引管理器
    /// </summary>
    public class GuideManager : MonoSingleton<GuideManager> {
        // 路径
        public const string GuideDataPath = "GuideXML.xml";

        // 指引列表转换
        private List<GuideInfo> infos = new List<GuideInfo>();
        // 上一个指引类型
        private GuideCompleteType lastType = GuideCompleteType.None;

        // 当前指引键值
        [CNName("当前指引键值", true)]
        [SerializeField]
        private int currentIndex = 0;
        /// <summary>
        /// 当前指引键值
        /// </summary>
        public int CurrentGuideIndex => currentIndex;
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
        /// 是否跳过指引
        /// </summary>
        public bool Skip => skip;

        /// <summary>
        /// 当前指引接口
        /// </summary>
        /// <value></value>
        public IGuide CurrentIGuide { get; private set; }

        private async void Start() {
            if (skip)
                return;
            
            // 等待UI初始化完成
            await UniTask.WaitUntil(() => !IsApplicationOut && UIManager.IsInited);

            currentIndex = GuideIndexKey.GetInt();
            XML xml = new XML(GuideDataPath);
            if (xml.Exist()) {
                xml.Read((x) => {
                    var nodes= x.SelectSingleNode("Data").ChildNodes;
                    foreach (XmlElement node in nodes) {
                        // 提前处理一下路径问题
                        var completeType = (GuideCompleteType)node.GetAttribute(GuideInfoName.GuideCompleteType).TryParseInt();
                        var path = node.GetAttribute(GuideInfoName.ArrowTargetPath);
                        GameObject target = default;
                        if (completeType == GuideCompleteType.UIWorld) {
                            // UI实例化Manager下查找物体
                            var prefabName = path.Split("/")[0];
                            var type = $"{prefabName}Panel".GetConfigTypeByAssembley();
                            target = UIManager.Instance.Get(type).transform.Find(path.Replace($"{prefabName}/", "")).gameObject;
                        } else {
                            // 3D物体下简单找到对象
                            // FIXME: 也修改为预制体
                            target = GameObject.Find(path);
                        }

                        infos.Add(new GuideInfo() {
                            ID = node.GetAttribute(GuideInfoName.ID).TryParseInt(),
                            completeType = completeType,
                            isShowBg = node.GetAttribute(GuideInfoName.IsShowBg).TryParseInt() == 1,
                            dialog = node.GetAttribute(GuideInfoName.Dialog),
                            dialogDirection = (GuideDialogDirection)node.GetAttribute(GuideInfoName.DialogDirection).TryParseInt(),
                            useArrow = node.GetAttribute(GuideInfoName.UseArrow).TryParseInt() == 1,
                            arrowTarget = target,
                            reachDistance = node.GetAttribute(GuideInfoName.ReachDistance).TryParseFloat(),
                            controlByIGuide = node.GetAttribute(GuideInfoName.ControlByIGuide).TryParseInt() == 1,
                        });
                    }
                });
            }
            if (currentIndex == infos.Count || infos.Count == 0)
                return;
            // 开始指引
            StartGuide(currentIndex);
        }

        /// <summary>
        /// 开始指引
        /// </summary>
        private async void StartGuide(int index) {
            var info = infos[index];
            
            // 如果是IGuide控制，while直到解锁为止
            if (info.controlByIGuide) {
                CurrentIGuide = info.arrowTarget.GetComponent<IGuide>();
                // 暂停事件
                onGuideUIPause?.Invoke();
                onGuide3DPause?.Invoke();
                // 循环判断
                await UniTask.WaitUntil(CurrentIGuide.UnlockCondition);
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
        public async void NextGuide(float waitTime = 0f) {
            CurrentIGuide = null;
            GuideIndexKey.Save(++currentIndex);
            if (currentIndex >= infos.Count) {
                onGuideFinish?.Invoke();
                return;
            }

            await UniTask.Delay(Mathf.RoundToInt(waitTime * 1000));
            StartGuide(currentIndex);
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
        private void DeleteGuideKey() {
            GuideIndexKey.Delete();
        }
#endif
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
        private float heightOffset = 1f;

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
            if (info.useArrow) {
                // 设置箭头位置
                arrow3D.transform.position = target.position + Vector3.up * heightOffset;
                arrow3D.SetActive(true);
                // 设置2D箭头
                arrow2D.SetTarget(target);
            }
            // 开始更新
            this.Regist();
        }

        public void GameUpdate() {
            var iGuide = GuideManager.Instance.CurrentIGuide;
            if (iGuide == null) {
                arrow3D.transform.position = target.position + Vector3.up * heightOffset;
                arrow2D.UpdateArrow();
                var dis = Vector3.Distance(player.position, target.position);
                if (dis <= distance)
                    EndGuide();
            } else {
                var targetPosition = iGuide.GetArrowUpdatePosition();
                arrow3D.transform.position = targetPosition + Vector3.up * heightOffset;
                arrow2D.UpdateArrow(targetPosition);
                if (iGuide.FinishCondition())
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