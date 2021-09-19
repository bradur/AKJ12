using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PartPicker : MonoBehaviour
{
    private int maxParts = 10;
    private int partsCollected = 0;
    private float totalValue = 0;

    private List<MinigameInfo> factoryInfos = new List<MinigameInfo>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CollectPart(float value)
    {
        ShowFactoryInfo();
        if (partsCollected >= maxParts)
        {
            return false;
        }
        else
        {
            partsCollected++;
            totalValue += value;
            return true;
        }

    }

    private void ShowFactoryInfo()
    {
        if (factoryInfos.Count == 0)
        {
            foreach (GameObject factory in GameObject.FindGameObjectsWithTag("Factory"))
            {
                Transform target = factory.GetComponentInChildren<UnloadParts>().transform;
                MinigameInfo info = WorldUI.main.GetMinigameInfo("Bring parts here!", target.position, target);
                factoryInfos.Add(info);
            };
        }
        Debug.Log($"Show infos: {factoryInfos.Count}");
        foreach (MinigameInfo info in factoryInfos)
        {
            if (!info.IsShown)
            {
                info.Show();
            }
        }
    }

    public void HideFactoryInfo()
    {
        foreach (MinigameInfo info in factoryInfos)
        {
            if (info.IsShown)
            {
                info.Hide();
            }
        }
    }

    public float UnloadParts()
    {
        float value = totalValue;
        partsCollected = 0;
        totalValue = 0;
        HideFactoryInfo();
        return value;
    }
}
