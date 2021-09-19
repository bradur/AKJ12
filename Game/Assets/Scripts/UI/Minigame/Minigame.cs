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
    private MinigameSelectionIndicator indicator;
    private MinigameOptions options;

    private List<MinigameArea> areas = new List<MinigameArea>();

    private UnityAction<int, Vector2> PositiveCallback;
    private UnityAction<int, Vector2> NegativeCallback;
    private UnityAction<int, Vector2> RestartCallback;

    public bool IsShown { get; private set; }

    private Animator animator;

    public MinigameTrigger Trigger { get; private set; }

    [SerializeField]
    private MinigameInfo minigameInfo;

    public void Initialize(
        MinigameTrigger trigger,
        MinigameOptions newOptions,
        UnityAction<int, Vector2> PositiveCallback,
        UnityAction<int, Vector2> NegativeCallback,
        UnityAction<int, Vector2> RestartCallback
    )
    {
        Trigger = trigger;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        Reset();
        options = newOptions;
        this.PositiveCallback = PositiveCallback;
        this.NegativeCallback = NegativeCallback;
        this.RestartCallback = RestartCallback;
        SetUpAreas();
        indicator.Initialize(options.IndicatorOptions);
    }

    public void ResetInfo() {
        minigameInfo.SetText($"Press {Configs.main.InputConfig.GetKeyByAction(GameAction.MiniGameIndicatorStart)} to start.");
    }

    public void Reset()
    {
        ResetInfo();
        animator.Play("minigameIdle");
        areas = new List<MinigameArea>();
        foreach (Transform child in areaContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void Started() {
        minigameInfo.SetText($"Press {Configs.main.InputConfig.GetKeyByAction(GameAction.MiniGameIndicatorStop)} at the correct position!");
    }

    public void SetPosition(Vector2 minigamePosition)
    {
        transform.position = minigamePosition;
    }

    public bool MakeSelection(int percentage, Vector2 selectionPosition)
    {
        MinigameArea area = GetArea(percentage);
        if (area == null)
        {
            return false;
        }

        if (area.Options.Result == SelectionResult.Positive)
        {
            PositiveResult(area.Options.ResultValue, selectionPosition);
        }
        else if (area.Options.Result == SelectionResult.Negative)
        {
            NegativeResult(area.Options.ResultValue, selectionPosition);
        }
        else if (area.Options.Result == SelectionResult.Restart)
        {
            RestartResult(area.Options.ResultValue, selectionPosition);
            ResetInfo();
            return false;
        }
        else if (area.Options.Result == SelectionResult.None)
        {
            ResetInfo();
            return false;
        }
        minigameInfo.Hide();
        Trigger.Finish();
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


    public void Show()
    {
        //animator.SetTrigger("Show");
        animator.Play("showMinigame");
    }

    public void Hide()
    {
        //animator.SetTrigger("Hide");
        animator.Play("hideMinigame");
        indicator.Disable();
        minigameInfo.Hide();
    }

    public void ShowFinished()
    {
        minigameInfo.Show();
        indicator.Enable();
        IsShown = true;
    }

    public void HideFinished()
    {
        indicator.Enable();
        IsShown = false;
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
        if (PositiveCallback != null)
        {
            SoundManager.main.PlaySound(GameSoundType.MinigameSuccess);
            PositiveCallback(value, resultPosition);
        }
        else
        {
            Debug.Log($"Minigame {name} doesn't have PositiveCallback!");
        }
    }
    public void NegativeResult(int value, Vector2 resultPosition)
    {
        if (NegativeCallback != null)
        {
            SoundManager.main.PlaySound(GameSoundType.MinigameFail);
            NegativeCallback(value, resultPosition);
        }
        else
        {
            Debug.Log($"Minigame {name} doesn't have NegativeCallback!");
        }
    }
    public void RestartResult(int value, Vector2 resultPosition)
    {
        if (RestartCallback != null)
        {
            RestartCallback(value, resultPosition);
        }
        else
        {
            Debug.Log($"Minigame {name} doesn't have RestartCallback!");
        }
    }
}

[System.Serializable]
public class MinigameOptions
{
    public IndicatorOptions IndicatorOptions;
    public List<MinigameAreaOptions> Areas;
}
