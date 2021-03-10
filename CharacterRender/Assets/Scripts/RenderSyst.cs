using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;
using Extensions;
using System.Runtime.CompilerServices;
using RenderSystem;

namespace RenderSystem
{
    public class ViewRect
    {
        public int
            height, // size in pixels for ren tex
            width, // size in pixels for ren tex
            size, // size in unity unit
            layerMask; // latermask
        public float
            xPos,
            yPos; // z not required due to orthografic camera type
        public MatControl MC;
        /*
         * possible addition of perpective and rotation values
         */

        public GameObject camObj;
        public Camera cam;
        public Texture2D normal, color;
        public Texture2D[] texs;

        public Texture2D normalSheet, colorSheet;

        public void InitSheet(int frames)
        {
            // initialize normal and color sheet to a texture of x,y pixels
            normalSheet = new Texture2D(width * frames, height);
            colorSheet = new Texture2D(width * frames, height);
        }
        public void StitchTex(int frame)
        {
            // add new texture to the back of old texute
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++)
                {
                    int xPos = frame * width + x;
                    int yPos = y;

                    


                    //normal stitch
                    normalSheet.SetPixel(xPos, yPos, normal.GetPixel(x, y));


                    //color sitich
                    colorSheet.SetPixel(xPos, yPos, color.GetPixel(x, y));
                }
            }
            colorSheet.Apply();
            normalSheet.Apply();
        }
        public void Render()
        {
            //MC.ReadMats();
            CalcNorm();
            CalcCol();
            ToTexs();
        }
        public Texture2D[] ToTexs()
        {
            Texture2D[] tex;
            tex = new Texture2D[2];
            tex[0] = new Texture2D(width, height); // color texture
            tex[1] = new Texture2D(width, height); // normal texure

            tex[0] = color;
            tex[1] = normal;

            texs = tex;
            return tex;
        } // end ToTexs
        public void CalcNorm()
        {
            //Create temp camara and render objects using a normal shader
            MC.SetNormal();
            RenderTexture renTex = new RenderTexture(width, height, 1, RenderTextureFormat.Default); // create render texture camara will render to
            cam.targetTexture = renTex;
            cam.Render();
            normal = renTex.ToTexture2D();
            normal.filterMode = FilterMode.Point;
            normal.Apply();
            return;
            //calculate norms
        } // end CalcNorm
        public void CalcCol()
        {
            // Create temp camara and render objects using a unlit color shader
            MC.SetOriginal();
            RenderTexture renTex = new RenderTexture(width, height, 1, RenderTextureFormat.Default); // create render texture camara will render to
            cam.targetTexture = renTex;
            cam.Render();
            color = renTex.ToTexture2D();
            color.filterMode = FilterMode.Point;
            color.Apply();
            return;
            //calculate norms
        } // end CalcCal
        public void GenerateCam()
        {
            // generate a camera with the varuables given

            camObj = new GameObject();
            camObj.name = "Render Cam:" + Time.time.ToString();
            Debug.Log(string.Format(
                "Render Cam created with components Height: {0}, width: {1}, size: {2}, LayerMask: {3}, xPos: {4}, yPos{5}",
                height, width, size, layerMask, xPos, yPos));
            camObj.transform.localPosition = new Vector3(xPos, yPos, -10);
            cam = camObj.AddComponent<Camera>();
            cam.orthographic = true;
            cam.aspect = width / height;
            cam.orthographicSize = size;
            cam.enabled = false;
            cam.allowMSAA = false;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0, 0, 0, 1);
            return;
        }
        public void UpdateCam()
        {
            DestroyCam();
            GenerateCam();
        }
        public void DestroyCam()
        {
            if (camObj != null)
            {
                GameObject.Destroy(camObj);
                Debug.Log("Render cam destroyed");
                return;
            }
            Debug.LogWarning("There is no camera for this object");
        }
        public Sprite NormalToSprite()
        {
            return Sprite.Create(normal, new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f));
        }
        public Sprite ColorToSprite()
        {
            return Sprite.Create(color, new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f));
        }
        public ViewRect(int _width, int _height, int _size, MatControl _MC)
        {
            height = _height;
            width = _width;
            size = _size;
            MC = _MC;
        }
        public void SaveSheets(string dir)
        {
            byte[] normByte = normalSheet.EncodeToPNG();
            byte[] colByte = colorSheet.EncodeToPNG();

            System.IO.File.WriteAllBytes(dir + "/norm.PNG", normByte);
            System.IO.File.WriteAllBytes(dir + "/col.PNG", colByte);


            //save sheets to file
        }
    };

}



/*
public class RenderSyst : MonoBehaviour
{
    public MatControl MC;
    ViewRect viewRect;
    public bool n, c, fr, t;
    // Start is called before the first frame update
    void Start()
    {
        viewRect = new ViewRect(1000, 1000, 10, MC);
        viewRect.GenerateCam();
        viewRect.Render();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(viewRect.normal, new Rect(0.0f, 0.0f, viewRect.width, viewRect.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(n)
        {
            n = false;
            viewRect.Render();
            GetComponent<SpriteRenderer>().sprite = viewRect.NormalToSprite();
        }        
        if(c)
        {
            c = false;
            viewRect.Render();
            GetComponent<SpriteRenderer>().sprite = viewRect.ColorToSprite();
        }        
        if(fr)
        {
            fr = false;
        }        
        if(t)
        {
            t = false;
        }
    }
}
*/ // I shouldn't need this
