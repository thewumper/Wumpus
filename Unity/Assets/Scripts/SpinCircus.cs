using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCircus : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 1f;

    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
