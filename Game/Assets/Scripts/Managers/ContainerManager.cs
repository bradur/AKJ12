using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject robotContainer;
    [SerializeField]
    private GameObject robotPartContainer;
    
    public static ContainerManager main;
    void Awake() {
        main = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetRobotContainer() {
        return robotContainer;
    }

    public GameObject GetRobotPartContainer() {
        return robotPartContainer;
    }
}
