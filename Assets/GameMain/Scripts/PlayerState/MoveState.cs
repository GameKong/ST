using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using PlayerFsm = GameFramework.Fsm.IFsm<ST.PlayerLogic>;

namespace ST
{
    public class MoveState : PlayerState
    {
        protected override void OnEnter(PlayerFsm playerFsm)
        {
            base.OnEnter(playerFsm);

            PlayerAnimator.SetInteger("tag", Constant.AnimationTag.Move);

            // float MoveDirection = playerFsm.GetData<VarSingle>(Constant.StateValue.MoveDirection).Value;
            // OnAction(playerFsm, MoveDirection);
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

            if (x != 0)
            {
                (playerFsm.Owner as PlayerLogic).Swerve(x);
            }

            if (jump != 0)
            {
                Debug.Log("move");
                Debug.Log(jump);
                playerFsm.SetData<VarSingle>(Constant.StateValue.JumpDirection, z);
                ChangeState<JumpState>(playerFsm);
            }
            else if (z != 0)
            {
                OnAction(playerFsm, z);
            }
            else
            {
                ChangeState<IdleState>(playerFsm);
            }
        }

        protected void OnAction(PlayerFsm playerFsm, float MoveDirection)
        {
            (playerFsm.Owner as PlayerLogic).Move(MoveDirection);
        }
    }
}

