using UnityEngine;
using UnityGameFramework.Runtime;

namespace ST
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetLogic : EntityLogicBase
    {
        [SerializeField]
        private TargetData m_TargetData = null;

        public bool IsDead
        {
            get
            {
                return m_TargetData.HP <= 0;
            }
        }


        public void ApplyDamage(EntityLogicBase attacker, int damageHP)
        {
            float fromHPRatio = m_TargetData.HPRatio;
            m_TargetData.HP -= damageHP;
            float toHPRatio = m_TargetData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                // GameEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            }

            if (m_TargetData.HP <= 0)
            {
                OnDead(attacker);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_TargetData = userData as TargetData;
            if (m_TargetData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(EntityLogicBase attacker)
        {
            // GameEntry.Entity.HideEntity(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            EntityLogicBase entity = other.gameObject.GetComponent<EntityLogicBase>();
            if (entity == null)
            {
                return;
            }

            if (entity is TargetLogic && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

            // AIUtility.PerformCollision(this, entity);
        }
    }
}
