using System;

public class Pistol : Weapon
{
    private const int BulletsPerShot = 3;
    private const int Damage = 5;

    public override event Action<int, int> Shooted;

    public Pistol(int patronsCount) : base(patronsCount)
    {
    }

    public override void Shoot()
    {
        if (PatronsCount > BulletsPerShot)
            Shooted?.Invoke(BulletsPerShot, Damage);
        else
            OnAmmoIsOut();
    }
}
