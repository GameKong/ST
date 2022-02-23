using UnityEngine;

namespace ST
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuildInComponents();
            InitCustomComponents();
            // DynamicGI.UpdateEnvironment();
        }
    }
}
