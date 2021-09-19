using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    private Light2D light;
    private float origIntensity;
    private float flashTimer = -1;

    private float defaultFlashTime = 0.1f;
    private float flashTime;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        origIntensity = light.intensity;
        light.intensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashTimer > 0)
        {
            var t = (flashTimer + flashTime) - Time.time;
            t = Mathf.Clamp(t / flashTime, 0.0f, 1.0f);
            light.intensity = t * origIntensity;
        }
    }

    public void Flash()
    {
        Flash(defaultFlashTime);
    }

    public void Flash(float flashDur)
    {
        flashTime = flashDur;
        flashTimer = Time.time;
    }
}
