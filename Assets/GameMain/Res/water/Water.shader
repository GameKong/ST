Shader "Water"
{
    Properties
    {
        _DepthMaxDistance("DepthMaxDistance", Float) = 7
        _ColorShallow("ColorShallow", Color) = (1,0,0,0)
        _ColorDeep("ColorDeep", Color) = (1,0,0,0)
        _TextureNoise("TextureNoise", 2D) = "white" {}
        _NoiseThreshold("NoiseThreshold", Range(0, 1)) = 0.5
        _FoamThreshold("FoamThreshold", Range(0, 1)) = 0.5
        _NoiseSpeed("NoiseSpeed", Vector) = (0.3, 0.3, 0, 0)
        _TextureDistortion("TextureDistortion", 2D) = "white" {}
        _DistortionStrength("DistortionStrength", Float) = 1
    }
    SubShader
    {
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent"}

        GrabPass{ }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv: TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 noiseUV: TEXCOORD0;
                float2 distortionUV: TEXCOORD1;
                float4 screenPosition : TEXCOORD2;
            };

            uniform half _DepthMaxDistance;
            uniform half4 _ColorShallow;
            uniform half4 _ColorDeep;
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _TextureNoise;
            uniform float4 _TextureNoise_ST;
            uniform half _NoiseThreshold;
            uniform half _FoamThreshold;
            uniform half4 _NoiseSpeed;
            uniform sampler2D _TextureDistortion;
            uniform float4 _TextureDistortion_ST;
            uniform float _DistortionStrength;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                // 计算屏幕空间坐标，未经透视除法，zw是裁剪空间的zw
                o.screenPosition = ComputeScreenPos(o.vertex);

                // 噪声纹理坐标
                o.noiseUV = TRANSFORM_TEX(v.uv, _TextureNoise);

                // 失真纹理坐标
                o.distortionUV = TRANSFORM_TEX(v.uv, _TextureDistortion);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //------------------------------------------------- 水深浅实现
                // 对深度纹理采样，得到裁剪空间下的深度映射值（0-1），非线性
                float projDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                
                // 将非线性深度值，转换到观察空间下的深度值，线性
                float eyeMaxDepth = LinearEyeDepth(projDepth01);

                // 获取观察空间下的水面深度值
                float depthWater = i.screenPosition.w;

                // 深度纹理记录的深度值 - 水面深度值 = 深度差
                float depthOffset = eyeMaxDepth - depthWater;

                // 使用深度差与最大深度差的比例对深水区和浅水区颜色做插值
                float waterDepthFactor01 = saturate(depthOffset / _DepthMaxDistance);
                float4 waterColor = lerp(_ColorShallow, _ColorDeep, waterDepthFactor01);
                //------------------------------------------------- 水深浅实现

                //------------------------------------------------- 水流动实现
                // UV动画实现噪声纹理坐标偏移
                float2 noiseUV = float2(i.noiseUV.x + _Time.y * _NoiseSpeed.x, i.noiseUV.y + _Time.y * _NoiseSpeed.y);
                //------------------------------------------------- 水流动实现

                //------------------------------------------------- 使用失真纹理使水流UV动画更自然
                // 采样失真纹理, 映射值到【-1，1】，乘失真强度
                float2 distortionSample = (tex2D(_TextureDistortion, i.distortionUV).xy * 2 - 1) * _DistortionStrength;

                // 对噪声处理坐标做失真处理
                noiseUV = float2(noiseUV.x + distortionSample.x, noiseUV.y + distortionSample.y);
                //------------------------------------------------- 使用失真纹理使水流UV动画更自然

                //------------------------------------------------- 水波纹实现
                // 采样噪声纹理
                float SampleNoise = tex2D(_TextureNoise, noiseUV).r;

                // 根据浮沫深度阈值，调整最终的阈值。使接近岸边的水浮沫更多
                float foamFactor01 = saturate(depthOffset / _FoamThreshold);
                float threshold = foamFactor01 * _NoiseThreshold;

                // 通过阈值截断
                float ColorNoise = step(threshold, SampleNoise);
                
                // 应用噪声，产生波纹
                waterColor = waterColor + ColorNoise;
                //------------------------------------------------- 水波纹实现

                return waterColor;
            }

            ENDCG
        }
    }
}