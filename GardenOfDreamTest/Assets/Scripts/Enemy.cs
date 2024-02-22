using System;

public class Enemy: IHealth
{
    public const int MaxHealth = 100;

    public event Action<int> HealthChanged;
    public event Action Died;

    private int _currentHealth;

    int IHealth.MaxHealth => MaxHealth;

    public Enemy()
    {
        _currentHealth = MaxHealth;
    }

    public void Reduce(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _currentHealth -= value;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            Died?.Invoke();
        }

        HealthChanged?.Invoke(_currentHealth);
    }

    public void Add(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _currentHealth += value;

        if (_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;

        HealthChanged?.Invoke(_currentHealth);
    }
}
