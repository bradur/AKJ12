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

    private Transform target;

    public void Initialize(MinigameTriggerOptions newOptions)
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
                Debug.Log($"minigame pos to {transform.position}");
            }
        }
        if (minigame.Trigger == this) {
            minigame.Show();
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
        return Mathf.Abs(Vector2.Distance(transform.position, target.position));
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
        if (options == null) {
            return;
        }
        // Draw a yellow sphere at the transform's position
        Gizmos.color = options.DebugColor;
        Gizmos.DrawSphere(transform.position, options.Radius);
    }
}

[System.Serializable]
public class MinigameTriggerOptions
{
    public Color DebugColor;
    public float Radius;
    public MinigameOptions MinigameOptions;
    public UnityAction<int, Vector2> PositiveAction;
    public UnityAction<int, Vector2> NegativeAction;
}
