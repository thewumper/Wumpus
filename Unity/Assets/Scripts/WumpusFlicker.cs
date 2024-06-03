using UnityEngine;

public class WumpusFlicker : MonoBehaviour
{
    [SerializeField]
    private float maxInterval = 0.3f;
    
    [SerializeField]
    private float maxFlicker = 10.0f;
    
    [SerializeField]
    private float minFlicker = 0.25f;

    
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
            delay = Random.Range(minFlicker, maxFlicker);
        }
        timer = 0;
    }
}