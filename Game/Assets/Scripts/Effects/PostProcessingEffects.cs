using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem bloodPuddle;
    [SerializeField]
    public ParticleSystem oilPuddle;
    [SerializeField]
    private CinemachineImpulseSource playerDamagedImpulseSource;
    [SerializeField]
    private CinemachineImpulseSource enemyDamagedImpulseSource;
    

    public static PostProcessingEffects Main;

    private VolumeProfile profile;
    private ChromaticAberration chrAberration;
    private Vignette vignette;
    private MotionBlur motionBlur;

    private float playerDamagedTimer = -1;

    private GameObject player;

    void Awake()
    {
        Main = this;
        profile = GetComponent<Volume>()?.profile;
        if (!profile) throw new System.NullReferenceException(nameof(VolumeProfile));
        if (!profile.TryGet(out chrAberration)) throw new System.NullReferenceException(nameof(chrAberration));
        if (!profile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if (!profile.TryGet(out motionBlur)) throw new System.NullReferenceException(nameof(motionBlur));
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            motionBlur.intensity.Override(t);
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
        //ScreenShakeEffect.main.Shake(options);
        playerDamagedImpulseSource.GenerateImpulse();

        bloodPuddle.transform.position = player.transform.position;
        bloodPuddle.Play();
    }

    public void EnemyDamaged(Vector2 position)
    {
        var options = new ScreenShakeOptions();
        options.ShakeDuration = 0.2f;
        options.ShakeMagnitude = 0.05f;
        options.DampingSpeed = 0.8f;
        options.Enabled = true;
        //ScreenShakeEffect.main.Shake(options);
        ScreenFlashEffect.main.Flash();
        enemyDamagedImpulseSource.GenerateImpulse();

        oilPuddle.transform.position = position;
        oilPuddle.Play();
    }
}
