using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnloadParts : MonoBehaviour
{
    private int partsNeeded;
    private UnityAction<float> addParts;

    public void Initialize(UnityAction<float> addParts, int partsNeeded) {
        this.addParts = addParts;
        this.partsNeeded = partsNeeded;
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
                float value = p.UnloadParts(partsNeeded);
                addParts.Invoke(value);
            }
        }
    }
}
