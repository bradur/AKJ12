using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour
{
    [SerializeField]
    private RobotPartsConfig partsConfig;
    [SerializeField]
    private GameObject robotPartPrefab;

    private bool isDestroyed = false;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed && !dead) {
            GameObject robotPart = Instantiate(robotPartPrefab);
            // TODO: set parent container
            robotPart.transform.parent = ContainerManager.main.GetRobotPartContainer().transform;
            robotPart.transform.position = transform.position;
            RobotPartConfig conf = partsConfig.getRandomPart();
            robotPart.GetComponent<RobotPart>().Initialize(conf);
            Destroy(gameObject);
            dead = true;
        }
    }

    public void SetDestroyed(bool isDestroyed = true) {
        this.isDestroyed = isDestroyed;
    }
}
