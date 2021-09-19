using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{

    public static UIHealth main;
    void Awake() {
        main = this;
    }

    [SerializeField]
    private Text txtValue;
    [SerializeField]
    private Image imgBg;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float animationDuration = 0.5f;

    [SerializeField]
    private Gradient hpBgGradient;
    private int targetHealth;
    private int healthAnimated;
    private int originalHealth;

    private bool isAnimating = false;

    private float timer = 0;

    public void SetHealth(int value, bool setOriginal = false) {
        if (value < 0) {
            value = 0;
        }
        if (setOriginal) {
            originalHealth = value;
        }
        txtValue.text = value.ToString();
        imgBg.color = hpBgGradient.Evaluate(value / 100f);
    }
    public void SetHealthAnimated(int value) {
        if (!isAnimating) {
            timer = 0f;
            isAnimating = true;
        }
        targetHealth = value;
    }

    void Update() {
        if (isAnimating) {
            timer += Time.deltaTime;

            healthAnimated = (int)Mathf.Lerp(originalHealth, targetHealth, timer / animationDuration);
            SetHealth(healthAnimated);

            if (timer > animationDuration) {
                SetHealth(targetHealth, true);
                isAnimating = false;
            }
        }
    }

}
