using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

[RequireComponent(typeof(Character))]
public class AIController : MonoBehaviour
{
    [SerializeField]
    private AIConfig config;

    [SerializeField]
    private bool requiresJumpStart;

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
    private State state = State.SLEEP;

    [HideInInspector]
    public Vector2 TargetLocation;

    [HideInInspector]
    public Vector2 TargetDirection;

    private Rigidbody2D rb;
    private Gun gun;
    private Destroyed destroyed;
    private float movementSpeed;
    private Factory factory;

    private Vector2 myPosition
    {
        get
        {
            return transform.position;
        }
    }

    private Vector2 moveTarget;

    void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        destroyed = GetComponent<Destroyed>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init(config);

        if (!requiresJumpStart)
        {
            Activate();
        }
        else
        {
            character.SetTargetable(false);
            rb.isKinematic = true;
        }
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
        movementSpeed = config.MoveSpeed;
    }

    private string[] targetLayers(Faction faction)
    {
        return faction == Faction.ENEMY ? ENEMY_TARGET_LAYERS : ALLY_TARGET_LAYERS;
    }

    public void Activate()
    {
        InvokeRepeating("AcquireTarget", 0.0f, 1.0f / ACQUIRE_TARGETS_PER_SECOND);
        state = State.IDLE;
        character.SetTargetable(true);
        rb.isKinematic = false;
        if (factory != null)
        {
            factory.RobotActivated();
        }
        Invoke("UpdatePathing", pathingInterval);
    }

    // Update is called once per frame
    void Update()
    {
        handleState();
        handleRoutines();
        handlePathing();
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
            if (possibleTarget.Dead || !possibleTarget.GetTargetable())
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
            SoundManager.main.PlaySound(GameSoundType.RobotDie);
            UIScore.main.AddValueAnimated(config.Score);
            state = State.DEAD;
            return;
        }
        if (state == State.SLEEP)
        {
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
        switch (state)
        {
            case State.SLEEP:
                idleRoutine.enabled = false;
                attackRoutine.enabled = false;
                break;
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
        if (state == State.SLEEP)
        {
            return;
        }

        if (Vector2.Distance(myPosition, moveTarget) > TARGET_LOCATION_MARGIN)
        {
            rb.velocity = (moveTarget - myPosition).normalized * movementSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void handleRotation()
    {
        if (state == State.SLEEP)
        {
            return;
        }

        var rotation = Vector2.SignedAngle(transform.up, TargetDirection);
        var maxRotationPerTick = Time.deltaTime * config.TurnSpeed;
        var rotationPerTick = Mathf.Clamp(rotation, -maxRotationPerTick, maxRotationPerTick);
        transform.Rotate(transform.forward, rotationPerTick);
    }

    public void Shoot()
    {
        if (state == State.SLEEP)
        {
            return;
        }

        gun.Shoot();
        SoundManager.main.PlaySoundLoop(GameSoundType.RobotShoot);
    }

    public void ScaleMoveSpeed(float scale)
    {
        movementSpeed = movementSpeed * scale;
    }

    public void ScaleStat(ScalingStat stat, float scale)
    {
        if (stat == ScalingStat.Health)
        {
            character.ScaleHealth(scale);
        }
        else if (stat == ScalingStat.Damage)
        {
            gun.ScaleDamage(scale);
        }
        else if (stat == ScalingStat.Speed)
        {
            ScaleMoveSpeed(scale);
        }
    }

    public void SetFactory(Factory factory)
    {
        this.factory = factory;
    }

    private bool hasTarget()
    {
        return target != null;
    }

    private bool isAlive()
    {
        return state != State.DEAD;
    }


    // *************************
    // PATHFINDING
    // *************************
    private NavMeshPath path;
    private int cornerIndex;

    private float pathingInterval = 0.2f;
    private float desiredRange = 0.1f;
    
    private void handlePathing()
    {
        if (HasPath())
        {
            if (IsLastCorner() && distanceToNextCorner() < desiredRange)
            {
                moveTarget = transform.position;
            }
            else
            {
                moveTarget = getNextCorner();
            }
        }
    }

    private Vector2 getNextCorner()
    {
        while (!IsLastCorner() && distanceToNextCorner() < 0.1f)
        {
            cornerIndex++;
        }
        return path.corners[cornerIndex];
    }

    private float distanceToNextCorner()
    {
        return Vector2.Distance(path.corners[cornerIndex], transform.position);
    }

    private bool IsLastCorner()
    {
        return cornerIndex >= path.corners.Length - 1;
    }

    private void UpdatePathing()
    {
        path = GetPathTo(TargetLocation);
        cornerIndex = 0;
        Invoke("UpdatePathing", pathingInterval);
    }

    private bool HasPath()
    {
        return path.corners.Length > 0;
    }

    private NavMeshPath GetPathTo(Vector2 target)
    {
        NavMeshPath newPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, newPath);
        return newPath;
    }

    private enum State
    {
        SLEEP,
        IDLE,
        ATTACK,
        DEAD
    }
}
