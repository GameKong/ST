using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ST
{
    public class CustomCinemachine : GameFrameworkComponent
    {
        [SerializeField]
        private Cinemachine.CinemachineVirtualCamera m_Cinemachine;

        public Cinemachine.CinemachineVirtualCamera Cinemachine
        {
            get
            {
                return m_Cinemachine;
            }
        }

        private void Start()
        {

        }
    }
}