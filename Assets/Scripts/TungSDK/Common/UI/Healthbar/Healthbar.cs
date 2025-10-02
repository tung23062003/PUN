using UnityEngine;

public class Healthbar : AlignCamera
{
    public HealthComponent Health;

    [SerializeField] Canvas canvas;

    [SerializeField] HealthProgress progress;

    [SerializeField]
    bool hideEmpty = false;

    [SerializeField]
    bool alignWithCamera = false;

    [SerializeField, Min(0.1f)]
    float changeSpeed = 100;

    float currentValue;

    float lastHealth;

    private void OnValidate()
    {
        if (Health == null)
        {
            Health = GetComponentInParent<HealthComponent>();
        }
    }

    private void Start()
    {
        if (Health == null)
        {
            Health = GetComponentInParent<HealthComponent>();
        }
        if (Health == null)
        {
            Debug.LogError("Health component not found");
            return;
        }
        InitHealth(Health.CurrentHealth);
        Health.OnHealthChanged += OnHealthChanged;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Health != null)
        {
            Health.OnHealthChanged -= OnHealthChanged;
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Health == null)
        {
            return;
        }
        IsAlignWithCamera = alignWithCamera;

        currentValue = Mathf.MoveTowards(currentValue, Health.CurrentHealth, Time.deltaTime * changeSpeed);
        UpdateFillbar();
        UpdateVisibility();
    }
    public void InitHealth(float health)
    {
        lastHealth = health;
        currentValue = health;        
    }
    public void OnHealthChanged(float changedAmount)
    {
        lastHealth = Health.CurrentHealth - changedAmount;
        progress.SetFillDamage(lastHealth / Health.MaxHealth);
    }

    void UpdateFillbar()
    {
        // Update the fill amount
        float value = Mathf.InverseLerp(0, Health.MaxHealth, currentValue);
        if (currentValue <= 0)
        {
            progress.SetFillDamage(0);
        }

        progress.Progress = value;
    }

    void UpdateVisibility()
    {
        float value = progress.Progress;

        if (canvas != null)
        {
            // Hide if empty
            if (Mathf.Approximately(value, 0))
            {
                if (hideEmpty && canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
            // Make sure the Canvas is enabled if health is not empty
            else if (value > 0 && canvas.gameObject.activeSelf == false)
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }
}