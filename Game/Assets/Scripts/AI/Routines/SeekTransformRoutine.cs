using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTransformRoutine : IdleRoutine
{
    private AIController controller;
    private Transform target;

    public void Init(AIController controller, Transform target)
    {
        this.controller = controller;
        this.target = target;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        controller.TargetLocation = target.position;
        controller.TargetDirection = target.position - transform.position;
    }
}
