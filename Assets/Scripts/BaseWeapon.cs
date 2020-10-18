using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon : BaseObject
{
    [SerializeField] protected Transform gunT;  // переменная трансформ для огнестрельных орудий
    [SerializeField] protected ParticleSystem muzzleFlash; // переменная частицы для огнестрельных орудий
    [SerializeField] protected GameObject hitParticle; // переменная геймобжект(объект столкновения с частицами)
    [SerializeField] protected bool fire = true; // разрешается огонь
    [SerializeField] protected Transform TMCam; // переменная трансформ для камеры
    [SerializeField] protected GameObject cross; 
    [SerializeField] protected int bulletCount;// колво пуль
    [SerializeField] protected int currentBulletCount;// текущее колво пуль
    [SerializeField] protected float shootDistance;// расстояние полёта снаряда
    [SerializeField] protected int damage;//урон

    protected Text ammoText;// текст колва пуль

    private void OnEnable()
    {
        ammoText.text = currentBulletCount.ToString();//текст = текущее колво пуль
    }

    protected override void Awake()
    {
        base.Awake();// переписанный эвэйк
        gunT = transform.GetChild(2);// трансформ ребёнка номер 2
        muzzleFlash = GetComponentInChildren<ParticleSystem>();// получение компонента частицы 
        hitParticle = Resources.Load<GameObject>("Flare");// загружение префаба флэйер
        TMCam = Camera.main.transform;// трансформ главной камеры
        ammoText = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Ammo").GetComponent<Text>();//получение текса объекта аммо для аммотекст
    }

    public abstract void Fire(); //вызов фаер
}
