using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterConfig CharacterConfig;
    public GunConfig GunConfig;
    public Transform AimTarget;

    private float speed = 4f;
    private float vertical;
    private float horizontal;
    private Rigidbody2D body;

    private Gun gun;
    private Character character;
    private string[] GUN_TARGET_LAYERS = { "Enemy" };

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
        gun.Init(GunConfig, GUN_TARGET_LAYERS);

        character = GetComponent<Character>();
        character.Init(Faction.PLAYER, CharacterConfig);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Mouse0))
        {
            gun.Shoot();
            SoundManager.main.PlaySoundLoop(GameSoundType.GunShoot);
        }

        var targetDir = AimTarget.position - transform.position;
        var angle = Vector2.SignedAngle(transform.up, targetDir);
        transform.Rotate(Vector3.forward, angle);
    }

    void FixedUpdate()
    {
        var dir = new Vector2(horizontal, vertical).normalized;
        body.velocity = dir * speed;
    }
}
