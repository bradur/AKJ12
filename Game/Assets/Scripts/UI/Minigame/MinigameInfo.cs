using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInfo : MonoBehaviour
{
    [SerializeField]
    private Text txtInfo;
    private Animator animator;

    public void SetText(string newText) {
        txtInfo.text = newText;
    }

    public void Hide() {
        if (animator == null) {
            animator = GetComponent<Animator>();
        }
        animator.Play("hideMinigameInfo");
    }

    public void Show() {
        if (animator == null) {
            animator = GetComponent<Animator>();
        }
        animator.Play("showMinigameInfo");
    }
}
