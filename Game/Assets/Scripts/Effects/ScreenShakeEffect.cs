using UnityEngine;
using System;

public class ScreenShakeEffect : MonoBehaviour
{

    Vector3 initialPosition;

    private bool shake = false;

    private ScreenShakeOptions options;

    private float shakeTimer = 0f;

    private bool shaking = false;

    public static ScreenShakeEffect main;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private ScreenShakeOptions defaultOptions;

    private bool allowShaking = true;

    void Awake() {
        main = this;
    }

    void Start() {
        Refresh();
    }

    public void Refresh() {
        int shakeOn = PlayerPrefs.GetInt("screenShake", 1);
        if (shakeOn == 0) {
            allowShaking = false;
        } else {
            allowShaking = true;
        }
        Debug.Log($"Shaking is: {allowShaking}");
    }

    public void Shake(ScreenShakeOptions newOptions)
    {
        if (shaking || !newOptions.Enabled) {
            return;
        }
        options = newOptions;
        initialPosition = target.localPosition;
        shake = true;
        shaking = true;
        shakeTimer = options.ShakeDuration;
    }

    public void Shake()
    {
        if (shaking) {
            return;
        }
        Shake(defaultOptions);
    }

    void Update()
    {
        if (!shake || !allowShaking)
        {
            return;
        }
        shakeTimer -= Time.deltaTime * options.DampingSpeed;
        if (shakeTimer > 0)
        {
            target.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * options.ShakeMagnitude;
        }
        else
        {
            shaking = false;
            shake = false;
            shakeTimer = 0f;
            target.localPosition = initialPosition;
        }
    }

}

[System.Serializable]
public class ScreenShakeOptions
{
    public float ShakeDuration = 0f;
    public float ShakeMagnitude = 0.7f;
    [Range(0.05f, 1f)]
    public float DampingSpeed = 1.0f;
    public bool Enabled = false;
}