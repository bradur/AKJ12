using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameTrigger : MonoBehaviour
{
    private Minigame minigame;
    public Minigame Minigame { get { return minigame; } }
    private MinigameTriggerOptions options;

    private bool triggerEnabled = false;
    private bool minigameShown = false;

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
        triggerEnabled = true;
    }

    private void ShowMinigame()
    {
        if (minigame == null)
        {
            minigame = WorldUI.main.CreateMinigame(transform.position);
            minigame.Initialize(options.MinigameOptions, options.PositiveAction, options.NegativeAction);
            minigame.Show();
            minigameShown = true;
            return;
        }
        if (!minigame.Finished)
        {
            minigame.Show();
            minigameShown = true;
        }
        else
        {
            triggerEnabled = false;
        }
    }

    private void HideMinigame()
    {
        if (minigame != null)
        {
            minigame.Hide();
            minigameShown = false;
        }
    }

    private float DistanceFromTrigger() {
        return Mathf.Abs(Vector2.Distance(transform.position, options.Target.position));
    }

    void Update()
    {
        if (triggerEnabled)
        {
            if (!minigameShown && DistanceFromTrigger() <= options.Radius)
            {
                ShowMinigame();
            }
            else if (minigameShown && DistanceFromTrigger() > options.Radius)
            {
                HideMinigame();
            }
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
