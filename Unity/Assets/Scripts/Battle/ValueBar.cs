using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float value;
    public float Value
    {
        get
        {
            return value;
        }
        set
        {
            if (writable)
            {
                this.value = value;
                if (this.value < 0f)
                {
                    this.value = 0f;
                }
                if (this.value > this.maxValue)
                {
                    this.value = this.maxValue;
                }
            }
        }
    }

    [SerializeField] private float maxValue;
    public float MaxValue
    {
        get => maxValue;
    }
    [HideInInspector] public bool writable = true;

    private void Start()
    {
        writable = true;
        value = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Value / maxValue;
    }
}
