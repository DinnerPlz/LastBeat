using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

public class ComputeTools
{
    public static void InitRenderTexture(ref RenderTexture ren, Vector2Int size)
    {
        if (ren == null || ren.width != size.x || ren.height != size.y)
        {
            Screen.SetResolution(size.x, size.y, false);
            // Release render texture if we already have one
            if (ren != null)
                ren.Release();
            ren = new RenderTexture(size.x, size.y, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            ren.antiAliasing = 1;
            ren.enableRandomWrite = true;
            ren.Create();
        }
    }
    public static unsafe void InitStructedBuffer<T>(ref ComputeBuffer comp) where T : unmanaged
    {
        if (comp == null || comp.count != Screen.width * Screen.height)
        {
            if (comp != null)
                comp.Release();
            comp = new ComputeBuffer(Screen.width * Screen.height, sizeof(T));
        }
    }
}
public class RenderSyst : MonoBehaviour
{
    private RenderTexture _target;
    private ComputeBuffer _Data0;
    private ComputeBuffer _Data1;

    public ComputeShader shader;

    public bool render;
    public bool stepRender;

    public int size;
    public bool refresh;

    public int buffSwap = 0;
    public void Start()
    {
        
    }
    struct Pixel
    {
        int val;
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
            ComputeTools.InitRenderTexture(ref _target, new Vector2Int(size, size));
            ComputeTools.InitStructedBuffer<Pixel>(ref _Data0);
            ComputeTools.InitStructedBuffer<Pixel>(ref _Data1);

            shader.SetBuffer(0, "Data0", _Data0);
            shader.SetBuffer(0, "Data1", _Data1);
            shader.SetTexture(0, "Result", _target);
            shader.SetInt("buff", buffSwap);
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
    private void OnApplicationQuit()
    {
        _Data0.Release();
        _Data1.Release();
        _target.Release();
    }

}
