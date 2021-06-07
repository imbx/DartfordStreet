Shader "Hidden/Custom/OutlineFx"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
        TEXTURE2D_SAMPLER2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2);

        float _OutlineThickness;
        float _OutlineDepthMultiplier;
        float _OutlineDepthBias;
        float _OutlineNormalMultiplier;
        float _OutlineNormalBias;

        float4 _OutlineColor;

        float SobelDepth(float ldc, float ldl, float ldr, float ldu, float ldd)
        {
            return abs(ldl - ldc) +
                abs(ldr - ldc) +
                abs(ldu - ldc) +
                abs(ldd - ldc);
        }

        float SobelSampleDepth(Texture2D t, SamplerState s, float2 uv, float3 offset)
        {
            float pixelCenter = LinearEyeDepth(t.Sample(s, uv).r);
            float pixelLeft   = LinearEyeDepth(t.Sample(s, uv - offset.xz).r);
            float pixelRight  = LinearEyeDepth(t.Sample(s, uv + offset.xz).r);
            float pixelUp     = LinearEyeDepth(t.Sample(s, uv + offset.zy).r);
            float pixelDown   = LinearEyeDepth(t.Sample(s, uv - offset.zy).r);

            return SobelDepth(pixelCenter, pixelLeft, pixelRight, pixelUp, pixelDown);
        }
        

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            // Sample the scene and our depth buffer
            float3 offset     = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;
            float3 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;
            float  sobelDepth = SobelSampleDepth(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord.xy, offset);
            sobelDepth = pow(saturate(sobelDepth) * _OutlineDepthMultiplier, _OutlineDepthBias);

            // Modulate the outline color based on it's transparency
            float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);

            // Calculate the final scene color
            float3 color = lerp(sceneColor, outlineColor, sobelDepth);
            return float4(color, 1.0);
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}