Shader "Curve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveY("curve_Y",Range(-0.2,0.2))=0.03
            _CurveX("curve_X",Range(-0.2,0.2)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
        "RenderPipeline" = "UniversalRenderPipeline"}
        
 
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 
 
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
            float4 _MainTex_ST;
            float _CurveX;
            float _CurveY;
 
            v2f vert (appdata v)
            {
                v2f o;
                //o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 WorldPos = TransformObjectToWorld(v.vertex);
                //获得摄像机位置,与物体位置相减
                float3 CamSubsPos = (_WorldSpaceCameraPos - WorldPos);
                float the_distance = pow( CamSubsPos.z,2);
 
                WorldPos.x += _CurveX * the_distance;
                WorldPos.y += _CurveY * the_distance;
                
                o.vertex=TransformWorldToHClip(WorldPos);
 
 
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                
                half4 col = tex2D(_MainTex, i.uv);
                //纯色
                half4 color = (0, 1, 0, 0);
                return col;
            }
            ENDHLSL
        }
    }
}