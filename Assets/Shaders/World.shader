Shader "Klonk/World"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _World ("WorldData", 2D) = "white" {}
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
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _World;
            static int _PixelsPerEntity;
            float2 _CameraPos;

            fixed4 frag (v2f_img i) : SV_Target
            {                                
                fixed4 offset = tex2D(_World, i.uv);
                fixed4 col = tex2D(_MainTex, offset.rg);
                return col;
            }
            ENDCG
        }
    }
}
