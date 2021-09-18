using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private float partsValue = 0;
    private UnloadParts unloadTrigger;

    // Start is called before the first frame update
    void Start()
    {
        unloadTrigger = GetComponentsInChildren<UnloadParts>()[0];
        if (unloadTrigger != null)
        {
            unloadTrigger.Initialize(AddParts);
        }
        else {
            Debug.LogError("No Pickup script in RobotPart prefab children found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddParts(float value) {
        partsValue += value;
        Debug.Log("Value " + partsValue);
    }
}
