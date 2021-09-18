using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSelectionIndicator : MonoBehaviour
{
    [SerializeField]
    private IndicatorOptions defaultOptions;
    [SerializeField]
    private Minigame minigame;
    private IndicatorOptions options;

    private InputConfig inputConfig;

    private bool finished = false;
    private bool canBeStarted = true;

    private Vector2 parentSize;

    private IndicatorDirection currentDirection;
    private RectTransform rectTransform;


    private void Start() {
        parentSize = transform.parent.GetComponent<RectTransform>().sizeDelta;
        rectTransform = GetComponent<RectTransform>();
        inputConfig = Configs.main.InputConfig;
        Initialize();
    }

    public void Initialize()
    {
        Initialize(defaultOptions);
    }

    public void Initialize(IndicatorOptions newOptions)
    {
        options = newOptions;
        if (minigame == null) {
            throw new System.Exception("MinigameSelectionIndicator needs minigame!");
        }
        Reset();
    }

    public void Enable() {
        canBeStarted = true;
    }

    public void Disable() {
        canBeStarted = false;
        Reset();
    }

    private void Reset() {
        started = false;
        currentDirection = options.Direction;
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(
            anchoredPos.x,
            parentSize.y * (options.StartPercentage / 100f)
        );
    }

    private bool warned = false;
    private void Update()
    {
        if (finished || !canBeStarted) {
            return;
        }
        if (inputConfig == null) {
            if (!warned) {
                Debug.Log("SelectionIndicator needs InputConfig!");
                warned = true;
            }
            return;
        }
        if (started)
        {
            HandleWhenStarted();
        }
        else
        {
            HandleWhenStopped();
        }
    }


    private int GetDirectionFactor() {
        return currentDirection == IndicatorDirection.Up ? 1 : -1;
    }

    private void HandleMoving() {
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        float newY = anchoredPos.y + GetDirectionFactor() * (options.Speed * Time.deltaTime);
        rectTransform.anchoredPosition = new Vector2(
            anchoredPos.x,
            Mathf.Clamp(newY, 0f, parentSize.y)
        );
        if (newY > parentSize.y) {
            currentDirection = IndicatorDirection.Down;
        } else if (newY <= 0) {
            currentDirection = IndicatorDirection.Up;
        }
    }

    private bool started = false;
    private void HandleWhenStarted()
    {
        HandleMoving();
        if (inputConfig.GetKeyDown(GameAction.MiniGameIndicatorStop)) {
            MakeSelection();
        }
    }

    private void MakeSelection() {

        int selectionPosition = (int)(rectTransform.anchoredPosition.y / parentSize.y * 100f);
        bool areaWasHit = minigame.MakeSelection(selectionPosition);
        if (!areaWasHit) {
            Reset();
        } else {
            started = false;
            finished = true;
        }
    }

    private void HandleWhenStopped() {
        if (inputConfig.GetKeyDown(GameAction.MiniGameIndicatorStart)) {
            started = true;
        }
    }

}

public enum IndicatorDirection
{
    None,
    Up,
    Down
}

[System.Serializable]
public class IndicatorOptions
{
    public float Speed;
    [Range(0, 100)]
    public int StartPercentage;
    public IndicatorDirection Direction;
}
