using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotPartInitialSpawn : MonoBehaviour
{
    [SerializeField]
    private RobotPartsConfig partsConfig;
    [SerializeField]
    private GameObject robotPartPrefab;
    [SerializeField]
    private int partsToSpawn;
    [SerializeField]
    private float spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i < partsToSpawn; i++)
        {
            spawnPart();
        }
    }

    private void spawnPart()
    {
        Vector2 pos = Vector2.zero;

        for (var i = 0; i < 100; i++)
        {
            pos = (Vector2)transform.position + Random.insideUnitCircle * spawnRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 1f, NavMesh.AllAreas))
            {
                break;
            }
        }

        GameObject robotPart = Instantiate(robotPartPrefab);
        robotPart.transform.parent = ContainerManager.main.GetRobotPartContainer().transform;
        robotPart.transform.position = pos;
        RobotPartConfig conf = partsConfig.getRandomPart();
        robotPart.GetComponent<RobotPart>().Initialize(conf);
        Destroy(gameObject);
    }
}
