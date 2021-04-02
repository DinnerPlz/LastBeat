using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Renderer : MonoBehaviour
{
    private RenderTexture _target;

    public ComputeShader shader;
    public void Start()
    {
        InitRenderTexture();
        Screen.SetResolution(256, 256, false);
    }
    private void InitRenderTexture()
    {
        if (_target == null || _target.width != 256 || _target.height != 256)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();

            // Get a render target for Ray Tracing
            _target = new RenderTexture(256, 256, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        shader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 32.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 32.0f);
        shader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Render(destination);
    }
    public void Compute()
    {
        RenderTexture.active = _target;

        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y <256; y++)
            {
                
            }
        }
    }
    private void Render(RenderTexture destination)
    {
        

        // compute shit

        Graphics.Blit(_target, destination);
    }
    
}
