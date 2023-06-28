using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using AKIRA.Data;
using AKIRA.UIFramework;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 指引管理器
    /// </summary>
    [Source("Source/Manager/[GuideManager]", GameData.Source.Manager)]
    public class GuideManager : MonoSingleton<GuideManager>, ISource {
        // 路径
        public const string GuideDataPath = "GuideXML.xml";
        // 3d指引物品路径
        public const string Guide3DRootPath = "Prefabs/Guidence/[Guidence]";

        // 3d指引父物体
        private Transform Guide3DRoot;
        // 指引列表转换
        private List<GuideInfo> infos = new List<GuideInfo>();
        // 上一个指引类型
        private GuideCompleteType lastType = GuideCompleteType.None;

        // 当前指引键值
        [CNName("当前指引键值", true)]
        [SerializeField]
        private int currentIndex = 0;
        // 存储名称
        private const string GuideIndexKey = "GuideIndexKey";

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

        public async UniTask Load() {
            if (skip)
                return;
            
            "指引异步加载开始".Log(GameData.Log.Guide);
            currentIndex = GuideIndexKey.GetInt();
            Guide3DRoot = Guide3DRootPath.Load<GameObject>().Instantiate().transform;
            var sceneRoot = GameObject.Find(GameData.Source.Scene)?.transform ?? new GameObject(GameData.Source.Scene).transform;
            Guide3DRoot.SetParent(sceneRoot);
            await UniTask.NextFrame();
            Init();
            await UniTask.Delay(200);
            "Xml加载完成(等待0.2f加载)".Log(GameData.Log.Guide);
            if (currentIndex >= infos.Count || infos.Count == 0) {
                "不包含指引，指引异步加载完成".Log(GameData.Log.Guide);
            } else {
                EventManager.Instance.AddEventListener(GameData.Event.OnGameStart, OnGameStart);
                "注册事件，指引异步加载完成".Log(GameData.Log.Guide);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init() {
            infos.Clear();
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
                            target = Guide3DRoot.Find(path).gameObject;
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
        }

        /// <summary>
        /// 游戏开始事件，开始指引更新
        /// </summary>
        /// <param name="data"></param>
        private void OnGameStart(object data) {
            $"进入游戏，开始指引".Log();
            StartGuide(currentIndex);
            EventManager.Instance.RemoveEventListener(GameData.Event.OnGameStart, OnGameStart);
        }

        /// <summary>
        /// 开始指引
        /// </summary>
        private async void StartGuide(int index) {
            var info = infos[index];

            $"当前指引键值： {index}，指引类型 {info.completeType} 是否是IGuide接口 {info.controlByIGuide}".Log(GameData.Log.Guide);

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
                if (lastType != GuideCompleteType.UIWorld) {
                    onGuideUIResume?.Invoke();
                    onGuide3DPause?.Invoke();
                }
            } else {
                // 向3D指引系统发送
                Guide3DSystem.Instance.ReceiveGuideInfo(info);
                if (lastType != GuideCompleteType.TDWorld) {
                    onGuide3DResume?.Invoke();
                    onGuideUIPause?.Invoke();
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
                EventManager.Instance.TriggerEvent(GameData.Event.OnGuidenceCompleted);
                return;
            }

            await UniTask.Delay(Mathf.RoundToInt(waitTime * 1000));
            StartGuide(currentIndex);
        }

        /// <summary>
        /// <para>当前指引是否是 <see cref="index" /> 且非跳过</para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsGuideOfIndex(int index) {
            return !Skip && currentIndex == index;
        }

        /// <summary>
        /// <para>当前 <see cref="CurrentIGuide" /> 是否已经开始</para>
        /// <para>当前指引的键值 <see cref="index" /> 且已经解锁状态 (会调用IGuide.UnlockCondition()) 且非跳过</para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsGuideOfInterface(int index) {
            return !Skip && currentIndex == index && CurrentIGuide.UnlockCondition();
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

        /// <summary>
        /// 停止指引
        /// </summary>
        public void StopGuide() {
            if (lastType == GuideCompleteType.UIWorld) {
                onGuideUIPause?.Invoke();
            } else {
                onGuide3DPause?.Invoke();
            }
        }

        /// <summary>
        /// 恢复指引
        /// </summary>
        public void ResumeGuide() {
            StartGuide(currentIndex);
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
        private const string Arrow3DPath = "Prefabs/Guidence/Arrow";
        private const string Arrow2DPath = "Prefabs/Guidence/Arrow2D";
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
            EventManager.Instance.AddEventListener(GameData.Event.OnGuidenceCompleted, _ => {
                GameObject.Destroy(arrow3D);
                GameObject.Destroy(arrow2D.gameObject);
            });
            // 指引停止
            GuideManager.Instance.RegistOnGuide3DPauseAction(Stop);
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
            } else {
                Stop();
            }

            // 就算不用指引也不一定是UI的，但不是UI的情况比较少
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
                // 否则就是在IGuide里重新开了一个GuideInfo传递给UI，在UI进行判断完成
                if (!UIManager.Instance.Get<GuidePanel>().Active) {
                    this.Remove();
                    return;
                } else {
                    var targetPosition = iGuide.GetArrowUpdatePosition();
                    arrow3D.transform.position = targetPosition + Vector3.up * heightOffset;
                    arrow2D.UpdateArrow(targetPosition);
                    if (iGuide.FinishCondition())
                        EndGuide();
                }
            }
        }

        /// <summary>
        /// 结束一个阶段的指引
        /// </summary>
        private void EndGuide() {
            Stop();
            GuideManager.Instance.NextGuide(1f);
        }

        /// <summary>
        /// 停止指引
        /// </summary>
        private void Stop() {
            this.Remove();
            arrow3D.SetActive(false);
            arrow2D.ClearTarget();
        }
    }
}