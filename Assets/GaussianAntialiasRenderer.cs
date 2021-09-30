using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GaussianAntialiasRenderer : MonoBehaviour
{
    public RenderTexture[] renderTextures1080;
    public RenderTexture[] renderTextures360;
    private RenderTexture[] renderTextures;
    public RenderTexture outputRenderTexture;
    public CameraJitter[] cameraJitters;
    private MeshRenderer renderer;
    private Shader shader;
    private bool antialiasingEnabled = true;
    private bool using1080 = true;
    public TMPro.TextMeshProUGUI text;
    private readonly string onString = "ON";
    private readonly string offString = "OFF";

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        //shader = renderer.material.shader;
        //renderer.material.SetTexture("SecondSourceTexture", renderTextures[1]);
        renderTextures = renderTextures1080;
        UpdateRenderTargets();
    }


    // Start is called before the first frame update
    void Start()
    {
        renderer.material.SetTexture("_FirstSourceTex", renderTextures[0]);
        renderer.material.SetTexture("_SecondSourceTex", renderTextures[1]);
        renderer.material.SetTexture("_ThirdSourceTex", renderTextures[2]);
        renderer.material.SetTexture("_FourthSourceTex", renderTextures[3]);
        renderer.material.SetTexture("_FifthSourceTex", renderTextures[4]);
        renderer.material.SetTexture("_SixthSourceTex", renderTextures[5]);
        renderer.material.SetTexture("_SeventhSourceTex", renderTextures[6]);
        renderer.material.SetTexture("_EighthSourceTex", renderTextures[7]);
        renderer.material.SetTexture("_NinthSourceTex", renderTextures[8]);
        renderer.material.SetTexture("_TenthSourceTex", renderTextures[9]);
        renderer.material.SetTexture("_EleventhSourceTex", renderTextures[10]);
        renderer.material.SetTexture("_TwelvthSourceTex", renderTextures[11]);
        renderer.material.SetTexture("_ThirteenthSourceTex", renderTextures[12]);
        renderer.material.SetTexture("_FourteenthSourceTex", renderTextures[13]);
        renderer.material.SetTexture("_FifteenthSourceTex", renderTextures[14]);
        renderer.material.SetTexture("_SixteenthSourceTex", renderTextures[15]);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            antialiasingEnabled = !antialiasingEnabled; // toggle
            for (int i = 0; i < cameraJitters.Length; i++)
            {
                cameraJitters[i].enabled = antialiasingEnabled;
            }
            UpdateTextures();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            using1080 = !using1080;
            if (using1080)
            {
                renderTextures = renderTextures1080;                
            }
            else
            {
                renderTextures = renderTextures360;
            }
            UpdateTextures();
            UpdateRenderTargets();
        }
    }


    private void UpdateTextures()
    {
        if (antialiasingEnabled)
        {
            renderer.material.SetTexture("_FirstSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_SecondSourceTex", renderTextures[1]);
            renderer.material.SetTexture("_ThirdSourceTex", renderTextures[2]);
            renderer.material.SetTexture("_FourthSourceTex", renderTextures[3]);
            renderer.material.SetTexture("_FifthSourceTex", renderTextures[4]);
            renderer.material.SetTexture("_SixthSourceTex", renderTextures[5]);
            renderer.material.SetTexture("_SeventhSourceTex", renderTextures[6]);
            renderer.material.SetTexture("_EighthSourceTex", renderTextures[7]);

            renderer.material.SetTexture("_NinthSourceTex", renderTextures[8]);
            renderer.material.SetTexture("_TenthSourceTex", renderTextures[9]);
            renderer.material.SetTexture("_EleventhSourceTex", renderTextures[10]);
            renderer.material.SetTexture("_TwelvthSourceTex", renderTextures[11]);
            renderer.material.SetTexture("_ThirteenthSourceTex", renderTextures[12]);
            renderer.material.SetTexture("_FourteenthSourceTex", renderTextures[13]);
            renderer.material.SetTexture("_FifteenthSourceTex", renderTextures[14]);
            renderer.material.SetTexture("_SixteenthSourceTex", renderTextures[15]);

            text.text = onString;
        }
        else
        {
            renderer.material.SetTexture("_FirstSourceTex", renderTextures[0]); // All zeroes here is on purpose. This practically disables the jitter.
            renderer.material.SetTexture("_SecondSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_ThirdSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_FourthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_FifthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_SixthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_SeventhSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_EighthSourceTex", renderTextures[0]);

            renderer.material.SetTexture("_NinthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_TenthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_EleventhSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_TwelvthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_ThirteenthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_FourteenthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_FifteenthSourceTex", renderTextures[0]);
            renderer.material.SetTexture("_SixteenthSourceTex", renderTextures[0]);

            text.text = offString;
        }
        Debug.Log((antialiasingEnabled ? "On." : "Off.") + " " + (using1080 ? "High res." : "Low res."));
    }

    private void UpdateRenderTargets()
    {
        for (int i = 0; i < cameraJitters.Length; i++)
        {
            cameraJitters[i].GetComponent<Camera>().targetTexture = renderTextures[i];
            cameraJitters[i].SetDimensions(renderTextures[i].width, renderTextures[i].height);
        }
        renderer.material.SetFloat("_ScreenWidth", renderTextures[0].width);
        renderer.material.SetFloat("_ScreenHeight", renderTextures[0].height);
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
