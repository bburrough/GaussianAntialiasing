using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRenderTexture : MonoBehaviour
{
    public RenderTexture outputTexture;

    // Start is called before the first frame update
    void Start()
    {
       // RenderTexture.active = outputTexture;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPreRender()
    {
        //Camera.main.targetTexture = outputTexture;
    }

    void OnPostRender()
    {
        //Camera.main.targetTexture = null;
        //Graphics.DrawTexture(new Rect(32, 32, Screen.width, Screen.height), outputTexture);
        Graphics.DrawTexture(new Rect(0, 0, 1920, 1080), outputTexture);
    }

}

