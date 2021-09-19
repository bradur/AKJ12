using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{

    public static UIScore main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Text txtValue;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float animationDuration = 0.5f;

    private int targetValue;
    private int valueAnimated;
    private int originalValue;

    private bool isAnimating = false;

    private float timer = 0;
    private int score = 0;

    public int Score { get { return score; } }

    private bool animatorAnimating = false;
    private Animator animator;

    public void Animate()
    {
        if (!animatorAnimating)
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator != null)
            {
                animator.Play("UIRobotPartValuePop");
                animatorAnimating = true;
            }
            else
            {
                Debug.LogWarning("No animator found for UIScore!");
            }
        }
    }

    public void FinishedAnimating()
    {
        animatorAnimating = false;
    }

    public void AddValueAnimated(int value)
    {
        if (!isAnimating)
        {
            timer = 0f;
            isAnimating = true;
            originalValue = score;
        }
        score += value;
        targetValue = score;
    }

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;

            valueAnimated = (int)Mathf.Lerp(originalValue, targetValue, timer / animationDuration);

            txtValue.text = valueAnimated.ToString();

            if (timer > animationDuration)
            {
                txtValue.text = targetValue.ToString();
                isAnimating = false;
            }
        }
    }

}
