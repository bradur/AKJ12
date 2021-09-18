using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryLightArray : MonoBehaviour
{
    private List<FactoryLight> lights;
    private GameObject lightPrefab;
    private int activatedLight = 0;
    private bool flashLights = false;
    private float lastFlashedTime = 0;
    private float flashDuration = 0.1f;

    public void Initialize(int lightCount, GameObject lightPrefab)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        lights = GetComponentsInChildren<FactoryLight>().ToList();
        lights = lights.OrderBy(x => Math.Floor(x.transform.position.y*10)).ThenBy(x => Math.Floor(x.transform.position.x*10)).ToList();
        lights.ForEach(x => x.SetOff());
    }

    // Update is called once per frame
    void Update()
    {
        // flash lights when production has started
        if (flashLights)
        {
            if (Time.time - lastFlashedTime > flashDuration)
            {
                activatedLight++;
                activatedLight = activatedLight >= lights.Count ? 0 : activatedLight;
                lights.ForEach(x => x.SetOff());
                lights[activatedLight].ToggleLight();
                lastFlashedTime = Time.time;
            }
        }
    }

    public void SetActive(int amount)
    {
        lights.ForEach(x => x.SetOff());
        if (amount >= lights.Count)
        {
            flashLights = true;
        }
        else
        {
            lights.Take(amount).ToList().ForEach(x => x.SetOn());
            flashLights = false;
        }
    }
}
