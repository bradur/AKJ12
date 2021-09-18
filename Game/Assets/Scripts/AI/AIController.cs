using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private AIConfig config;
    private Character character;
    private Character target;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        idleRoutine = IdleRoutine.addToGameObject(this, config.IdleType);
        attackRoutine = AttackRoutine.addToGameObject(this, config.AttackType);

        InvokeRepeating("AcquireTarget", 0.0f, 1.0f / ACQUIRE_TARGETS_PER_SECOND);
        targetFaction = config.Faction == Faction.ENEMY ? Faction.PLAYER : Faction.ENEMY;
    }

    // Update is called once per frame
    void Update()
    {
        handleState();
        handleRoutines();
        handleMoving();
    }

    void FixedUpdate()
    {
        handleMoving();
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

    private bool hasTarget()
    {
        return target != null;
    }

    private enum State
    {
        IDLE,
        ATTACK
    }
}
