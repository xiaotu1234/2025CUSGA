Shader "Custom/InvisibleWithShadowTransparent" {
    Properties {
        // �����ڲ�������е���͸���ȵ�����
        _Transparency ("Transparency", Range(0, 1)) = 0
    }
    SubShader {
        // ��ǩ���ã�����Ⱦ��������Ϊ͸������
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        // ��ӰͶ��ͨ����ȷ��������Ͷ����Ӱ
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

        // ����Ⱦͨ�������������͸����
        Pass {
            // ���ģʽ���ã�ʵ��͸��Ч��
            Blend SrcAlpha OneMinusSrcAlpha
            // ��д����Ȼ�����
            ZWrite Off
            // ��Ȳ�������
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

            // ��ȡ��������е�͸����ֵ
            uniform float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ����һ��͸������ɫ��͸���������Կ���
                return fixed4(0, 0, 0, _Transparency);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}