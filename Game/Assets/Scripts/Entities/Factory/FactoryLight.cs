using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryLight : MonoBehaviour
{
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;
    private bool lightOn = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = offSprite;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleLight()
    {
        lightOn = !lightOn;
        spriteRenderer.sprite = lightOn ? onSprite : offSprite;
    }

    public void SetOn()
    {
        lightOn = true;
        spriteRenderer.sprite = onSprite;
    }

    public void SetOff()
    {
        lightOn = false;
        spriteRenderer.sprite = offSprite;
    }
}
