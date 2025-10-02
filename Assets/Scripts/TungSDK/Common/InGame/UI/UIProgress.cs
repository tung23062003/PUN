using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : TickBehaviour
{
    [SerializeField] bool showText;
    [ShowIfNot(nameof(percent)), SerializeField]
    bool fractions;
    [ShowIfNot(nameof(fractions)), SerializeField]
    bool percent;
    [SerializeField] protected Slider slider;
    [SerializeField] TMP_Text text;
    [SerializeField] Image imgFill;
    [SerializeField] Image imgFillArea;
    [SerializeField] Color colorFill = Color.green;
    [SerializeField] Color colorBackground = Color.gray;
    [HorizontalLine("Progress", 2, FixedColor.Orange)]
    [Progress(nameof(slider), nameof(UpdateProgress))]
    [SerializeField] float progress = 0f;

    public float Progress
    {
        get => progress;
        set
        {
            progress = value;
            UpdateProgress();
        }
    }

    public void SetColor(Color color)
    {
        imgFill.color = color;
    }
    public void SetMin(float min = 0)
    {
        slider.minValue = min;
    }
    public void SetMax(float max = 1)
    {
        slider.maxValue = max;
    }
    protected virtual void OnValidate()
    {
        if (imgFillArea != null)
        {
            imgFillArea.color = colorBackground;
        }
        if (imgFill != null)
        {
            imgFill.color = colorFill;
        }
    }
    public void UpdateProgress()
    {
        slider.value = progress;
        if (text != null)
        {
            if (showText)
            {
                if (percent)
                {
                    text.text = $"{(int)(progress * 100)}%";
                }
                else if (fractions)
                {
                    text.text = $"{(int)(progress * 100)}/{(int)(slider.maxValue * 100)}";
                }
                else
                {
                    text.text = $"{progress}";
                }
            }
            else
            {
                text.text = "";
            }
        }
    }
}

