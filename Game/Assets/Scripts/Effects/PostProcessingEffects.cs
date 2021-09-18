using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    public static PostProcessingEffects Main;

    public VolumeProfile profile;
    public ChromaticAberration chrAberration;
    public Vignette vignette;

    private float playerDamagedTimer = -1;

    void Awake()
    {
        Main = this;
        profile = GetComponent<Volume>()?.profile;
        if (!profile) throw new System.NullReferenceException(nameof(VolumeProfile));
        if (!profile.TryGet(out chrAberration)) throw new System.NullReferenceException(nameof(chrAberration));
        if (!profile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDamagedTimer > 0)
        {
            var t = (playerDamagedTimer + 0.5f) - Time.time;
            t = Mathf.Clamp(t * 2.0f, 0.0f, 1.0f);
            chrAberration.intensity.Override(t * 2);

            vignette.intensity.Override(t / 2.0f);
        }
    }

    public void PlayerDamaged()
    {
        playerDamagedTimer = Time.time;

        var options = new ScreenShakeOptions();
        options.ShakeDuration = 0.4f;
        options.ShakeMagnitude = 0.1f;
        options.DampingSpeed = 0.8f;
        options.Enabled = true;
        ScreenShakeEffect.main.Shake(options);
        
    }

    public void EnemyDamaged()
    {
        var options = new ScreenShakeOptions();
        options.ShakeDuration = 0.2f;
        options.ShakeMagnitude = 0.05f;
        options.DampingSpeed = 0.8f;
        options.Enabled = true;
        ScreenShakeEffect.main.Shake(options);
        ScreenFlashEffect.main.Flash();
    }
}
