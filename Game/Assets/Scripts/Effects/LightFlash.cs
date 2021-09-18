using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    private Light2D light;
    private float origIntensity;
    private float flashTimer = -1;

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
            var t = (flashTimer + 0.1f) - Time.time;
            t = Mathf.Clamp(t * 10.0f, 0.0f, 1.0f);
            light.intensity = t * origIntensity;
        }
    }

    public void Flash()
    {
        flashTimer = Time.time;
    }
}
