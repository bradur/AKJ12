using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private int partsNeededCount;
    [SerializeField]
    private GameObject robotPrefab;
    [SerializeField]
    private Transform spawnPoint;

    private float partsValue = 0;

    private UnloadParts unloadTrigger;
    private FactoryLightArray lightArray;
    private bool assemblyOngoing = false;
    private float assemblyStartedTime = 0;
    private float assemblyDuration = 5f;

    [SerializeField]
    private MinigameConfig minigameConfig;

    // Start is called before the first frame update
    void Start()
    {
        if (minigameConfig == null) {
            Debug.LogError("Factory needs a MinigameConfig!");
        }
        unloadTrigger = GetComponentsInChildren<UnloadParts>()[0];
        if (unloadTrigger != null)
        {
            unloadTrigger.Initialize(AddParts);
        }
        else
        {
            Debug.LogError("No Pickup script in RobotPart prefab children found!");
        }

        lightArray = GetComponentsInChildren<FactoryLightArray>()[0];
        if (lightArray == null)
        {
            Debug.LogError("No Pickup script in RobotPart prefab children found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (assemblyOngoing && Time.time - assemblyStartedTime > assemblyDuration)
        {
            assemblyOngoing = false;
            lightArray.SetActive(Mathf.FloorToInt(partsValue));
            unloadTrigger.gameObject.SetActive(true);

            GameObject allyRobot = Instantiate(robotPrefab);
            allyRobot.transform.position = spawnPoint.position;
            allyRobot.transform.parent = ContainerManager.main.GetRobotContainer().transform;
            SetupMinigame(allyRobot);
        }
    }

    private void SetupMinigame(GameObject allyRobot) {
        MinigameTrigger minigameTrigger = allyRobot.GetComponentInChildren<MinigameTrigger>();
        minigameConfig.Options.PositiveAction = delegate(int value, Vector2 pos) {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = value == 100 ? Color.green : Color.yellow;
            textOptions.Text = value == 100 ? $"Great!" : "OK!";
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
            AIController aiController = allyRobot.GetComponentInChildren<AIController>();
            aiController.Activate();
        };
        minigameConfig.Options.NegativeAction = delegate(int value, Vector2 pos) {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = Color.red;
            textOptions.Text = $"Boom you died!";
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
        };
        minigameTrigger.Initialize(minigameConfig.Options);
    }

    public void AddParts(float value)
    {
        partsValue += value;
        lightArray.SetActive(Mathf.FloorToInt(partsValue));

        if (partsValue >= partsNeededCount)
        {
            partsValue = 0; // If player can carry more than 1, should this be partsValue - partsNeedeCount?

            unloadTrigger.gameObject.SetActive(false);
            assemblyStartedTime = Time.time;
            assemblyOngoing = true;
        }
    }
}
