Shader "Custom/dGrid"
{
    Properties
    {
        _MainTex ("Grid Texture", 2D) = "white" {}
        _Tiling ("Tiling (World Units)", Float) = 1.0   // 网格大小（世界单位）
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Tiling;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // 将顶点转换到世界空间
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 使用世界坐标的 XZ 平面作为 UV
                float2 uv = i.worldPos.xz / _Tiling;
                fixed4 tex = tex2D(_MainTex, uv);
                return tex * _Color;
            }
            ENDCG
        }
    }
}