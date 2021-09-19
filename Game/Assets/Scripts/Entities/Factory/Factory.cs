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
    private bool previousRobotInactive = false;
    private float assemblyStartedTime = 0;
    private float assemblyDuration = 3f;

    [SerializeField]
    private MinigameConfig minigameConfig;
    [SerializeField]
    private StatScalingConfig positiveScaling;
    [SerializeField]
    private StatScalingConfig negativeScaling;

    // Start is called before the first frame update
    void Start()
    {
        if (minigameConfig == null)
        {
            Debug.LogError("Factory needs a MinigameConfig!");
        }
        unloadTrigger = GetComponentsInChildren<UnloadParts>()[0];
        if (unloadTrigger != null)
        {
            unloadTrigger.Initialize(AddParts, partsNeededCount);
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
            previousRobotInactive = true;
            lightArray.SetActive(Mathf.FloorToInt(partsValue));

            SoundManager.main.PlaySound(GameSoundType.FactoryBuild);
            GameObject allyRobot = Instantiate(robotPrefab);
            allyRobot.transform.position = spawnPoint.position;
            allyRobot.transform.parent = ContainerManager.main.GetRobotContainer().transform;
            AIController aiController = allyRobot.GetComponentInChildren<AIController>();
            aiController.SetFactory(this);
            SetupMinigame(allyRobot);
            ShowFactoryInfo();
        }
    }

    private MinigameInfo minigameInfo;
    private void ShowFactoryInfo()
    {
        Transform target = transform.Find("SpawnPointInfo").transform;
        minigameInfo = WorldUI.main.GetMinigameInfo("Jumpstart the robot!", target.position, target);

        if (!minigameInfo.IsShown)
        {
            minigameInfo.Show();
        }
    }
    private UnityEngine.Events.UnityAction<int, Vector2> PositiveMinigameAction(AIController aiController)
    {
        return delegate (int value, Vector2 pos)
        {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            string text = "";

            if (value == 100)
            {
                text += "Awesome!";
                textOptions.Color = Color.green;
                ScalingStat stat1 = positiveScaling.getRandomStat();
                aiController.ScaleStat(stat1, positiveScaling.getStatScale(stat1));
                ScalingStat stat2 = positiveScaling.getRandomStat();
                aiController.ScaleStat(stat2, positiveScaling.getStatScale(stat2));
                text += $"\n+{stat1.ToString()}";
                text += $"\n+{stat2.ToString()}";
            }
            else if (value == 80)
            {
                text += "Great!";
                textOptions.Color = new Color(0.2f, 1, 0.1f, 1);
                ScalingStat stat1 = positiveScaling.getRandomStat();
                aiController.ScaleStat(stat1, positiveScaling.getStatScale(stat1));
                text += $"\n+{stat1.ToString()}";
            }
            else
            {
                textOptions.Color = Color.yellow;
                text += "OK!";
            }
            text += $"\n+{value} points";
            UIScore.main.AddValueAnimated(value);
            textOptions.Text = text;
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
            aiController.Activate();
        };
    }

    private UnityEngine.Events.UnityAction<int, Vector2> NegativeMinigameAction(AIController aiController, GameObject allyRobot)
    {
        return delegate (int value, Vector2 pos)
        {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = Color.red;
            ScalingStat stat1 = negativeScaling.getRandomStat();
            aiController.ScaleStat(stat1, negativeScaling.getStatScale(stat1));
            textOptions.Text = $"Bad kickstart!\n-{stat1.ToString()}\n+{value} points";
            UIScore.main.AddValueAnimated(value);
            textOptions.Position = pos;
            aiController.Activate();

            WorldUI.main.ShowPoppingText(textOptions);
        };
    }

    private void SetupMinigame(GameObject allyRobot)
    {
        MinigameTrigger minigameTrigger = allyRobot.GetComponentInChildren<MinigameTrigger>();
        AIController aiController = allyRobot.GetComponentInChildren<AIController>();
        minigameConfig.Options.PositiveAction = PositiveMinigameAction(aiController);
        minigameConfig.Options.NegativeAction = NegativeMinigameAction(aiController, allyRobot);
        minigameConfig.Options.RestartAction = delegate (int value, Vector2 pos)
        {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = Color.white;
            textOptions.Text = $"Not quite! Try again.";
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
        };
        minigameTrigger.Initialize(minigameConfig.Options);
    }

    public void AddParts(float value)
    {
        // can't use factory until robot is kickstarted
        if (previousRobotInactive)
        {
            Debug.Log("Can't use");
            return;
        }
        partsValue += value;
        lightArray.SetActive(Mathf.FloorToInt(partsValue));

        if (partsValue >= partsNeededCount)
        {
            partsValue = 0;

            unloadTrigger.gameObject.SetActive(false);
            assemblyStartedTime = Time.time;
            assemblyOngoing = true;
        }
    }

    public void RobotActivated()
    {
        previousRobotInactive = false;
        if (minigameInfo != null)
        {
            minigameInfo.Hide();
        }
        unloadTrigger.gameObject.SetActive(true);
    }
}
