using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbBlasterRoutine : AttackRoutine
{

    private AIController controller;

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
    }

    void OnDisable()
    {
    }

    void OnEnable()
    {
    }

}
