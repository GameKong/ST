
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
namespace ST
{
    public class PlayerLogic : TargetLogic
    {
        [SerializeField]
        private PlayerData m_PlayerData = null;
        private float m_SpeedMove = 5f;
        private float m_RotateSpeed = 3f;
        private float m_Gravity = -9.8f * 2f;
        private float m_JumpHeight = 3f;
        private bool m_StartJump = false;
        private float m_MoveDirection = 0f;
        private Vector3 m_GravitySpeed = Vector3.zero;
        public Transform GroundCheck;
        public float CheckRadius = 0.1f;
        public LayerMask GroundLayerMask;
        private CharacterController m_PlayerController;

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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }

            m_PlayerController = GetComponent<CharacterController>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            // GroundCheck
            GroundCheck = transform.Find("GroundCheck");
            GroundLayerMask = (LayerMask)LayerMask.GetMask("Ground");

            // 摄像机跟随玩家
            GameEntry.Cinemachine.LookAt = CachedTransform;
            GameEntry.Cinemachine.Follow = CachedTransform;

            //创建玩家状态列表
            List<PlayerState> states = new List<PlayerState>();
            Type stateBaseType = typeof(PlayerState);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsClass || types[i].IsAbstract)
                {
                    continue;
                }

                if (stateBaseType.IsAssignableFrom(types[i]))
                {
                    PlayerState stateInstance = (PlayerState)Activator.CreateInstance(types[i]);
                    states.Add(stateInstance);
                }
            }

            // 创建玩家有限状态机
            GameFramework.Fsm.IFsm<PlayerLogic> fsm = GameEntry.Fsm.CreateFsm<PlayerLogic>("player", this, states.ToArray());
            fsm.Start<IdleState>();

            // ScrollableBackground sceneBackground = FindObjectOfType<ScrollableBackground>();
            // if (sceneBackground == null)
            // {
            //     Log.Warning("Can not find scene background.");
            //     return;
            // }

            // m_PlayerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x, sceneBackground.PlayerMoveBoundary.bounds.min.z,
            //     sceneBackground.PlayerMoveBoundary.bounds.size.x, sceneBackground.PlayerMoveBoundary.bounds.size.z);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            GravityProcess();
        }

        private void GravityProcess()
        {
            if (!IsInGround())
            {
                m_GravitySpeed.y += m_Gravity * Time.deltaTime;
                PlayerController.Move(m_GravitySpeed * Time.deltaTime);
            }
            else if (m_StartJump)
            {
                m_StartJump = false;
                m_GravitySpeed = Vector3.zero;
                m_GravitySpeed = m_MoveDirection * transform.forward * m_SpeedMove;
                m_GravitySpeed.y = Mathf.Sqrt(m_JumpHeight * -2 * m_Gravity);
                PlayerController.Move(m_GravitySpeed * Time.deltaTime);
            }
        }

        public void Move(float moveVector)
        {
            float direct = moveVector >= 0 ? 1 : -1;
            Vector3 move = transform.forward * direct * m_SpeedMove;
            PlayerController.Move(move * Time.deltaTime);
        }

        public void Jump(float v)
        {
            m_StartJump = true;

            if (v == 0)
            {
                m_MoveDirection = 0;
            }
            else
            {
                m_MoveDirection = v >= 0 ? 1 : -1;
            }
        }

        public void Swerve(float v)
        {
            float rotate = v >= 0 ? 1 : -1;
            transform.Rotate(Vector3.up, rotate * m_RotateSpeed);
        }

        public bool IsInGround()
        {
            return Physics.CheckSphere(GroundCheck.position, CheckRadius, GroundLayerMask);
        }
    }
}
