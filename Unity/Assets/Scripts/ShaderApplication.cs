using System;
using UnityEngine;

namespace WumpusUnity
{
    public class ShaderApplication : MonoBehaviour
    {
        [SerializeField]
        private int width;
        [SerializeField]
        private int height;
        [SerializeField]
        private Shader shader;
        private Material material;
        private static readonly int Width = Shader.PropertyToID("_width");
        private static readonly int Height = Shader.PropertyToID("_height");

        public void Awake()
        {
            
            material = new Material(shader);

        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetInteger(Width,width);
            material.SetInteger(Height,width);            
            Graphics.Blit(source,destination,material);
        }
    }
}