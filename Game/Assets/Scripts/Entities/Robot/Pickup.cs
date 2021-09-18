using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    private float value;
    private UnityAction destroyPart;

    public void Initialize(float value, UnityAction destroyPart)
    {
        this.value = value;
        this.destroyPart = destroyPart;
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
            PartPicker picker = collider.gameObject.GetComponent<PartPicker>();
            if (picker != null)
            {
                bool collected = picker.CollectPart(value);
                if (collected)
                {
                    destroyPart.Invoke();
                }
            }
        }
    }
}