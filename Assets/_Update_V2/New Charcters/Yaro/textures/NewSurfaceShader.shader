Shader "Custom/ShadowShader"
{
    Properties
    {
        _MainTex ("Background Texture", 2D) = "white" {}
        _ShadowTex ("Shadow Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
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
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            sampler2D _ShadowTex;
            float4 _Color;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 background = tex2D(_MainTex, i.uv);
                fixed4 shadow = tex2D(_ShadowTex, i.uv);
                
                // Apply shadow color
                shadow.rgb *= _Color.rgb;
                
                // Combine background and shadow
                fixed4 finalColor = background + shadow;
                
                // Apply alpha blending
                finalColor.a = max(background.a, shadow.a);
                
                return finalColor;
            }
            ENDCG
        }
    }
}
