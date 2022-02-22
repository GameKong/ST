using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;

using PlayerFsm = GameFramework.Fsm.IFsm<ST.PlayerLogic>;

namespace ST 
{
    public class IdleState : PlayerState
    {
        protected override void OnEnter(PlayerFsm playerFsm)
        {
            base.OnEnter(playerFsm);
            PlayerAnimator.SetInteger("tag", Constant.AnimationTag.Idle);
        }

        protected override void OnUpdate(PlayerFsm playerFsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(playerFsm, elapseSeconds, realElapseSeconds);

            InputProgress(playerFsm);
        }

        protected override void InputProgress(PlayerFsm playerFsm)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float jump = Input.GetAxis("Jump");

            if (jump != 0)
            {
                Debug.Log("idle");
                Debug.Log(jump);
                playerFsm.SetData<VarSingle>(Constant.StateValue.JumpDirection, 0f);
                ChangeState<JumpState>(playerFsm);
            }
            else if (x != 0 || z != 0)
            {
                if (z != 0)
                {
                    playerFsm.SetData<VarSingle>(Constant.StateValue.MoveDirection, z);
                    ChangeState<MoveState>(playerFsm);
                }

                if (x != 0)
                {
                    (playerFsm.Owner as PlayerLogic).Swerve(x);
                }
            }
        }
    }
}

