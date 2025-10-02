using UnityEngine;
using UnityEngine.UI;

public class HealthProgress : UIProgress
{
    [SerializeField] private Slider sliderDamage;
    [SerializeField] Image imgFillDamage;
    [SerializeField] Color colorDamage = Color.white;
    [SerializeField] float timeShowDamage = 0.5f;
    [SerializeField] float speedLerp = 5f;
    float timer { get; set; }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (imgFillDamage != null)
        {
            imgFillDamage.color = colorDamage;
        }
    }

    public void SetFillDamage(float value)
    {
        timer = Time.time + timeShowDamage;
        sliderDamage.value = value;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (timer <= Time.time)
        {
            sliderDamage.value = Mathf.Lerp(sliderDamage.value, slider.value, Time.deltaTime * speedLerp);
        }
    }
}