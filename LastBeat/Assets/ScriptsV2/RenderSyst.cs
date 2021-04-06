using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class RenderSyst : MonoBehaviour
{
    private RenderTexture _target;
    private RenderTexture _Data0;
    private RenderTexture _Data1;

    public ComputeShader shader;

    public int size;
    public void Start()
    {
        InitRenderTexture(ref _target, 0);
        InitRenderTexture(ref _Data0, 1);
        InitRenderTexture(ref _Data1, 1);
        
    }
    private void InitRenderTexture(ref RenderTexture ren, int type)
    {
        if (ren == null || ren.width !=  size || ren.height != size)
        {
            Screen.SetResolution(size, size, false);
            // Release render texture if we already have one
            if (ren != null)
                ren.Release();


            RenderTextureFormat renTexForm;
            // Get a render target for Ray Tracing
            switch (type)
            {
                case 0: // type float
                    renTexForm = RenderTextureFormat.ARGBFloat;
                    break;
                case 1: // type int
                    renTexForm = RenderTextureFormat.ARGBInt;
                    break;
                default :
                    renTexForm = RenderTextureFormat.ARGBFloat;
                    break;


            }
            ren = new RenderTexture(size, size, 0,
                renTexForm, RenderTextureReadWrite.Linear);
            ren.antiAliasing = 1;
            ren.enableRandomWrite = true;
            ren.Create();
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        InitRenderTexture(ref _target, 0);
        InitRenderTexture(ref _Data0, 1);
        InitRenderTexture(ref _Data1, 1);


        shader.SetTexture(0, "Result", _target);
        shader.SetTexture(0, "Data0", _Data0);
        shader.SetTexture(0, "Data1", _Data1);
        shader.SetInt("Size", size);
        int threadGroupsX = Mathf.CeilToInt((float)size / 8.0f);
        int threadGroupsY = Mathf.CeilToInt((float)size / 8.0f);
        shader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Render(destination);
    }
    private void Render(RenderTexture destination)
    {
        

        // compute shit

        Graphics.Blit(_target, destination);
    }
    
}
