using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private int bulletsPerShot = 1;

    [SerializeField]
    private Transform Muzzle;

    [SerializeField]
    private ParticleSystem HitEffect;
    private List<ParticleSystem> hitEffects = new List<ParticleSystem>();

    [SerializeField]
    private ParticleSystem BloodEffect;

    [SerializeField]
    private LightFlash MuzzleLight;

    [SerializeField]
    private Animator anim;

    private GunConfig config;
    private int targetLayerMask = 0;

    private bool readyToFire = true;

    private string[] DEFAULT_LAYERS = { "Wall" };

    public ParticleSystem MuzzleFlash;
    public TrailRenderer BulletTrail;

    private float minDamage = 0f;
    private float maxDamage = 10f;

    public void Init(GunConfig config, string[] targetLayers)
    {
        this.config = config;
        var defaultMask = LayerMask.GetMask(DEFAULT_LAYERS);
        targetLayerMask = LayerMask.GetMask(targetLayers) | defaultMask;
        minDamage = config.MinDamage;
        maxDamage = config.MaxDamage;
        SetBulletsPerShot(bulletsPerShot);
    }

    public void SetBulletsPerShot(int shots)
    {
        bulletsPerShot = shots;
        while (hitEffects.Count < bulletsPerShot)
        {
            var effect = Instantiate(HitEffect, transform);
            hitEffects.Add(effect);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if (readyToFire)
        {
            for (var i = 0; i < bulletsPerShot; i++)
            {
                var shotgunSign = i % 2 == 0 ? 1 : -1;
                var shotgunAngle = shotgunSign * ((i+1)/2) * 15f;
                var shotgunAdjustment = Quaternion.AngleAxis(shotgunAngle, Vector3.forward);
                var baseDir = shotgunAdjustment * Muzzle.up;

                var inaccuracy = Random.Range(-config.Inaccuracy, config.Inaccuracy);
                var inaccuracyAdjustment = Quaternion.AngleAxis(inaccuracy, Vector3.forward);

                var targetDirection = inaccuracyAdjustment * baseDir;
                var hit = Physics2D.Raycast(Muzzle.position, targetDirection, config.Range, targetLayerMask);
                if (hit.collider != null)
                {
                    bool effectPlayed = false;
                    var character = hit.collider.GetComponent<Character>();
                    if (character != null)
                    {
                        var damage = Random.Range(minDamage, maxDamage);
                        character.Hurt(damage);
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            PostProcessingEffects.Main.PlayerDamaged();
                            BloodEffect.transform.position = hit.point;
                            BloodEffect.Play();
                            effectPlayed = true;
                        } else {
                            SoundManager.main.PlaySound(GameSoundType.RobotHit);
                            PostProcessingEffects.Main.EnemyDamaged(hit.point);
                        }
                    }

                    if (!effectPlayed)
                    {
                        hitEffects[i].transform.position = hit.point;
                        hitEffects[i].Play();
                    }
                }

                readyToFire = false;
                Invoke("ReadyToFire", 1.0f / config.FireRate);

                var trail = Instantiate(BulletTrail, Muzzle.position, Quaternion.identity);
                trail.AddPosition(Muzzle.position);
                if (hit.collider != null)
                {
                    trail.transform.position = hit.point;
                }
                else
                {
                    trail.transform.position = Muzzle.position + targetDirection.normalized * config.Range;
                }

                MuzzleFlash.Play();
                MuzzleLight.Flash();

                if (anim != null)
                {
                    anim.Play("Base Layer.Shoot", 0, 0.25f);
                }
            }
        }
    }

    void ReadyToFire()
    {
        readyToFire = true;
    }

    public void ScaleDamage(float scale)
    {
        minDamage = minDamage * scale;
        maxDamage = maxDamage * scale;
    }
}
