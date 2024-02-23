using UnityEngine;

public class Level
{
    private Enemy _enemy;
    private Player _player;
    private Inventory _inventory;
    private DefeatPanel _defeatPanel;

    public Level(Enemy enemy, Player player, DefeatPanel defeatPanel, Inventory inventory)
    {
        _enemy = enemy;
        _player = player;
        _defeatPanel = defeatPanel;
        _inventory = inventory;

        _defeatPanel.Hide();

        _enemy.Died += Win;
        _player.Died += Lose;
    }

    private void Win()
    {
        _inventory.AddRandomItem();

        _enemy.Died -= Win;
    }

    private void Lose()
    {
        _defeatPanel.Show();

        _player.Died -= Lose;
    }
}
