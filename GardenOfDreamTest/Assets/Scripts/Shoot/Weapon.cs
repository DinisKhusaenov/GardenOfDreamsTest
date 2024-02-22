using System;
using UnityEngine;

public abstract class Weapon
{
    protected int PatronsCount;

    public abstract event Action<int, int> Shooted;

    protected Weapon(int patronsCount)
    {
        PatronsCount = patronsCount;
    }

    public abstract void Shoot();

    protected void OnAmmoIsOut()
    {
        Debug.Log("Нужно зарядить!");
    }
}
