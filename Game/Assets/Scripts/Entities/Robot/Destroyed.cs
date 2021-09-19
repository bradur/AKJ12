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
    private static int killedBots = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed && !dead)
        {
            // TODO: set parent container
            killedBots++;
            if (killedBots == 9)
            {
                GameObject robotPart = Instantiate(robotPartPrefab);
                robotPart.transform.parent = ContainerManager.main.GetRobotPartContainer().transform;
                robotPart.transform.position = transform.position;
                RobotPartConfig conf = partsConfig.getHealthPack();
                robotPart.GetComponent<RobotPart>().Initialize(conf);
                killedBots = 0;
            }
            float drop = Random.Range(0f, 1f);
            if (drop < partsConfig.chanceToDrop)
            {
                GameObject robotPart = Instantiate(robotPartPrefab);
                robotPart.transform.parent = ContainerManager.main.GetRobotPartContainer().transform;
                robotPart.transform.position = transform.position;
                RobotPartConfig conf = partsConfig.getRandomPart();
                robotPart.GetComponent<RobotPart>().Initialize(conf);
            }
            Destroy(gameObject);
            dead = true;
        }
    }

    public void SetDestroyed(bool isDestroyed = true)
    {
        this.isDestroyed = isDestroyed;
    }
}
