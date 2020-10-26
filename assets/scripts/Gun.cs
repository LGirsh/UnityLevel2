using UnityEngine;

public class Gun : BaseWeapon 
{
    protected override void Awake()
    {
        base.Awake();
        //fire = true;
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

            Ray ray = new Ray(TMCam.position, TMCam.forward);   // In BaseWeapon there is another "ray"

            if (Physics.Raycast(ray, out hit, shootDistance))
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

   
}
