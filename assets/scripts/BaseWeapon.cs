using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : BaseObject
{
    [SerializeField] protected Transform gunT;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject hitParticle;
    [SerializeField] protected bool fire = true;
    [SerializeField] protected Transform TMCam;
    [SerializeField] protected GameObject cross;
    [SerializeField] protected int bulletCount;
    [SerializeField] protected int currentBulletCount;
    [SerializeField] protected float shootDistance;
    [SerializeField] protected int damage;

    protected override void Awake()
    {
        base.Awake();
        gunT = transform.GetChild(2);
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Flare");
        TMCam = Camera.main.transform;
    }

    public abstract void Fire();   
}
