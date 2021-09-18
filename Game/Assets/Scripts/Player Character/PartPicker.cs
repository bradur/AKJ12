using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPicker : MonoBehaviour
{
    private int maxParts = 10;
    private int partsCollected = 0;
    private float totalValue = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CollectPart(float value)
    {
        if (partsCollected >= maxParts)
        {
            return false;
        }
        else
        {
            partsCollected++;
            totalValue += value;
            return true;
        }
    }

    public float UnloadParts() 
    {
        float value = totalValue;
        partsCollected = 0;
        totalValue = 0;
        return value;
    }
}
