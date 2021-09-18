using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoppingText : MonoBehaviour
{
    [SerializeField]
    private Text txtValue;
    [SerializeField]
    private PoppingTextOptions defaultOptions;
    private PoppingTextOptions options;
    private Animator animator;
    public void Initialize() {
        Initialize(defaultOptions);
    }
    public void Initialize(PoppingTextOptions newOptions) {
        options = newOptions;
        transform.position = options.Position;
        txtValue.text = options.Text;
        txtValue.color = options.Color;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Show");
    }

    public void FinishAnimation() {
        //Kill();
    }

    public void Kill() {
        Destroy(gameObject);
    }
}

public class PoppingTextOptions {
    public Color Color;
    public string Text;
    public Vector2 Position;
}
