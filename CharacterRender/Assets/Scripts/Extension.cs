using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static Color ToColor(this float3 f)
        {
            return new Color(f.x, f.y, f.z);
        }
        public static Texture2D ToTexture2D(this RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
        public static void TexturesToSpriteSheet(this Texture2D[] tex)
        {
            // stitch an array of textures into a sprite sheet
            for(int i = 0; i < tex.Length; i++)
            {

            }
        }
        public static void SetFrame(this Animator anim, string stateName, int layer, float frame, float totalFrames)
        {
            anim.Play(stateName, layer, frame /totalFrames);
            anim.Update(0);
            //Debug.Log(frame / totalFrames);
        }
        public static List<Transform> GetAllChildren(this Transform trans, List<Transform> children)
        {
            if(children == null)
            {
                children = new List<Transform>();
            }
            foreach(Transform child in trans)
            {
                if (child == null)
                    continue;
                children.Add(child);
                GetAllChildren(child, children);
            }
            return children;
        }
    }
}
