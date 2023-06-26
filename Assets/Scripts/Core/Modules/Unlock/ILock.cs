namespace AKIRA.Behaviour.Unlock {
    public interface ILock {
        /// <summary>
        /// 检查条件，返回True进入解锁条件
        /// </summary>
        /// <returns></returns>
        bool CheckCondition();

        /// <summary>
        /// 解锁条件
        /// </summary>
        /// <returns></returns>
        bool UnlockCondition();

        /// <summary>
        /// 解锁完成事件
        /// </summary>
        void CompleteLock();
    }
}