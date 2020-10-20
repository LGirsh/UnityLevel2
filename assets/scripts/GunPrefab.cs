using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPrefab : BaseWeapon
{

    protected override void Awake()
    {
        base.Awake();
        fire = true;
        bulletCount = 30;
        currentBulletCount = bulletCount;
        shootDistance = 1000f;
        damage = 20;
        Reload = KeyCode.R;
    }

    public override void Fire()
    {
        if(currentBulletCount > 0 && fire)
        {
            GoAnimator.SetTrigger("shoot");
            muzzleFlash.Play();
            currentBulletCount--;

            //GameObject temp = Instantiate(bullet, gunT.position, Quaternion.identity);

        }
    }


  
}
