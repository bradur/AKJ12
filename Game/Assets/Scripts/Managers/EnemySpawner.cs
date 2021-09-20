using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float Radius = 10.0f;
    public float InitialDelay = 5.0f;
    public float InitialEnemiesPerSecond = 0.2f;
    public float EnemiesPerSecondRampUp = 0.01f;

    private Transform enemyParent;

    public GameObject EnemyPrefab;

    private float started;

    // Start is called before the first frame update
    void Start()
    {
        started = Time.time;
        Invoke("Spawn", InitialDelay);
        enemyParent = ContainerManager.main.GetRobotContainer().transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Spawn()
    {
        var beenRunning = Time.time - started;
        var position = Random.insideUnitCircle.normalized * Radius;
        var enemy = Instantiate(EnemyPrefab, enemyParent);
        enemy.transform.position = position;

        var timeUntilNext = 1 / (InitialEnemiesPerSecond + (beenRunning - InitialDelay) * EnemiesPerSecondRampUp);
        Invoke("Spawn", timeUntilNext);
    }
}
