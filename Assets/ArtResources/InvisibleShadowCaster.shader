Shader "Custom/InvisibleWithShadowTransparent" {
    Properties {
        // 可以在材质面板中调整透明度的属性
        _Transparency ("Transparency", Range(0, 1)) = 0
    }
    SubShader {
        // 标签设置，将渲染队列设置为透明队列
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        // 阴影投射通道，确保物体能投射阴影
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert (appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        // 主渲染通道，控制物体的透明度
        Pass {
            // 混合模式设置，实现透明效果
            Blend SrcAlpha OneMinusSrcAlpha
            // 不写入深度缓冲区
            ZWrite Off
            // 深度测试设置
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            // 获取属性面板中的透明度值
            uniform float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 返回一个透明的颜色，透明度由属性控制
                return fixed4(0, 0, 0, _Transparency);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}