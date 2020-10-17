using UnityEngine;

public class Gun : BaseWeapon 
{
    public KeyCode Reload;
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
        if (currentBulletCount > 0 && fire)
        {
            GoAnimator.SetTrigger("shoot");
            muzzleFlash.Play();
            currentBulletCount--;
            ammoText.text = currentBulletCount.ToString();

            RaycastHit hit;

            Ray ray = new Ray(TMCam.position, TMCam.forward);

            if(Physics.Raycast(ray, out hit, shootDistance))
            {
                if (hit.collider.tag == "Player")
                {
                    return;
                }
                else
                {
                    SetDamage(hit.collider.GetComponent<ISetDamage>());
                }
            }

            CreateParticleHit(hit);

        } 
    } 
    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(damage);
        }
    }
    private void CreateParticleHit(RaycastHit hit)
    {
        GameObject tempHit = Instantiate(hitParticle, hit.point, Quaternion.identity);
        tempHit.transform.parent = hit.transform;
        Destroy(tempHit,0.5f);
    }

    public void ReloadBullet()
    {
        fire = true;
        currentBulletCount = bulletCount;
        ammoText.text = currentBulletCount.ToString();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if (Input.GetKeyDown(Reload))
        {
            fire = false;
            ReloadBullet();
        }
    }
}
