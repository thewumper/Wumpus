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
        private float magnitude;
        [SerializeField]
        private float speed;
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        
        [SerializeField]
        private Shader shader;
        private Material material;
        private static readonly int Width = Shader.PropertyToID("_Width");
        private static readonly int Height = Shader.PropertyToID("_Height");
        private static readonly int DistortionSpeed = Shader.PropertyToID("_DistortionSpeed");
        private static readonly int DistortionMagnitude = Shader.PropertyToID("_DistortionMagnitude");
        private static readonly int DistortionMinTime = Shader.PropertyToID("_DistortionMinTime");
        private static readonly int DistortionMaxTime = Shader.PropertyToID("_DistortionMaxTime");

        public ShaderApplication(float maxTime)
        {
            this.maxTime = maxTime;
        }

        public void Awake()
        {
            material = new Material(shader);
        }

        public void Update()
        {
            material.SetInteger(Width,width);
            material.SetInteger(Height,height);
            material.SetFloat(DistortionSpeed,speed);
            material.SetFloat(DistortionMagnitude,magnitude);
            material.SetFloat(DistortionMinTime,minTime);
            material.SetFloat(DistortionMaxTime,maxTime);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
  
            Graphics.Blit(source,destination,material);
        }
    }
}