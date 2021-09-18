using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minigame : MonoBehaviour
{
    [SerializeField]
    private Transform areaContainer;

    [SerializeField]
    private MinigameOptions defaultOptions;
    private MinigameOptions options;

    private List<MinigameArea> areas = new List<MinigameArea>();

    private UnityAction<int> PositiveCallback;
    private UnityAction<int> NegativeCallback;

    void Start() {
        Initialize();
    }

    public void Initialize()
    {
        Initialize(
            delegate (int value)
            {
                Debug.Log($"Performed <color=green>positive</color> reaction with value {value}.");
            },
            delegate (int value)
            {
                Debug.Log($"Performed <color=red>negative</color> reaction with value {value}.");
            }
        );
    }

    public void Initialize(
        UnityAction<int> PositiveCallback,
        UnityAction<int> NegativeCallback
    )
    {
        Initialize(defaultOptions, PositiveCallback, NegativeCallback);
    }

    public void Initialize(
        MinigameOptions newOptions,
        UnityAction<int> PositiveCallback,
        UnityAction<int> NegativeCallback
    )
    {
        options = newOptions;
        this.PositiveCallback = PositiveCallback;
        this.NegativeCallback = NegativeCallback;
        SetUpAreas();
    }

    public bool MakeSelection(int percentage)
    {
        Debug.Log($"Minigame selection at: {percentage}");
        MinigameArea area = GetArea(percentage);
        if (area == null) {
            Debug.Log("Indicator hit no area.");
            return false;
        }
        Debug.Log($"Selection stopped at area {area}.");

        if (area.Options.Result == SelectionResult.Positive)
        {
            PositiveResult(area.Options.ResultValue);
        }
        else if (area.Options.Result == SelectionResult.Negative)
        {
            NegativeResult(area.Options.ResultValue);
        }
        return true;
    }

    public MinigameArea GetArea(int selectionPercentage)
    {
        int currentPercentageMin = 0;
        int currentPercentageMax = 100;
        MinigameAreaOptions previousArea = null;
        foreach (MinigameArea area in areas)
        {
            if (previousArea == null)
            {
                currentPercentageMin = 0;
                previousArea = area.Options;
            }
            else
            {
                currentPercentageMin = currentPercentageMax;
            }
            currentPercentageMax = currentPercentageMin + area.PercentageSize;
            Debug.Log($"selectionPercentage: {selectionPercentage}. Area ps: {currentPercentageMin} / {currentPercentageMax}");
            if (selectionPercentage >= currentPercentageMin && selectionPercentage <= currentPercentageMax)
            {
                Debug.Log($"selectionPercentage: {selectionPercentage} is within {currentPercentageMin} and {currentPercentageMax}");
                return area;
            }
        }
        return null;
    }

    private void SetUpAreas()
    {
        foreach (MinigameAreaOptions areaOptions in options.Areas)
        {
            MinigameArea area = Prefabs.Get<MinigameArea>();
            area.transform.SetParent(areaContainer, false);
            area.Initialize(areaOptions);
            areas.Add(area);
        }
    }

    public void PositiveResult(int multiplier)
    {
        PositiveCallback(multiplier);
    }
    public void NegativeResult(int multiplier)
    {
        NegativeCallback(multiplier);
    }
}

[System.Serializable]
public class MinigameOptions
{
    public List<MinigameAreaOptions> Areas;
}
