Shader "Custom/RoundedPanelWithTextureSquareFrame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0,0.5)) = 0.25
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            float _Radius;
            sampler2D _MainTex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);
                float2 offset = abs(uv - center);
                float dist = max(offset.x, offset.y);
                float alpha = saturate(smoothstep(_Radius - 0.01, _Radius + 0.01, dist));
                
                // Mapeo de la textura
                half4 texColor = tex2D(_MainTex, uv);
                
                // Aplicación de bordes redondeados
                half4 result = texColor * half4(1, 1, 1, alpha);
                
                return result;
            }
            ENDCG
        }
    }
}
