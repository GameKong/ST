using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using PlayerFsm = GameFramework.Fsm.IFsm<ST.PlayerLogic>;

namespace ST
{
    public class JumpState : PlayerState
    {
        protected override void OnInit(PlayerFsm playerFsm)
        {
            base.OnInit(playerFsm);
            playerFsm.SetData<VarSingle>(Constant.StateValue.JumpDirection, 0f);
        }
        protected override void OnEnter(PlayerFsm playerFsm)
        {
            base.OnEnter(playerFsm);

            PlayerAnimator.SetInteger("tag", Constant.AnimationTag.Jump);

            float moveValue = (float)playerFsm.GetData<VarSingle>(Constant.StateValue.JumpDirection).Value;
            OnAction(playerFsm, moveValue);
        }

        protected override void OnUpdate(PlayerFsm playerFsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(playerFsm, elapseSeconds, realElapseSeconds);

            if (IsInGround())
            {
                InputProgress(playerFsm);
            }
        }

        protected override void InputProgress(PlayerFsm playerFsm)
        {
            float z = Input.GetAxis("Vertical");

            if (z != 0)
            {
                playerFsm.SetData<VarSingle>(Constant.StateValue.MoveDirection, z);
                ChangeState<MoveState>(playerFsm);
            }
            else
            {
                ChangeState<IdleState>(playerFsm);
            }
        }

        protected override void OnLeave(PlayerFsm playerFsm, bool isShutdown)
        {
            base.OnLeave(playerFsm, isShutdown);

            if (!isShutdown)
            {
                playerFsm.SetData<VarSingle>(Constant.StateValue.JumpDirection, 0f);
            }
        }

        protected void OnAction(PlayerFsm playerFsm, float moveValue)
        {
            (playerFsm.Owner as PlayerLogic).Jump(moveValue);
        }
    }
}

