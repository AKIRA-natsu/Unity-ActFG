using System.Collections.Generic;
using AKIRA.Coroutine;

namespace AKIRA.Manager {
    /// <summary>
    /// 更新驱动管理
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager> {
        // 更新列表
        private List<IUpdate> updates = new List<IUpdate>();

        /// <summary>
        /// 注册更新，不检查是否重复（过多）
        /// </summary>
        /// <param name="update"></param>
        public void Regist(IUpdate update) {
            updates.Add(update);
        }

        /// <summary>
        /// 移除更新
        /// </summary>
        /// <param name="update"></param>
        public void Remove(IUpdate update) {
            updates.Remove(update);
        }

        private void Update() {
            // 协程更新
            CoroutineManager.Instance.UpdateCoroutine();
            // 遍历更新
            for (int i = 0; i < updates.Count; i++)
                updates[i].GameUpdate();
        }
    }
}