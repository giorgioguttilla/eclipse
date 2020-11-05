// Shader "Custom/DitherPPE"
// {
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
// 		_DitherTex ("Dither Texture", 2D) = "" {}
// 		_ShadowHue ("Shadow Hue", Color) = (0,0,0,0)
// 		_NumColors ("Color Palette", Range(1,255)) = 20
//     }
//     SubShader
//     {
//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #pragma target 3.0
// 			#include "UnityCG.cginc"

//             // note: no SV_POSITION in this struct
//             struct v2f {
//                 float2 uv : TEXCOORD0;
//             };

//             v2f vert (
//                 float4 vertex : POSITION, // vertex position input
//                 float2 uv : TEXCOORD0, // texture coordinate input
//                 out float4 outpos : SV_POSITION // clip space position output
//                 )
//             {
//                 v2f o;
//                 o.uv = uv;
//                 outpos = UnityObjectToClipPos(vertex);
//                 return o;
//             }

//             sampler2D _MainTex;
// 			sampler2D _DitherTex;
// 			fixed4 _DitherTex_TexelSize;
// 			float4 _ShadowHue;

// 			half _NumColors;

//             fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
//             {
//                 //gets input texture coordinate
// 				float4 texColor = tex2D(_MainTex, i.uv);

// 				//calculates dither value at screen space coordinate
//                 float2 ditherCoordinate = screenPos.xy * _DitherTex_TexelSize.xy;// * _ScreenParams.xy * _DitherTex_TexelSize.xy;
// 				half4 ditherValue = tex2D(_DitherTex, ditherCoordinate);

				
// 				//BW
// 				//float col = step(ditherValue, texColor);

// 				//Cool colors
// 				//float4 col = step(ditherValue, texColor);

// 				float shadow = step(ditherValue, texColor);

// 				float4 col = (floor(texColor*_NumColors)/_NumColors) * float4(_ShadowHue.rgb, 1) * clamp(shadow, _ShadowHue.a, 1);

// 				return col;
//             }
//             ENDCG
//         }
//     }
// }
Shader "Custom/DitherPPE"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DitherTex ("Dither Texture", 2D) = "" {}
		_NumColors ("Color Palette", Range(1,50)) = 20
        _DitherHarhness("Dither Harshness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
			#include "UnityCG.cginc"

            // note: no SV_POSITION in this struct
            struct v2f {
                float2 uv : TEXCOORD0;
            };

            v2f vert (
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0, // texture coordinate input
                out float4 outpos : SV_POSITION // clip space position output
                )
            {
                v2f o;
                o.uv = uv;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

            sampler2D _MainTex;
			sampler2D _DitherTex;
			fixed4 _DitherTex_TexelSize;
            float _DitherHarhness;

			half _NumColors;

            //returns an approximate color by dividing the color space evenly
            float3 nearestColors (float3 i){
                return float3(
                    floor(i.r * _NumColors)/_NumColors,
                    floor(i.g * _NumColors)/_NumColors,
                    floor(i.b * _NumColors)/_NumColors
                );
            }

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                //gets input texture coordinate
				float4 texColor = tex2D(_MainTex, i.uv);

				//calculates dither value at screen space coordinate
                float2 ditherCoordinate = screenPos.xy * _DitherTex_TexelSize.xy;// * _ScreenParams.xy * _DitherTex_TexelSize.xy;
				//half4 ditherValue = tex2D(_DitherTex, ditherCoordinate);
				half ditherValue = tex2D(_DitherTex, ditherCoordinate).r;
                
                float4 c = texColor + _DitherHarhness * (ditherValue - 0.5);

                c = float4(nearestColors(c.rgb), c.a);

                return c;
            }
            ENDCG
        }
    }
}