using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private HealthView _playerHealth;
    [SerializeField] private HealthView _enemyHealth;
    [SerializeField] private PlayerCells _playerCells;
    [SerializeField] private Shooter _shooter;

    private Player _player;
    private Enemy _enemy;

    private void Awake()
    {
        _player = new Player(_playerCells);
        _enemy = new Enemy();

        _playerHealth.Initialize(_player);
        _enemyHealth.Initialize(_enemy);
        _shooter.Initialize(_player, _enemy);
    }
}
