using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoboParts : MonoBehaviour
{

    public static UIRoboParts main;
    void Awake() {
        main = this;
    }

    [SerializeField]
    private Text txtValue;

    private Animator animator;
    private bool animating = false;

    public void SetValue(int value, bool animate = false) {
        txtValue.text = value.ToString();
        if (animate && !animating) {
            if (animator == null) {
                animator = GetComponent<Animator>();
            }
            if (animator != null) {
                animator.Play("UIRobotPartValuePop");
                animating = true;
            } else {
                Debug.LogWarning("No animator found for UIRobotParts!");
            }
        }
    }

    public void FinishedAnimating() {
        animating = false;
    }


}
