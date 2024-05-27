using UnityEngine;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public float value;
    public float maxValue;

    // Update is called once per frame
    void Update()
    {
        slider.value = value / maxValue;
    }
}
