using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light light;

    [SerializeField]
    private float maxInterval = 0.2f;

    [SerializeField]
    private float minLightIntensity = 0.05f;

    [SerializeField]
    private float maxFlicker = 0.5f;

    [SerializeField]
    private float flickerLerpSpeed = 100.0f;

    private float defaultIntensity;
    private float wantedIntensity;
    private bool lightState;
    private float timer;
    private float delay;

    void Start()
    {
        light = GetComponent<Light>();
        defaultIntensity = light.intensity;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            lightState = !lightState;

            ToggleLight(lightState);
        }

        light.intensity = Mathf.Lerp(light.intensity, wantedIntensity, flickerLerpSpeed * Time.deltaTime);
    }

    private void ToggleLight(bool lightOn)
    {
        if (lightOn)
        {
            wantedIntensity = defaultIntensity;
            delay = Random.Range(0, maxInterval);
        }
        else
        {
            wantedIntensity = Random.Range(0.1f, defaultIntensity);
            delay = Random.Range(minLightIntensity, maxFlicker);
        }

        timer = 0;
    }
}