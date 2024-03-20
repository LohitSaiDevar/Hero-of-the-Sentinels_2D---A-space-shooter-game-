using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    //Weapon [0]: Grunt's Bullet
    //Weapon [1]: Elite's Bullet
    //Weapon [2]: Player's Default Bullet
    //Weapon [3]: Laser

    //Bullet Cooldown
    public bool countdown;

    public bool laserActive;
    private float laserCDTime = 0;
    public bool laserCooldown;
    int laserCDMaxTime = 5;
    public bool isCooldownActive = false;
    [SerializeField] CooldownBar laserCDBar;

    public bool changeToBulletGrunt, changeToBulletElite, changeToLaser, defaultWeapon;

    private void Start()
    {
        laserCDBar.SetMaxCooldown(laserCDMaxTime);
        laserActive = false;
        defaultWeapon = true;
    }
    //Weapon type: Bullets
    public void ShootDefaultWeapon(Transform playerTransform)
    {
        if (!countdown)
        {
            Instantiate(weapons[0], playerTransform.position + new Vector3(0, 0, 1), playerTransform.rotation);
            countdown = true;
            StartCoroutine(CountDown());
        }
    }

    public void DefaultWeaponActive()
    {
        defaultWeapon = true;
        changeToBulletGrunt = false;
        changeToBulletElite = false;
        changeToLaser = false;
    }

    public void ShootBulletGrunt(Transform playerTransform)
    {
        if (!countdown)
        {
            Instantiate(weapons[1], playerTransform.position + new Vector3(0, 0, 1), playerTransform.rotation);
            countdown = true;
            StartCoroutine(CountDown());
        }
    }
    public void BulletGruntActive()
    {
        defaultWeapon = false;
        changeToBulletGrunt = true;
        changeToBulletElite = false;
        changeToLaser = false;
    }


    public void ShootBulletElite(Transform playerTransform)
    {
        if (!countdown)
        {
            Instantiate(weapons[2], playerTransform.position + new Vector3(0, 0, 1), playerTransform.rotation);
            countdown = true;
            StartCoroutine(CountDown());
        }
    }

    public void BulletEliteActive()
    {
        changeToBulletGrunt = false;
        changeToBulletElite = true;
        defaultWeapon = false;
        changeToLaser = false;
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.1f);
        countdown = false;
    }

    //Weapon type: Laser
    public void ToggleLaser()
    {
        weapons[3].SetActive(laserActive);
        if (laserActive && laserCDTime <= laserCDMaxTime)
        {
            Debug.Log("Laser Active: " + laserActive);
            laserCDTime += Time.deltaTime;
            Debug.Log("laserCDTime: " + laserCDTime);
            laserCDBar.SetCooldown(laserCDTime);
            laserCDBar.gameObject.SetActive(true);
        }
        else if (!laserActive && laserCDTime > 0)
        {
            laserCDTime = Mathf.Max(0, laserCDTime - Time.deltaTime);
            laserCDBar.SetCooldown(laserCDTime);
            if (laserCDTime == 0)
            {
                laserCDBar.gameObject.SetActive(false);
            }
        }
        if (laserCDTime >= laserCDMaxTime)
        {
            laserCooldown = true;
        }
    }

    public void LaserActive()
    {
        defaultWeapon = false;
        changeToBulletGrunt = false;
        changeToBulletElite = false;
        changeToLaser = true;
    }

    public IEnumerator LaserCooldown()
    {
        isCooldownActive = true;
        weapons[3].SetActive(false);

        while (laserCDTime > 0)
        {
            laserCDTime = Mathf.Max(0, laserCDTime - Time.deltaTime);
            laserCDBar.SetCooldown(laserCDTime);
            yield return null;
        }

        laserCDBar.gameObject.SetActive(false);
        isCooldownActive = false;
        laserCooldown = false;
    }
}
