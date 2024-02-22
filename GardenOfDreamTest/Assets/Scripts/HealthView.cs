using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private IntValueView _currentHealth;

    private IHealth _health;

    public void Initialize(IHealth health)
    {
        _health = health;
        _currentHealth.Show(_health.MaxHealth);
    }

    private void OnEnable()
    {
        _health.HealthChanged += ChangeView;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= ChangeView;
    }

    private void ChangeView(int currentHealth)
    {
        _currentHealth.Show(currentHealth);
        _healthBar.fillAmount = (float)currentHealth/(float)_health.MaxHealth;
    }
}
