Shader "Unlit/GaussianAntialias"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FirstSourceTex("FirstSourceTexture", 2D) = "white" {}
        _SecondSourceTex("SecondSourceTexture", 2D) = "white" {}
        _ThirdSourceTex("ThirdSourceTexture", 2D) = "white" {}
        _FourthSourceTex("FourthSourceTexture", 2D) = "white" {}
        _FifthSourceTex("FifthSourceTexture", 2D) = "white" {}
        _SixthSourceTex("SixthSourceTexture", 2D) = "white" {}
        _SeventhSourceTex("SeventhSourceTexture", 2D) = "white" {}
        _EighthSourceTex("EighthSourceTexture", 2D) = "white" {}

        _NinthSourceTex("NinthSourceTexture", 2D) = "white" {}
        _TenthSourceTex("TenthSourceTexture", 2D) = "white" {}
        _EleventhSourceTex("EleventhSourceTexture", 2D) = "white" {}
        _TwelvthSourceTex("TwelvthSourceTexture", 2D) = "white" {}
        _ThirteenthSourceTex("ThirteenthSourceTexture", 2D) = "white" {}
        _FourteenthSourceTex("FourteenthSourceTexture", 2D) = "white" {}
        _FifteenthSourceTex("FifteenthSourceTexture", 2D) = "white" {}
        _SixteenthSourceTex("SixteenthSourceTexture", 2D) = "white" {}

        _NoiseGain("NoiseGain", Float) = 1.0
        _ScreenWidth("ScreenWidth", Float) = 1920.0
        _ScreenHeight("ScreenHeight", Float) = 1080.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma require 2darray

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FirstSourceTex;
            float4 _FirstSourceTex_ST;
            sampler2D _SecondSourceTex;
            float4 _SecondSourceTex_ST;
            sampler2D _ThirdSourceTex;
            float4 _ThirdSourceTex_ST;
            sampler2D _FourthSourceTex;
            float4 _FourthSourceTex_ST;
            sampler2D _FifthSourceTex;
            float4 _FifthSourceTex_ST;
            sampler2D _SixthSourceTex;
            float4 _SixthSourceTex_ST;
            sampler2D _SeventhSourceTex;
            float4 _SeventhSourceTex_ST;
            sampler2D _EighthSourceTex;
            float4 _EighthSourceTex_ST;

            sampler2D _NinthSourceTex;
            float4 _NinthSourceTex_ST;
            sampler2D _TenthSourceTex;
            float4 _TenthSourceTex_ST;
            sampler2D _EleventhSourceTex;
            float4 _EleventhSourceTex_ST;
            sampler2D _TwelvthSourceTex;
            float4 _TwelvthSourceTex_ST;
            sampler2D _ThirteenthSourceTex;
            float4 _ThirteenthSourceTex_ST;
            sampler2D _FourteenthSourceTex;
            float4 _FourteenthSourceTex_ST;
            sampler2D _FifteenthSourceTex;
            float4 _FifteenthSourceTex_ST;
            sampler2D _SixteenthSourceTex;
            float4 _SixteenthSourceTex_ST;
            float _NoiseGain;
            float _ScreenWidth;
            float _ScreenHeight;

            UNITY_DECLARE_TEX2DARRAY(_MyArr);


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float random(float2 uv)
            {
                return frac(dot(uv, float2(12.9898, 78.233))*43758.5453123);
            }

            float hash12(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }


            float fract(float x)
            {
                return x - floor(x);
            }

            float hash13(float3 p3)
            {
                p3 = fract(p3 * 0.1031f);
                p3 += dot(p3, p3.zyx + 31.32f);
                return fract((p3.x + p3.y) * p3.z);
            }

            float ign(float2 v)
            {
                float3 magic = float3(0.06711056, 0.00583715, 52.9829189);
                return frac(magic.z * frac(dot(v, magic.xy)));
            }


            float pseudo(float2 v) {
                v = frac(v / 128.0f) * 128.0f + float2(-64.340622f, -72.465622f);
                return frac(dot(v.xyx * v.xyy, float3(20.390625f, 60.703125f, 2.4281209f)));
            }


            float rand3dTo1d(float3 value, float3 dotDir = float3(12.9898, 78.233, 37.719)) {
                //make value smaller to avoid artefacts
                float3 smallValue = sin(value);
                //get scalar value from 3d vector
                float random = dot(smallValue, dotDir);
                //make value more random by making it bigger and then taking the factional part
                random = frac(sin(random) * 143758.5453);
                return random;
            }


            float rand(float3 myVector) {
                return frac(sin(_Time[0] * dot(myVector, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
            }


            //sampler2D textures[] = { _FirstSourceTex, _SecondSourceTex, _ThirdSourceTex, _FourthSourceTex };

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //int sourceIndex = (int)(noise(0) * 1e5f) % 2;
                //fixed4 a = tex2D(_FirstSourceTex, i.uv);
                //fixed4 b = tex2D(_SecondSourceTex, i.uv);
                //fixed4 c = tex2D(_ThirdSourceTex, i.uv);
                //fixed4 d = tex2D(_FourthSourceTex, i.uv);

                //float3 mine;
                //mine.z = i.uv.x;
                //mine.y = i.uv.y;
                //mine.x = _Time;
                //float noiseValue = hash13(mine);

                //float2 mine;
                float3 screenCoords;
                screenCoords.x = (floor(i.uv.x * _ScreenWidth) + 0.5f) / _ScreenWidth;
                screenCoords.z = (floor(i.uv.y * _ScreenHeight) + 0.5f) / _ScreenHeight;
                screenCoords.y = fmod(_Time, 100.0f);//_Time;// frac(_Time);// / 10.0f);

#if 1
                fixed4 col;
                float noiseValue = rand3dTo1d(screenCoords) * _NoiseGain;
                if (noiseValue < 0.0625f)
                {
                    col = tex2D(_FirstSourceTex, i.uv);
                }
                else if (noiseValue < 0.125f)
                {
                    col = tex2D(_SecondSourceTex, i.uv);
                }
                else if (noiseValue < 0.1875f)
                {
                    col = tex2D(_ThirdSourceTex, i.uv);
                }
                else if (noiseValue < 0.25f)
                {
                    col = tex2D(_FourthSourceTex, i.uv);
                }
                else if (noiseValue < 0.3125f)
                {
                    col = tex2D(_FifthSourceTex, i.uv);
                }
                else if (noiseValue < 0.375f)
                {
                    col = tex2D(_SixthSourceTex, i.uv);
                }
                else if (noiseValue < 0.4375f)
                {
                    col = tex2D(_SeventhSourceTex, i.uv);
                }
                else if (noiseValue < 0.5f)
                {
                    col = tex2D(_EighthSourceTex, i.uv);
                }
                else if (noiseValue < 0.5625f)
                {
                    col = tex2D(_NinthSourceTex, i.uv);
                }
                else if (noiseValue < 0.625f)
                {
                    col = tex2D(_TenthSourceTex, i.uv);
                }
                else if (noiseValue < 0.6875f)
                {
                    col = tex2D(_EleventhSourceTex, i.uv);
                }
                else if (noiseValue < 0.75f)
                {
                    col = tex2D(_TwelvthSourceTex, i.uv);
                }
                else if (noiseValue < 0.8125f)
                {
                    col = tex2D(_ThirteenthSourceTex, i.uv);
                }
                else if (noiseValue < 0.875f)
                {
                    col = tex2D(_FourteenthSourceTex, i.uv);
                }
                else if (noiseValue < 0.9375f)
                {
                    col = tex2D(_FifteenthSourceTex, i.uv);
                }
                else
                {
                    col = tex2D(_SixteenthSourceTex, i.uv);
                }
#else
                fixed4 a = tex2D(_FirstSourceTex, i.uv);
                fixed4 b = tex2D(_SecondSourceTex, i.uv);
                fixed4 c = tex2D(_ThirdSourceTex, i.uv);
                fixed4 d = tex2D(_FourthSourceTex, i.uv);
                fixed4 e = tex2D(_FifthSourceTex, i.uv);
                fixed4 f = tex2D(_SixthSourceTex, i.uv);
                fixed4 g = tex2D(_SeventhSourceTex, i.uv);
                fixed4 h = tex2D(_EighthSourceTex, i.uv);
                fixed4 col = (a + b + c + d + e + f + g + h) / 8.0f;
#endif 

                //float stepValue = step(0.5f, noiseValue);
                //fixed4 col = lerp(
                //    lerp(a, b, step(0.25f, noiseValue)),
                //    lerp(c, d, step(0.75f, noiseValue)),
                //    step(0.5f, noiseValue));
                //fixed4 col = tex2D(_SecondSourceTex, i.uv);
                //fixed4 col = tex2D(sourceIndex == 0 ? _FirstSourceTex : _SecondSourceTex, i.uv); 
                //fixed4 col;
                //if (sourceIndex == 0)
                //    col = tex2D(_FirstSourceTex, i.uv);
                //else
                //    col = tex2D(_SecondSourceTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}


/*
------------------------------------------------------------------------------
This software is available under 2 licenses -- choose whichever you prefer.
------------------------------------------------------------------------------
ALTERNATIVE A - MIT License
Copyright (c) 2021 Bobby G. Burrough
Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
------------------------------------------------------------------------------
ALTERNATIVE B - Public Domain (www.unlicense.org)
This is free and unencumbered software released into the public domain.
Anyone is free to copy, modify, publish, use, compile, sell, or distribute this
software, either in source code form or as a compiled binary, for any purpose,
commercial or non-commercial, and by any means.
In jurisdictions that recognize copyright laws, the author or authors of this
software dedicate any and all copyright interest in the software to the public
domain. We make this dedication for the benefit of the public at large and to
the detriment of our heirs and successors. We intend this dedication to be an
overt act of relinquishment in perpetuity of all present and future rights to
this software under copyright law.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
------------------------------------------------------------------------------
*/