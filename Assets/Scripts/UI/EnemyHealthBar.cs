using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Who to track")]
    [SerializeField] private CombatantBase enemy;

    [Header("UI (use either)")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;


    [Header("Behaviour")]
    [SerializeField] private bool useStartingHealthAsMax = true;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float lerpSpeed = 8f;

    private float displayed; // smoothed current value
    private int cachedMax;

    void OnEnable()
    {
        if (!enemy) return;

        cachedMax = useStartingHealthAsMax ? Mathf.Max(1, enemy.Health) : Mathf.Max(1, maxHealth);
        displayed = enemy.Health;
        ApplyImmediate();
    }

    void Update()
    {
        if (!enemy) return;

        // Smooth towards the real health
        displayed = Mathf.MoveTowards(displayed, enemy.Health, Time.deltaTime * lerpSpeed * Mathf.Max(1, cachedMax));
        Apply(displayed);
    }

    void ApplyImmediate()
    {
        Apply(displayed);
    }

    void Apply(float currentValue)
    {
        float norm = Mathf.Clamp01(cachedMax > 0 ? currentValue / cachedMax : 0f);

        if (fillImage) fillImage.fillAmount = norm;
    }
}

