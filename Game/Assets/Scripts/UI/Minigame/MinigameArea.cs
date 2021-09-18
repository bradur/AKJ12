using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameArea : MonoBehaviour
{
    private RectTransform parentRectTransform;

    private RectTransform rectTransform;
    private MinigameAreaOptions options;
    public MinigameAreaOptions Options { get { return options; } }
    private Minigame game;
    [SerializeField]
    private Image imgBackground;

    public int PercentageSize { get { return options.PercentageSize; } }

    public void Initialize(MinigameAreaOptions newOptions)
    {
        options = newOptions;
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        SetupSize();
        imgBackground.color = options.Color;
        name = $"Area {newOptions.Result} ({newOptions.ResultValue})";
    }

    private void SetupSize()
    {
        float sizeMultiplier = options.PercentageSize / 100f;
        float sizeY = parentRectTransform.rect.height * sizeMultiplier;
        rectTransform.sizeDelta = new Vector2(
            parentRectTransform.rect.width,
            sizeY
        );
    }
}



[System.Serializable]
public class MinigameAreaOptions
{
    [Range(1, 100)]
    public int PercentageSize;

    public Color Color;

    [Range(1, 100)]
    public int ResultValue = 100;

    public SelectionResult Result;
}

public enum SelectionResult
{
    None,
    Positive,
    Negative
}