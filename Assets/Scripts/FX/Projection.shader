Shader "Custom/Projection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGBA)", 2D) = "transparent" {}
        _Projection1 ("Albedo (RGBA)", 2D) = "white" {}
        _Projection2 ("Albedo (RGBA)", 2D) = "white" {}
        _Projection3 ("Albedo (RGBA)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Projection1;
        sampler2D _Projection2;
        sampler2D _Projection3;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Projection1;
            float2 uv_Projection2;
            float2 uv_Projection3;
        };
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        inline float unity_noise_randomValue (float2 uv)
        {
            return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
        }

        inline float unity_noise_interpolate (float a, float b, float t)
        {
            return (1.0-t)*a + (t*b);
        }

        inline float unity_valueNoise (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);

            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0 = unity_noise_randomValue(c0);
            float r1 = unity_noise_randomValue(c1);
            float r2 = unity_noise_randomValue(c2);
            float r3 = unity_noise_randomValue(c3);

            float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
            float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
            float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
            return t;
        }

        float Unity_SimpleNoise_float(float2 UV, float Scale)
        {
            float t = 0.0;

            float freq = pow(2.0, float(0));
            float amp = pow(0.5, float(3-0));
            t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

            return t;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 p1 = tex2D(_Projection1, IN.uv_Projection1) * _Color;
            fixed4 p2 = tex2D(_Projection2, IN.uv_Projection2) * _Color;
            fixed4 p3 = tex2D(_Projection3, IN.uv_Projection3) * _Color;
            
            fixed4 cp1 = lerp(c, p1, p1.a);
            fixed4 cp2 = lerp(cp1, p2, p2.a);
            fixed4 cp3 = lerp(cp2, p3, p3.a);

            half flickering = Unity_SimpleNoise_float(_Time.y * 0.2, 50);
            flickering = smoothstep(0, 0.4, flickering) * 0.5;

            float2 l = float2(IN.uv_MainTex[0] * 2000, ((_Time.y * 5) +  IN.uv_MainTex[1]) * 0.4);
            half lines = step(0.06 , Unity_SimpleNoise_float(l, 1));

            fixed4 rbgaAlbedo = cp3;
            o.Alpha = rbgaAlbedo.a;
            rbgaAlbedo = (rbgaAlbedo + flickering) * lines;
            o.Albedo = rbgaAlbedo;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
