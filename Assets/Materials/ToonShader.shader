// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Lighting/CelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = ( 1, 1, 1, 1 )
        _Threshold ("Threshold", Range(2.0, 5.0)) = 2
        _Ambience ("Ambience", Range(0., 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
 
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
            };
 
            float _Threshold;
 
            float Shade(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL * _Threshold) / (_Threshold - 0.5);
            }
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
 
            v2f vert (appdata_full v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.worldNormal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);
                return o;
            }
 
            half _Ambience;
            fixed4 _LightColor0;
			fixed4 _Color;
 

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed shading = Shade(i.worldNormal, _WorldSpaceLightPos0.xyz);
                col.rgb *= saturate(shading + _Ambience) * _LightColor0.rgb;
				col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}