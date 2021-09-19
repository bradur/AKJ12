using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafingBlasterRoutine : AttackRoutine
{
    private AIController controller;
    private Character target;

    private float STRAFE_MIN_TIME = 1;
    private float STRAFE_MAX_TIME = 2;
    private float STRAFE_MIN_ANGLE = 85;
    private float STRAFE_MAX_ANGLE = 120;

    private float strafeAngle = 0.0f;

    public void Init(AIController controller)
    {
        this.controller = controller;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > 8.0f)
            {
                controller.TargetLocation = target.transform.position;
            }
            else
            {
                Vector2 targetDir = target.transform.position - transform.position;
                controller.TargetDirection = targetDir;

                if (Vector2.Angle(transform.up, targetDir) < 5.0f)
                {
                    controller.Shoot();
                }

                var strafeRotation = Quaternion.AngleAxis(strafeAngle, Vector3.forward);
                var moveDir = strafeRotation * targetDir.normalized;
                controller.TargetLocation = transform.position + moveDir * 2.5f;
            }
        }
        else
        {
            controller.TargetLocation = transform.position;
        }
    }

    void OnDisable()
    {
        CancelInvoke("PickNewDirection");
    }

    void OnEnable()
    {
        PickNewDirection();
    }

    private void PickNewDirection()
    {
        var leftOrRight = Random.Range(0, 100) < 50 ? 1 : -1;
        strafeAngle = leftOrRight * Random.Range(STRAFE_MIN_ANGLE, STRAFE_MAX_ANGLE);

        var strafeTime = Random.Range(STRAFE_MIN_TIME, STRAFE_MAX_TIME);
        Invoke("PickNewDirection", strafeTime);
    }

    override public void SetTarget(Character target)
    {
        this.target = target;
    }

}
