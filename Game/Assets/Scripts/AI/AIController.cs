using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Character))]
public class AIController : MonoBehaviour
{
    [SerializeField]
    private AIConfig config;
    private Character character;
    private Character target;

    private string ALLY_LAYER = "Ally";
    private string ENEMY_LAYER = "Enemy";
    private string[] ALLY_TARGET_LAYERS = { "Enemy" };
    private string[] ENEMY_TARGET_LAYERS = { "Ally" };

    private float ACQUIRE_TARGETS_PER_SECOND = 5;
    private float TARGET_LOCATION_MARGIN = 0.1f;
    private float TARGET_DIRECTION_MARGIN = 0.1f;

    private IdleRoutine idleRoutine;
    private AttackRoutine attackRoutine;
    private Faction targetFaction;
    private State state = State.IDLE;

    [HideInInspector]
    public Vector2 TargetLocation;

    [HideInInspector]
    public Vector2 TargetDirection;

    private Rigidbody2D rb;
    private Gun gun;
    private Destroyed destroyed;

    private Vector2 myPosition
    {
        get
        {
            return transform.position;
        }
    }

    void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        destroyed = GetComponent<Destroyed>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AcquireTarget", 0.0f, 1.0f / ACQUIRE_TARGETS_PER_SECOND);
        Init(config);
    }

    public void Init(AIConfig config)
    {
        this.config = config;
        character.Init(config.Faction, config.CharacterConfig);
        idleRoutine = IdleRoutine.addToGameObject(this, config.IdleType);
        attackRoutine = AttackRoutine.addToGameObject(this, config.AttackType);
        targetFaction = config.Faction == Faction.ENEMY ? Faction.PLAYER : Faction.ENEMY;

        gun = GetComponentInChildren<Gun>();
        gun.Init(config.GunConfig, targetLayers(config.Faction));

        var layerName = config.Faction == Faction.ENEMY ? ENEMY_LAYER : ALLY_LAYER;
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    private string[] targetLayers(Faction faction)
    {
        return faction == Faction.ENEMY ? ENEMY_TARGET_LAYERS : ALLY_TARGET_LAYERS;
    }

    // Update is called once per frame
    void Update()
    {
        handleState();
        handleRoutines();
        if (isAlive())
        {
            handleRotation();
        }
        else
        {
            destroyed.SetDestroyed();
        }
    }

    void FixedUpdate()
    {
        if (isAlive())
        {
            handleMoving();
        }
    }

    void Die()
    {
        CancelInvoke("AcquireTarget");
    }

    void AcquireTarget()
    {
        var enemies = CharacterManager.Main.charactersBy(targetFaction);
        Character nearestTarget = null;
        float nearestDist = float.MaxValue;
        foreach (var possibleTarget in enemies)
        {
            if (possibleTarget.Dead)
            {
                continue;
            }

            var distance = Vector2.Distance(myPosition, possibleTarget.transform.position);
            if (distance > config.AggroRange)
            {
                continue;
            }

            if (nearestTarget == null || distance < nearestDist)
            {
                nearestTarget = possibleTarget;
                nearestDist = distance;
            }
        }
        target = nearestTarget;
    }

    private void handleState()
    {
        if (character.Dead)
        {
            state = State.DEAD;
            return;
        }

        if (hasTarget())
        {
            if (target.Dead)
            {
                AcquireTarget();
            }
        }

        if (target == null)
        {
            state = State.IDLE;
        }
        else
        {
            state = State.ATTACK;
        }
    }

    private void handleRoutines()
    {
        switch(state)
        {
            case State.IDLE:
                idleRoutine.enabled = true;
                attackRoutine.enabled = false;
                break;
            case State.ATTACK:
                idleRoutine.enabled = false;
                attackRoutine.enabled = true;
                attackRoutine.SetTarget(target);
                break;
            case State.DEAD:
                idleRoutine.enabled = false;
                attackRoutine.enabled = false;
                break;
            default:
                Debug.LogError("Unexpected state " + state);
                break;
        }
    }

    private void handleMoving()
    {
        if (Vector2.Distance(myPosition, TargetLocation) > TARGET_LOCATION_MARGIN)
        {
            rb.velocity = (TargetLocation - myPosition).normalized * config.MoveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void handleRotation()
    {
        var rotation = Vector2.SignedAngle(transform.up, TargetDirection);
        var maxRotationPerTick = Time.deltaTime * config.TurnSpeed;
        var rotationPerTick = Mathf.Clamp(rotation, -maxRotationPerTick, maxRotationPerTick);
        transform.Rotate(transform.forward, rotationPerTick);
    }

    public void Shoot()
    {
        gun.Shoot();
    }

    private bool hasTarget()
    {
        return target != null;
    }

    private bool isAlive()
    {
        return state != State.DEAD;
    }

    private enum State
    {
        IDLE,
        ATTACK,
        DEAD
    }
}
