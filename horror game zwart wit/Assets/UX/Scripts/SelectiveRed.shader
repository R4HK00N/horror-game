Shader "Hidden/SelectiveRed"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "SelectiveRed"
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            TEXTURE2D(_InputTexture);
            SAMPLER(sampler_InputTexture);

            float _Intensity;
            float _RedThreshold; // nieuw

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings o;
                o.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                o.uv = GetFullScreenTriangleTexCoord(input.vertexID);
                return o;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float3 col = SAMPLE_TEXTURE2D(_InputTexture, sampler_InputTexture, input.uv).rgb;

                float gray = dot(col, float3(0.3, 0.59, 0.11));

                // hoeveel roder dan groen/blauw?
                float redness = col.r - max(col.g, col.b);

                // threshold bepaalt hoe streng
                float mask = saturate(step(_RedThreshold, redness));

                float3 finalCol = lerp(float3(gray, gray, gray), col, mask);

                return float4(lerp(col, finalCol, _Intensity), 1);
            }
            ENDHLSL
        }
    }
}
