using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnloadParts : MonoBehaviour
{
    private UnityAction<float> addParts;

    private Factory factory;

    public void Initialize(UnityAction<float> addParts, Factory factory) {
        this.addParts = addParts;
        this.factory = factory;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            PartPicker p = collider.GetComponent<PartPicker>();
            if (p != null) {
                var partsNeeded = factory.partsNeededCount - (int)factory.partsValue;
                float value = p.UnloadParts(partsNeeded);
                addParts.Invoke(value);
            }
        }
    }
}
