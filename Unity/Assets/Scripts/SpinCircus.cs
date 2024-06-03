using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCircus : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 1f;
    [SerializeField] private Vector3 spinVector;
    
    private void Update()
    {
        transform.Rotate(spinVector * (spinSpeed * Time.deltaTime));
    }
}
