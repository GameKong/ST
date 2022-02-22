using GameFramework.Fsm;
using UnityEngine;
using PlayerFsm = GameFramework.Fsm.IFsm<ST.PlayerLogic>;

namespace ST
{
    /// <summary>
    /// 玩家状态基类。
    /// </summary>
    public abstract class PlayerState : FsmState<PlayerLogic>
    {
        private PlayerLogic m_EntityLogic;
        private Transform m_PlayerTransform;
        private CharacterController m_PlayerController;
        private Animator m_PlayerAnimator;

        /// <summary>
        /// 玩家实体逻辑类
        /// </summary>
        public PlayerLogic EntityLogic
        {
            get
            {
                return m_EntityLogic;
            }

            set
            {
                m_EntityLogic = value;
            }
        }

        /// <summary>
        /// 玩家位置。
        /// </summary>
        public Transform PlayerTransform
        {
            get
            {
                return m_PlayerTransform;
            }

            set
            {
                m_PlayerTransform = value;
            }
        }

        /// <summary>
        /// 玩家控制器。
        /// </summary>
        public CharacterController PlayerController
        {
            get
            {
                return m_PlayerController;
            }

            set
            {
                m_PlayerController = value;
            }
        }

        /// <summary>
        /// 玩家动画控制器
        /// </summary>
        public Animator PlayerAnimator
        {
            get
            {
                return m_PlayerAnimator;
            }

            set
            {
                m_PlayerAnimator = value;
            }
        }

        /// <summary>
        /// 状态初始化时调用。
        /// </summary>
        /// <param name="PlayerFsm">玩家有限状态机</param>
        protected override void OnInit(PlayerFsm playerFsm)
        {
            base.OnInit(playerFsm);

            EntityLogic = playerFsm.Owner as PlayerLogic;
            m_PlayerTransform = EntityLogic.CachedTransform;
            m_PlayerController = EntityLogic.GetComponent<CharacterController>();
            PlayerAnimator = EntityLogic.GetComponent<Animator>();
        }

        /// <summary>
        /// 进入状态时调用。
        /// </summary>
        /// <param name="PlayerFsm">玩家有限状态机</param>
        protected override void OnEnter(PlayerFsm playerFsm)
        {
            base.OnEnter(playerFsm);
        }

        /// <summary>
        /// 状态轮询时调用。
        /// </summary>
        /// <param name="PlayerFsm">玩家有限状态机</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected override void OnUpdate(PlayerFsm playerFsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(playerFsm, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 离开状态时调用。
        /// </summary>
        /// <param name="PlayerFsm">玩家有限状态机</param>
        /// <param name="isShutdown">是否是关闭状态机时触发。</param>
        protected override void OnLeave(PlayerFsm playerFsm, bool isShutdown)
        {
            base.OnLeave(playerFsm, isShutdown);
        }

        /// <summary>
        /// 状态销毁时调用。
        /// </summary>
        /// <param name="PlayerFsm">玩家有限状态机</param>
        protected override void OnDestroy(PlayerFsm playerFsm)
        {
            base.OnDestroy(playerFsm);
        }

        protected virtual void InputProgress(PlayerFsm playerFsm)
        {

        }

        protected bool IsInGround()
        {
            return EntityLogic.IsInGround();
        }
    }
}
