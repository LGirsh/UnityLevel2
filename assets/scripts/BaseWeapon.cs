using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected RaycastHit hit;
    [SerializeField] protected Ray ray;
    [SerializeField] protected KeyCode Reload;

    protected Text ammoText;// текст колва пуль

    private void OnEnable()
    {
        ammoText.text = currentBulletCount.ToString();
    }

    protected override void Awake()
    {
        base.Awake();// переписанный эвэйк
        gunT = transform.GetChild(2);// трансформ реб
        muzzleFlash = GetComponentInChildren<ParticleSystem>();// получение компонента частицы 
        hitParticle = Resources.Load<GameObject>("Flare");// загружение префаба флэйер
        TMCam = Camera.main.transform;// трансформ главной камеры
        ammoText = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Ammo").GetComponent<Text>();//получение текса объекта аммо для аммотекст
    }

    public abstract void Fire(); //вызов фаер


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
            GoAnimator.SetTrigger("reload");
            fire = false;
            ReloadBullet();
        }
    }
}
