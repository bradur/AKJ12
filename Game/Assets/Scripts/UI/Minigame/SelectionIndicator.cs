using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField]
    private IndicatorOptions defaultOptions;
    private IndicatorOptions options;

    private InputConfig inputConfig;

    private bool finished = false;
    private bool canBeStarted = false;

    private Vector2 parentSize;

    private IndicatorDirection currentDirection;

    private void Start() {
        parentSize = transform.parent.GetComponent<RectTransform>().sizeDelta;
        Debug.Log(parentSize);
        Debug.Log(transform.position);
    }

    public void Initialize()
    {
        Initialize(defaultOptions);
    }

    public void Initialize(IndicatorOptions newOptions)
    {
        options = newOptions;
        Reset();
    }

    public void Enable() {
        canBeStarted = true;
    }

    public void Disable() {
        canBeStarted = false;
    }

    private void Reset() {
        currentDirection = options.Direction;
        transform.position = new Vector2(
            transform.position.x,
            options.StartPercentage
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
        return currentDirection == IndicatorDirection.Up ? -1 : 1;
    }

    private void HandleMoving() {
        transform.position = new Vector2(
            transform.position.x,
            transform.position.y + GetDirectionFactor() * (options.Speed * Time.deltaTime)
        );
        //transform.position options.Speed
    }


    private bool started = false;
    private void HandleWhenStarted()
    {
        HandleMoving();
        if (inputConfig.GetKeyDown(GameAction.MiniGameIndicatorStop)) {

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
    public int StartPercentage;
    public IndicatorDirection Direction;
}
