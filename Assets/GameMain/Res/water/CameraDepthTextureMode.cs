using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
*********************************************************************
Copyright (C) 2021 MI
File Name:           CameraDepthTextureMode.cs
Author:              #AuthorName#
Email:               #AuthorEmail# 
CreateTime:          #CreateTime#
********************************************************************* 
*/

/// <summary>       
/// CameraDepthTextureMode
/// <summary>
namespace ST 
{
    public class CameraDepthTextureMode : MonoBehaviour 
    {
        [SerializeField]
        DepthTextureMode depthTextureMode;

        private void OnValidate()
        {
            SetCameraDepthTextureMode();
        }

        private void Awake()
        {
            SetCameraDepthTextureMode();
        }

        private void SetCameraDepthTextureMode()
        {
            GetComponent<Camera>().depthTextureMode = depthTextureMode;
        }
    }
}
