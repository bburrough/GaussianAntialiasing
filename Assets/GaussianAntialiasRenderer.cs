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


