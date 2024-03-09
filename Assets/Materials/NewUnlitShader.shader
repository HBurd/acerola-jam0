Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        fold_factor("fold_factor", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float deviation : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float fold_factor;

            v2f vert (appdata v)
            {
                float3 camera_dir = mul((float3x3)unity_CameraToWorld, float3(0, 0, 1));

                //float3 camera_dir = mul(UNITY_MATRIX_IT_MV, float4(0.0f, 0.0f, 1.0f, 1.0f));
                float t = -_WorldSpaceCameraPos.y / camera_dir.y;
                float3 world_curve_center = _WorldSpaceCameraPos + t * camera_dir;
                float3 camera_relative_pos = mul(unity_ObjectToWorld, v.vertex) - world_curve_center;

                v2f o;
                float deviation = fold_factor * dot(camera_relative_pos, camera_relative_pos);
                //v.vertex.y -= deviation;
                
                o.deviation = deviation;
                o.vertex = UnityObjectToClipPos(v.vertex) + float4(0.0f, deviation, 0.0f, 1.0f);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(1.0f - i.deviation / 5, 1.0f - i.deviation / 5, 1.0f - i.deviation/ 5, 255);//tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
