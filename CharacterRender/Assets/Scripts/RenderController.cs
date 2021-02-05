using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderSystem;
using UnityEngine.UI;
using Extensions;

public class RenderController : MonoBehaviour
{
    /*
     * Controlls all rendering
     * gets data from ui, renders each frame
     * controlls animation frame
     */
    
    public MatControl MC;
    [SerializeField]
    ViewRect view;

    public RawImage normImg, colImg;

    public Animator anim;
    public float FPS;
    public float frame;
    public float totalFrames;

    public Texture2D temp;

    float t;
    public bool render;
    

    // Start is called before the first frame update
    void Start()
    {
        MC.ReadMats();
        view = new ViewRect(64, 64, 10, MC);
        view.xPos = 8.1f;
        view.yPos = 10.2f;
        view.GenerateCam();
        anim.speed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!render)
        {
            t += Time.deltaTime;
            if (t >= 1 / FPS)
            {
                t = 0;
                frame++;
                if (frame >= totalFrames)
                    frame = 0;
                JumpTo(frame, totalFrames);
            }
        }
        if (render)
        {
            RenderAnim(0, (int)totalFrames, view);
        }



        view.Render();
        normImg.texture = view.normal;
        colImg.texture = view.color;
    }
    void RenderAnim(int _frame, int _tFrame, ViewRect rect)
    {
        render = true;
        // recersive function to render frames
        if (_frame >= _tFrame)
        {
            render = false;
            temp = view.normalSheet;
            temp.Apply();
            view.SaveSheets(@"C:\Users\Joel Danielewicz\Documents\Unity\CharacterRender\Assets\STONK");
            return; // break statment
        }
        if (_frame == 0)
            view.InitSheet((int)_tFrame); // initialize texture

        JumpTo(_frame, _tFrame);
        view.Render();
        view.StitchTex(_frame);


        _frame++; // step frame        
        RenderAnim(_frame, _tFrame, rect);
    }
    void JumpTo(float _frame, float _tFrame)
    {
        anim.SetFrame("Base Layer.Scene", 0, _frame, _tFrame);
    } 
}
