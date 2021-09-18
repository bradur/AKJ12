using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Minigame : MonoBehaviour
{
    [SerializeField]
    private Transform areaContainer;

    [SerializeField]
    private MinigameOptions defaultOptions;
    [SerializeField]
    private MinigameSelectionIndicator indicator;
    private MinigameOptions options;

    private List<MinigameArea> areas = new List<MinigameArea>();

    private UnityAction<int, Vector2> PositiveCallback;
    private UnityAction<int, Vector2> NegativeCallback;

    private RectTransform rectTransform;

    public bool Finished {get; private set;}

    private Animator animator;

    void Start() {
    }

    public void SetPosition(Vector2 minigamePosition) {
        transform.position = minigamePosition;
    }

    public void Initialize(
        UnityAction<int, Vector2> PositiveCallback,
        UnityAction<int, Vector2> NegativeCallback
    )
    {
        Initialize(defaultOptions, PositiveCallback, NegativeCallback);
    }

    public void Initialize(
        MinigameOptions newOptions,
        UnityAction<int, Vector2> PositiveCallback,
        UnityAction<int, Vector2> NegativeCallback
    )
    {
        animator = GetComponent<Animator>();
        Finished = false;
        options = newOptions;
        this.PositiveCallback = PositiveCallback;
        this.NegativeCallback = NegativeCallback;
        SetUpAreas();
        indicator.Initialize(options.IndicatorOptions);
    }

    public bool MakeSelection(int percentage, Vector2 selectionPosition)
    {
        MinigameArea area = GetArea(percentage);
        if (area == null) {
            return false;
        }

        if (area.Options.Result == SelectionResult.Positive)
        {
            PositiveResult(area.Options.ResultValue, selectionPosition);
        }
        else if (area.Options.Result == SelectionResult.Negative)
        {
            NegativeResult(area.Options.ResultValue, selectionPosition);
        } else if (area.Options.Result == SelectionResult.None) {
            return false;
        }
        Finished = true;
        indicator.Disable();
        return true;
    }

    public MinigameArea GetArea(int selectionPercentage)
    {
        int currentPercentageMin = 0;
        int currentPercentageMax = 100;
        MinigameAreaOptions previousArea = null;
        foreach (MinigameArea area in areas.Reverse<MinigameArea>())
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
            if (selectionPercentage >= currentPercentageMin && selectionPercentage <= currentPercentageMax)
            {
                return area;
            }
        }
        return null;
    }


    public void Show() {
        animator.SetTrigger("Show");
    }

    public void Hide() {
        animator.SetTrigger("Hide");
        indicator.Disable();
    }

    public void ShowFinished() {
        indicator.Enable();
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

    public void PositiveResult(int value, Vector2 resultPosition)
    {
        if (PositiveCallback != null) {
            PositiveCallback(value, resultPosition);
        } else {
            Debug.Log($"Minigame {name} doesn't have PositiveCallback!");
        }
    }
    public void NegativeResult(int value, Vector2 resultPosition)
    {
        if (NegativeCallback != null) {
            NegativeCallback(value, resultPosition);
        } else {
            Debug.Log($"Minigame {name} doesn't have NegativeCallback!");
        }
    }
}

[System.Serializable]
public class MinigameOptions
{
    public IndicatorOptions IndicatorOptions;
    public List<MinigameAreaOptions> Areas;
}
