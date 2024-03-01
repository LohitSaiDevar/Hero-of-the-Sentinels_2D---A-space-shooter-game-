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
            Debug.Log("Default Weapon: " + powerUps.defaultWeapon);
            powerUps.ShootDefaultWeapon(transform);
        }
        else if (powerUps.changeToBulletGrunt)
        {
            Debug.Log("Default Weapon: " + powerUps.defaultWeapon);
            Debug.Log("Default Weapon: " + powerUps.changeToBulletGrunt);
            powerUps.ShootBulletGrunt(transform);
        }
        else if (powerUps.changeToBulletElite)
        {
            Debug.Log("Default Weapon: " + powerUps.defaultWeapon);
            Debug.Log("Default Weapon: " + powerUps.changeToBulletElite);
            powerUps.ShootBulletElite(transform);
        }
        else if (powerUps.changeToLaser && !powerUps.laserCooldown)
        {
            Debug.Log("Default Weapon: " + powerUps.defaultWeapon);
            Debug.Log("Default Weapon: " + powerUps.changeToLaser);
            powerUps.laserActive = true;
            powerUps.ToggleLaser();

            if (!powerUps.laserCooldown && !powerUps.isCooldownActive)
            {
                powerUps.StartCoroutine(powerUps.LaserCooldown());
            }
        }
    }
}
