using System;

public class MachineGun : Weapon
{
    private const int BulletsPerShot = 3;
    private const int Damage = 9;

    public override event Action<int, int> Shooted;

    public MachineGun(int patronsCount) : base(patronsCount)
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
