using System;

public class Player: IHealth
{
    public const int MaxHealth = 100;

    public event Action<int> HealthChanged;
    public event Action Died;

    private PlayerCells _playerCells;
    private int _currentHealth;

    private bool _isHeadshot;

    int IHealth.MaxHealth => MaxHealth;

    public Player(PlayerCells playerCells)
    {
        _playerCells = playerCells;
        _currentHealth = MaxHealth;

        _isHeadshot = true;
    }

    public void Reduce(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (_isHeadshot)
            value -= _playerCells.HeadProtect;
        else
            value -= _playerCells.TorsProtect;

        if (value < 0)
            value = 0;

        _currentHealth -= value;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            Died?.Invoke();
        }

        _isHeadshot = !_isHeadshot;
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
