using System;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace WumpusUnity
{
    public class PersistentData : MonoBehaviour
    {
        public static PersistentData Instance => instance;
        private static PersistentData instance;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        [SerializeField] private Vector3 eulerAngle;

        public Vector3 EulerAngle
        {
            get => eulerAngle;
            set => eulerAngle = value;
        }
    }
}