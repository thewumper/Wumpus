using UnityEngine;

public class WumpusFlicker : MonoBehaviour
{
    [SerializeField, Range(1,100)] private int chanceForLongFlicker = 8;
 
    [SerializeField] private float minLongFlicker = 1.0f;
    
    [SerializeField] private float maxLongFlicker = 4.0f;
    
    [SerializeField] private float maxInterval = 0.16f;
    
    [SerializeField] private float maxFlicker = 0.2f;
    
    [SerializeField] private float minFlicker = 0.04f;
    
    private MeshRenderer model;
    private bool lightState;
    private bool lightVisuallyOn;
    private float timer;
    private float delay;

    void Awake()
    {
        model = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            lightState = !lightState;

            ToggleWumpus(lightState);
        }

        model.enabled = lightVisuallyOn;
    }

    private void ToggleWumpus(bool lightOn)
    {
        if (lightOn)
        {
            lightVisuallyOn = true;
            delay = Random.Range(0, maxInterval);
        }
        else
        {
            lightVisuallyOn = false;

            // Random chance to have a really long flicker
            delay = Random.Range(1, 100) <= chanceForLongFlicker ? Random.Range(minLongFlicker, maxLongFlicker) : Random.Range(minFlicker, maxFlicker);
        }
        
        timer = 0;
    }
}