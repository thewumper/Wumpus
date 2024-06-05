using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretLogo : MonoBehaviour
{
    [SerializeField] private Texture realLogo;
    [SerializeField] private Texture fakeLogo;
    [SerializeField, Range(0, 100)] private float chanceOfWinning;

    // Start is called before the first frame update
    private void Start()
    {
        RawImage rawImage = GetComponent<RawImage>();
        
        if (Random.Range(0.0f, 100.0f) <= chanceOfWinning)
        {
            rawImage.texture = fakeLogo;
        }
        else
        {
            rawImage.texture = realLogo;
        }
    }
}
