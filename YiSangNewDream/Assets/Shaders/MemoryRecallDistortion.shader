Shader "Hidden/YiSang/MemoryRecallDistortion"
{
    Properties
    {
        _MainTex ("Screen", 2D) = "white" {}
        _EffectStrength ("Effect Strength", Range(0, 1)) = 0
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _EffectStrength;

            float RandomNoise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f_img input) : SV_Target
            {
                float2 uv = input.uv;
                float2 centered = uv - 0.5;
                float radius = length(centered);
                float time = _Time.y;

                float angle = sin(radius * 28.0 - time * 2.1) * 0.035 * _EffectStrength;
                float sineAngle = sin(angle);
                float cosineAngle = cos(angle);
                centered = float2(
                    centered.x * cosineAngle - centered.y * sineAngle,
                    centered.x * sineAngle + centered.y * cosineAngle
                );

                float2 distortedUv = centered + 0.5;
                distortedUv.x += sin(uv.y * 42.0 + time * 3.0) * 0.0035 * _EffectStrength;
                distortedUv.y += sin(uv.x * 31.0 - time * 2.4) * 0.0020 * _EffectStrength;

                float aberration = (0.0015 + radius * 0.0040) * _EffectStrength;
                float2 colorOffset = normalize(centered + float2(0.0001, 0.0001)) * aberration;

                fixed red = tex2D(_MainTex, distortedUv + colorOffset).r;
                fixed green = tex2D(_MainTex, distortedUv).g;
                fixed blue = tex2D(_MainTex, distortedUv - colorOffset).b;
                fixed3 color = fixed3(red, green, blue);

                float luminance = dot(color, float3(0.299, 0.587, 0.114));
                fixed3 memoryTint = lerp(color, fixed3(luminance * 1.08, luminance, luminance * 0.90), 0.22 * _EffectStrength);

                float vignette = 1.0 - smoothstep(0.25, 0.78, radius);
                memoryTint *= lerp(1.0, 0.72 + vignette * 0.28, _EffectStrength);

                float grain = RandomNoise(uv * _ScreenParams.xy + floor(time * 24.0)) - 0.5;
                memoryTint += grain * 0.035 * _EffectStrength;

                return fixed4(memoryTint, 1.0);
            }
            ENDCG
        }
    }

    Fallback Off
}
