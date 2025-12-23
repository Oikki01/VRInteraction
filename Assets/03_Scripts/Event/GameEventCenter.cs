using UnityEngine;

namespace GameEvent
{
    public class GameEventCenter
    {
        /// <summary>
        /// 流程进度更新
        /// </summary>
        public static UnityEventCenter OnTaskProgressChanged = new UnityEventCenter();
        
        /// <summary>
        /// 任务完成
        /// </summary>
        public static UnityEventCenter OnTaskCompleted = new UnityEventCenter();
        
        /// <summary>
        /// 请求帮助
        /// </summary>
        public static UnityEventCenter OnRequestHelp = new UnityEventCenter();
    }
}