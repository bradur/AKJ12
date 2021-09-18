using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoutine : IdleRoutine
{
    private AIController controller;
    private PatrolConfig config;
    private Vector2 targetPosition;

    public void Init(AIController controller, PatrolConfig config)
    {
        this.controller = controller;
        this.config = config;
    }

    void OnDisable()
    {
        CancelInvoke("PickNewTargetLocation");
    }

    void OnEnable()
    {
        var idleTime = Random.Range(config.MinIdleTime, config.MaxIdleTime);
        Invoke("PickNewTargetLocation", idleTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            var idleTime = Random.Range(config.MinIdleTime, config.MaxIdleTime);
            Invoke("PickNewTargetLocation", idleTime);
        }
    }

    void PickNewTargetLocation()
    {
        targetPosition = (Vector2)config.PatrolAreaCenter.position + Random.insideUnitCircle * config.PatrolRadius;
        controller.TargetLocation = targetPosition;
    }

    public struct PatrolConfig
    {
        public Transform PatrolAreaCenter;
        public float PatrolRadius;
        public float MinIdleTime;
        public float MaxIdleTime;
    }
}
