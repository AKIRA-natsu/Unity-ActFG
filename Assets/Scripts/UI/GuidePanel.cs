using UnityEngine;
using AKIRA.Manager;
using System;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Guide, "UI/Guide", WinType.Normal)]
    public class GuidePanel : GuidePanelProp, IUpdate {
        public override void Awake() {
            base.Awake();
            // 初始化隐藏
            DialogGroup.alpha = 0f;
            Mask.Active(false);
            // 注册指引事件
            GuideManager.Instance.RegistOnGuideFinishAction(() => this.gameObject.SetActive(false));
            GuideManager.Instance.RegistOnGuideUIResumeAction(() => this.gameObject.SetActive(true));
            GuideManager.Instance.RegistOnGuideUIPauseAction(() => this.gameObject.SetActive(false));
        }

        /// <summary>
        /// 接收指引
        /// </summary>
        /// <param name="info"></param>
        public void ReceiveGuideInfo(in GuideInfo info) {
            if (info.isShowBg) {
                Mask.RefreshMask(info);
                Mask.Active(true);
                Mask.DoMaskAnim(0.3f);
                Rigid.transform.position = info.arrowTarget.transform.position;
                Rigid.size = info.arrowTarget.GetComponent<RectTransform>().sizeDelta;
            } else {
                Mask.Active(false);
                Rigid.transform.localPosition = Vector3.zero;
                Rigid.size = OutScreenMark.ScreenRightTop;
            }

            if (info.dialogDirection == GuideDialogDirection.None || String.IsNullOrWhiteSpace(info.dialog)) {
                DialogGroup.alpha = 0f;
            } else {
                UpdateDialogPosition(info.dialogDirection);
                DialogContext.text = info.dialog;
                if (DialogGroup.alpha <= 0.99f) {
                    this.Repeat(() => {
                        DialogGroup.alpha = Mathf.Lerp(DialogGroup.alpha, 1f, Time.deltaTime * 5f);
                    }, () => DialogGroup.alpha <= 0.99f).End(() => {
                        DialogGroup.alpha = 1f;
                    });
                }
            }

            Rigid.enabled = true;
            this.Regist();
        }

        /// <summary>
        /// 更新Dialog位置
        /// </summary>
        /// <param name="direction"></param>
        private void UpdateDialogPosition(GuideDialogDirection direction) {
            Vector2 position = default;
            var size = DialogBg.sizeDelta;
            var x = Mathf.Abs(Screen.width / 2 - OutScreenMark.WidthOffset - size.x);
            var y = Mathf.Abs(Screen.height / 2 - OutScreenMark.HeightOffset - size.y);
            switch (direction) {
                case GuideDialogDirection.Up:
                position = new Vector2(0, y);
                break;
                case GuideDialogDirection.Down:
                position = new Vector2(0, -y);
                break;
                case GuideDialogDirection.Left:
                position = new Vector2(-x, 0);
                break;
                case GuideDialogDirection.Right:
                position = new Vector2(x, 0);
                break;
                case GuideDialogDirection.Center:
                break;
            }
            Dialog.anchoredPosition = position;
        }

        /// <summary>
        /// 结束教程
        /// </summary>
        private void EndGuide() {
            this.Remove();
            Rigid.enabled = false;
            Mask.Active(false);
            GuideManager.Instance.NextGuide(0.3f);
        }

        public void GameUpdate() {
            if (GuideManager.Instance.CurrentIGuide == null) {
                if (Input.GetMouseButtonDown(0)) {
                    // 判断是否按在目标上
                    var hit = Physics2D.Raycast(UI.UICamera.transform.position, Input.mousePosition.ScreenToUGUI());
                    if (hit.collider != null && hit.collider.Equals(Rigid))
                        EndGuide();
                }
            } else {
                if (GuideManager.Instance.CurrentIGuide.FinishCondition())
                    EndGuide();
            }

        }
    }
}