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

    public bool render;
    public bool stepRender;

    public int size;
    public bool refresh;

    public int buffSwap = 0;
    public void Start()
    {
        InitRenderTexture(ref _target);
        
    }
    struct Pixel
    {
        int val;
    }
    private void InitRenderTexture(ref RenderTexture ren)
    {
        if (ren == null || ren.width !=  size || ren.height != size)
        {
            Screen.SetResolution(size, size, false);
            // Release render texture if we already have one
            if (ren != null)
                ren.Release();
            ren = new RenderTexture(size, size, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            ren.antiAliasing = 1;
            ren.enableRandomWrite = true;
            ren.Create();
        }
    }
    private unsafe void InitStructedBuffer(ref ComputeBuffer comp)
    {
        if (comp.count != Screen.width * Screen.height || !comp.IsValid())
        {
            if (comp != null)
                comp.Release();
            comp = new ComputeBuffer(Screen.width * Screen.height, sizeof(Pixel));
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (render)
        {
            buffSwap = buffSwap == 0 ? 1 : 0;



            shader.SetBool("regen", refresh);
            if (refresh)
            {
                refresh = false;

            }
            InitRenderTexture(ref _target);

            shader.SetTexture(0, "Result", _target);
            shader.SetInt("beff", buffSwap);
            shader.SetInt("Size", size);
            int threadGroupsX = Mathf.CeilToInt((float)size / 8.0f);
            int threadGroupsY = Mathf.CeilToInt((float)size / 8.0f);
            shader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            Debug.Log("e");

            Render(destination);
            if(stepRender)
            {
                render = false;
            }
        }
    }
    private void Render(RenderTexture destination)
    {
        

        // compute shit

        Graphics.Blit(_target, destination);
    }
    
}
