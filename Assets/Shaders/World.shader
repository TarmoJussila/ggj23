Shader "Klonk/World"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WorldTex ("WorldData", 2D) = "green" {}
        _PixelsPerEntity ("Pixels per entity", int) = 6
        _CameraPos ("Camera center world position", Vector) = (0.0, 0.0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _WorldTex;
            float4 _WorldTex_ST;
            static int _PixelsPerEntity;
            float2 _CameraPos;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 offset = tex2D(_WorldTex, i.uv);
                float2 uv = float2(offset.rg);
                return fixed4(offset.rg, 0, 1);
                fixed4 col = tex2D(_MainTex, uv);
                return col; //fixed4(col.r, 1, col.b, 1);
                return fixed4(offset.rg, 0, 1);
            }
            ENDCG
        }
    }
}