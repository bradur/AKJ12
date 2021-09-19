using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInfo : MonoBehaviour
{
    [SerializeField]
    private Text txtInfo;
    private Animator animator;

    private Transform target;

    public bool IsShown { get; private set; } = false;

    public void SetText(string newText)
    {
        txtInfo.text = newText;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void Hide()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        animator.Play("hideMinigameInfo");
        IsShown = false;
    }

    public void Show()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        animator.Play("showMinigameInfo");
        IsShown = true;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
