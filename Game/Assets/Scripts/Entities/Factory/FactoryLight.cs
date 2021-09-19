using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryLight : MonoBehaviour
{
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;
    [SerializeField]
    private Material onMaterial;
    [SerializeField]
    private Material offMaterial;
    private bool lightOn = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = offSprite;
        spriteRenderer.material = offMaterial;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleLight()
    {
        lightOn = !lightOn;
        spriteRenderer.sprite = lightOn ? onSprite : offSprite;
        spriteRenderer.material = lightOn ? onMaterial : offMaterial;
    }

    public void SetOn()
    {
        lightOn = true;
        spriteRenderer.sprite = onSprite;
        spriteRenderer.material = onMaterial;
    }

    public void SetOff()
    {
        lightOn = false;
        spriteRenderer.sprite = offSprite;
        spriteRenderer.material = offMaterial;
    }
}
