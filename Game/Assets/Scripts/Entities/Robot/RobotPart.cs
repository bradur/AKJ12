using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPart : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private SpriteRenderer effectSprite;
    private Rigidbody2D body;
    private float value;
    private Pickup pickupTrigger;

    public void Initialize(RobotPartConfig conf)
    {
        sprite.sprite = conf.sprite;
        effectSprite.sprite = conf.sprite;
        value = conf.value;

        pickupTrigger = GetComponentsInChildren<Pickup>()[0];
        if (pickupTrigger != null)
        {
            pickupTrigger.Initialize(value, Kill);
        }
        else {
            Debug.LogError("No Pickup script in RobotPart prefab children found!");
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        Vector2 randomForce = Random.insideUnitCircle.normalized;
        body.AddForce(randomForce * 2f, ForceMode2D.Impulse);
        transform.Rotate(0, 0, Random.Range(0, 360f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
