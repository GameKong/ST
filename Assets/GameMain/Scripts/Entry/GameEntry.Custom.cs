using UnityEngine;

namespace ST
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static Cinemachine.CinemachineVirtualCamera Cinemachine
        {
           get;
           private set;
        }

        //public static HPBarComponent HPBar
        //{
        //    get;
        //    private set;
        //}

        private static void InitCustomComponents()
        {
            Cinemachine = UnityGameFramework.Runtime.GameEntry.GetComponent<CustomCinemachine>().Cinemachine;
            //HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}
