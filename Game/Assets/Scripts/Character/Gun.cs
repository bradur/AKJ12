using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform Muzzle;

    [SerializeField]
    private ParticleSystem HitEffect;

    [SerializeField]
    private Animator anim;

    private GunConfig config;
    private int targetLayerMask = 0;

    private bool readyToFire = true;

    private string[] DEFAULT_LAYERS = { "Wall" };

    public ParticleSystem MuzzleFlash;
    public TrailRenderer BulletTrail;

    public void Init(GunConfig config, string[] targetLayers)
    {
        this.config = config;
        var defaultMask = LayerMask.GetMask(DEFAULT_LAYERS);
        targetLayerMask = LayerMask.GetMask(targetLayers) | defaultMask;
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
            var inaccuracy = Random.Range(-config.Inaccuracy, config.Inaccuracy);
            var inaccuracyAdjustment = Quaternion.AngleAxis(inaccuracy, Vector3.forward);
            
            var targetDirection = inaccuracyAdjustment * Muzzle.up;
            var hit = Physics2D.Raycast(Muzzle.position, targetDirection, config.Range, targetLayerMask);
            if (hit.collider != null)
            {
                var character = hit.collider.GetComponent<Character>();
                if (character != null)
                {
                    var damage = Random.Range(config.MinDamage, config.MaxDamage);
                    character.Hurt(damage);
                }
                HitEffect.transform.position = hit.point;
                HitEffect.Play();
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

            if (anim != null)
            {
                anim.Play("Base Layer.Shoot", 0, 0.25f);
            }
        }
    }

    void ReadyToFire()
    {
        readyToFire = true;
    }
}
