Shader "Klonk/World"
{
    Properties
    {
        _GroundSheet ("Ground Sheet", 2D) = "white" {}
        _WorldTex ("WorldData", 2D) = "white" {}
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
            };

            //static int _PixelsPerEntity;
            //float2 _CameraPos;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            Texture2D _GroundSheet;
            SamplerState sampler_GroundSheet_Point_Clamp;
            float4 _GroundSheet_ST;

            Texture2D _WorldTex;
            SamplerState sampler_WorldTex_Point_Clamp;
            float4 _WorldTex_ST;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 offset = _WorldTex.Sample(
                    sampler_WorldTex_Point_Clamp, i.uv);
                //fixed4 offset = tex2D(_WorldTex, i.uv);

                //return fixed4(offset.rg, 0, 1);
                fixed4 col = _GroundSheet.Sample(
                    sampler_GroundSheet_Point_Clamp, offset.rg);
                //fixed4 col = tex2D(_GroundSheet, offset.rg);
                return col;
            }
            ENDCG
        }
    }
}