using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretLogo : MonoBehaviour
{
    [SerializeField] private Texture realLogo;
    [SerializeField] private Texture fakeLogo;
    [SerializeField] private float chanceOfWinning;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0.0f, 100.0f) <= chanceOfWinning)
        {
            GetComponent<RawImage>().texture = fakeLogo;
        }
        else
        {
            GetComponent<RawImage>().texture = realLogo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
