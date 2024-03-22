using UnityEngine;
public class ShootCommand : ICommand
{
    private readonly PowerUps powerUps;
    private readonly Transform transform;

    public ShootCommand(PowerUps _powerUps, Transform _transform)
    {
        powerUps = _powerUps;
        transform = _transform;
    }

    public void Execute()
    {
        if (powerUps.defaultWeapon)
        {
            powerUps.ShootDefaultWeapon(transform);
        }
        else if (powerUps.changeToBulletGrunt)
        {
            powerUps.ShootBulletGrunt(transform);
        }
        else if (powerUps.changeToBulletElite)
        {
            powerUps.ShootBulletElite(transform);
        }
        else if (powerUps.changeToLaser && !powerUps.laserCooldown)
        {
            powerUps.laserActive = true;
            powerUps.ToggleLaser();

            if (powerUps.laserCooldown && !powerUps.isCooldownActive)
            {
                powerUps.StartCoroutine(powerUps.LaserCooldown());
            }
        }
    }
}
