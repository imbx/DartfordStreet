Shader "Hidden/Custom/CelFx"
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

        //Texture2D _RampTexture;
        TEXTURE2D_SAMPLER2D(_RampTexture, sampler_RampTexture_linear_clamp);

        float3 CalculatedRamp(float attenuation, float4 color, float RampSampleV = 0)
        {
            float rRamp = SAMPLE_TEXTURE2D(_RampTexture, sampler_RampTexture_linear_clamp, float2(attenuation, RampSampleV)).r;
            return color * rRamp;
        }

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float3 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;
            return float4(sceneColor, 1.0);
            float3 calculatedRamp0 = sceneColor * CalculatedRamp(1, float4(sceneColor, 1));
            return float4(calculatedRamp0, 1.0) + float4(CalculatedRamp(1, float4(sceneColor, 1)), 1.0);
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