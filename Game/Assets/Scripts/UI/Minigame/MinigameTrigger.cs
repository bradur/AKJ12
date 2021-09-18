using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameTrigger : MonoBehaviour
{
    private Minigame minigame;
    public Minigame Minigame { get { return minigame; } }
    private MinigameTriggerOptions options;

    private bool showing = false;

    private bool finished = false;

    [SerializeField]
    private MinigameTriggerOptions defaultOptions;

    void Start() {
        Initialize();
    }

    public void Initialize()
    {
        defaultOptions.PositiveAction = delegate(int value, Vector2 pos) {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = Color.green;
            textOptions.Text = $"Great!";
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
        };
        defaultOptions.NegativeAction = delegate(int value, Vector2 pos) {
            PoppingTextOptions textOptions = new PoppingTextOptions();
            textOptions.Color = Color.red;
            textOptions.Text = $"Poor!";
            textOptions.Position = pos;
            WorldUI.main.ShowPoppingText(textOptions);
        };
        Initialize(defaultOptions);
    }
    public void Initialize(MinigameTriggerOptions newOptions)
    {
        options = newOptions;
        if (minigame == null)
        {
            minigame = WorldUI.main.GetMinigame();
        }
    }

    public void Finish() {
        finished = true;
    }

    private void ShowMinigame()
    {
        if (minigame.Trigger != this)
        {
            if (minigame.Trigger == null || (minigame.Trigger.DistanceFromTrigger() >= DistanceFromTrigger())) {
                minigame.Initialize(this, options.MinigameOptions, options.PositiveAction, options.NegativeAction);
                minigame.SetPosition(transform.position);
            }
        }
        if (minigame.Trigger == this) {
            minigame.Show();
            Debug.Log("Show minigame!");
        }
    }

    private void HideMinigame()
    {
        if (minigame != null)
        {
            minigame.Hide();
        }
    }

    public float DistanceFromTrigger() {
        return Mathf.Abs(Vector2.Distance(transform.position, options.Target.position));
    }

    void Update()
    {
        if (minigame == null) {
            return;
        }
        if (!finished && !showing && DistanceFromTrigger() <= options.Radius)
        {
            showing = true;
            ShowMinigame();
        }
        if (showing && DistanceFromTrigger() > options.Radius)
        {
            if (minigame.Trigger == this) {
                HideMinigame();
            }
            showing = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = defaultOptions.DebugColor;
        Gizmos.DrawSphere(transform.position, defaultOptions.Radius);
    }
}

[System.Serializable]
public class MinigameTriggerOptions
{
    public Color DebugColor;
    public float Radius;
    public Transform Target;
    public MinigameOptions MinigameOptions;
    public UnityAction<int, Vector2> PositiveAction;
    public UnityAction<int, Vector2> NegativeAction;
}
