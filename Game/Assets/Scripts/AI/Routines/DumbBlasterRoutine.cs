using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbBlasterRoutine : AttackRoutine
{
    private AIController controller;
    private Character target;

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
        controller.TargetLocation = transform.position;
        if (target != null)
        {
            Vector2 targetDir = target.transform.position - transform.position;
            controller.TargetDirection = targetDir;

            if (Vector2.Angle(transform.up, targetDir) < 5.0f)
            {
                controller.Shoot();
            }
        }
    }

    void OnDisable()
    {
    }

    void OnEnable()
    {
    }

    override public void SetTarget(Character target)
    {
        this.target = target;
    }

}
