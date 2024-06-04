using System;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("OverallDistortionSpeed")] [SerializeField] private float overallDistortionSpeed;
        [FormerlySerializedAs("OverallDistortionMag")] [SerializeField] private float overallDistortionMag;
        [SerializeField] private float overallDistortionFreq;
        [SerializeField] private bool doDistortionPrePixelazation;
        [SerializeField] private int posterzationBands;
        [SerializeField] private Shader shader;

        public int Width1
        {
            get => width;
            set => width = value;
        }

        public int Height1
        {
            get => height;
            set => height = value;
        }

        public float Magnitude
        {
            get => magnitude;
            set => magnitude = value;
        }

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public float MinTime
        {
            get => minTime;
            set => minTime = value;
        }

        public float MaxTime
        {
            get => maxTime;
            set => maxTime = value;
        }

        public float OverallDistortionSpeed
        {
            get => overallDistortionSpeed;
            set => overallDistortionSpeed = value;
        }

        public float OverallDistortionMag
        {
            get => overallDistortionMag;
            set => overallDistortionMag = value;
        }

        public float OverallDistortionFreq
        {
            get => overallDistortionFreq;
            set => overallDistortionFreq = value;
        }

        public bool DoDistortionPrePixelazation
        {
            get => doDistortionPrePixelazation;
            set => doDistortionPrePixelazation = value;
        }

        public int PosterzationBands1
        {
            get => posterzationBands;
            set => posterzationBands = value;
        }

        private Material material;
        private static readonly int Width = Shader.PropertyToID("_Width");
        private static readonly int Height = Shader.PropertyToID("_Height");
        private static readonly int DistortionSpeed = Shader.PropertyToID("_DistortionSpeed");
        private static readonly int DistortionMagnitude = Shader.PropertyToID("_DistortionMagnitude");
        private static readonly int DistortionMinTime = Shader.PropertyToID("_DistortionMinTime");
        private static readonly int DistortionMaxTime = Shader.PropertyToID("_DistortionMaxTime");

        private static readonly int Lines = Shader.PropertyToID("_ScanLines");
        private static readonly int LineWidth = Shader.PropertyToID("_ScanLineWidth");
        private static readonly int DistortionMag = Shader.PropertyToID("_OverallDistortionMag");
        private static readonly int DistortionFreq = Shader.PropertyToID("_OverallDistortionFrequency");
        private static readonly int OverallDistortionChangeRate = Shader.PropertyToID("_OverallDistortionChangeRate");
        private static readonly int DoDistortionAfterPixelization = Shader.PropertyToID("_DoDistortionAfterPixelization");
        private static readonly int PosterzationBands = Shader.PropertyToID("_PosterzationBands");

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
            material.SetFloat(DistortionMag,overallDistortionMag);
            material.SetFloat(DistortionFreq,overallDistortionFreq);
            material.SetFloat(OverallDistortionChangeRate,overallDistortionSpeed);
            material.SetInteger(DoDistortionAfterPixelization,(doDistortionPrePixelazation) ? 1 : 0);
            material.SetInteger(PosterzationBands,posterzationBands);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source,destination,material);
        }
    }
}