Shader "Rito/PaintTextureURP"
{
    Properties
    {
        _Color ("Tint Color", Color) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _PaintTex ("Painted Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_PaintTex);
            SAMPLER(sampler_PaintTex);

            float4 _Color;

            // Vertex Shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(float4(v.positionOS, 1.0));
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            float4 frag(Varyings i) : SV_Target
            {
                // Sample textures
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 paintTex = SAMPLE_TEXTURE2D(_PaintTex, sampler_PaintTex, i.uv);

                // Blend main texture with painted texture
                float3 blendedColor = lerp(mainTex.rgb * _Color.rgb, paintTex.rgb, paintTex.a);
                float alpha = mainTex.a * paintTex.a;

                return float4(blendedColor, alpha);
            }

            ENDHLSL
        }
    }
}
