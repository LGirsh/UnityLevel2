using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class Bot : Unit
{
    private NavMeshAgent _agent;
    private Transform playerPos;
    // private int stoppingDistance = 3; // Commented by me
    private Transform target;
    
    [Header("Дистанции остановки: ")]
    [SerializeField] private float stopDistanse = 0.2f;
    [SerializeField] private float seekDistance = 2f;
    [SerializeField] private float attackDistance = 8f;
    [Header("Скорость поворота: ")]

    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private List<Vector3> wayPoints = new List<Vector3>();
    private int pointCounter;
    [SerializeField] private GameObject wayPointMain;// Change it to load data from file

    private float timeWait = 3;
    private float timeOut;

    //Shooting


    [Header("Настройки для оружия: ")]
    [SerializeField] protected int bulletCount;
    [SerializeField] protected int currentBulletCount;
    [SerializeField] protected float shootDistance;
    [SerializeField] protected int damage;

    [Tooltip("Объект добавляется автоматически, должен находится на дуле оружия")]
    [SerializeField] protected Transform gunT;
    [Tooltip("Объект добавляется автоматически")]
    [SerializeField] protected ParticleSystem muzzleFlash;
    [Tooltip("Объект добавляется автоматически")]
    [SerializeField] protected GameObject hitParticle;


    [Header("Состояние бота: ")]
    [SerializeField] private bool patrol;
    [SerializeField] private bool shooting;

    //Target
    [Header("Списки целей: ")]

    [SerializeField] private Collider[] targetInViewRadius;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    [Header("Настройки зоны видимости: ")]

    [Range(30,90)] [SerializeField] private float maxAngle = 30;
    [Range(10,40)] [SerializeField] private float maxRadius = 20;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    public List<Vector3> WayPoints { get => wayPoints; set => wayPoints = value; }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up;
        Handles.color = new Color(1,0,1,0.1f);
        Handles.DrawSolidArc(pos, transform.up, transform.forward, maxAngle, maxRadius);
        Handles.DrawSolidArc(pos, transform.up, transform.forward, -maxAngle, maxRadius);

    }

    [ContextMenu("Default Values")]
    public void Default()
    {
        bulletCount =30;
        shootDistance = 1000f;
        damage = 20;
        maxAngle = 30;
        maxRadius = 20;
        patrol = true;
    }

    [ContextMenu("Random values for visibility")]
    public void RandomAngle()
    {
        maxRadius = Random.Range(10,40);
        maxAngle = Random.Range(30, 90);
    }
#endif

    IEnumerator Shoot(RaycastHit playerHit)
    {
        yield return new WaitForSeconds(0.5f);
        muzzleFlash.Play();
        playerHit.collider.GetComponent<ISetDamage>().SetDamage((damage));
        GameObject temp = Instantiate(hitParticle, playerHit.point, Quaternion.identity);
        temp.transform.parent = playerHit.transform;
        Destroy(temp, 0.8f);
        shooting = false;
    }
    IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = stopDistanse;

        playerPos = GameObject.FindObjectOfType<SinglePlayer>().transform;

        Health = 100; 
        Dead = false;

        patrol = true;

        //foreach(Transform item in wayPointMain.transform)
        //{
        //    wayPoints.Add(item.position);
        //}

        StartCoroutine(FindTargets(0.1f));
        //gun

        gunT = GameObject.FindGameObjectWithTag("GunT").transform;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Flare");
        bulletCount = 30;
        currentBulletCount = bulletCount;
        shootDistance = 1000f;
        damage = 20;

    }

    private void FindVisibleTargets()
    {
        //visibleTargets.Clear(); // My Code!!!!

        targetInViewRadius = Physics.OverlapSphere(transform.position, maxRadius, targetMask);
        for(int i = 0;i < targetInViewRadius.Length; i++)
        {
            Transform tempTarget = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (tempTarget.position - transform.position).normalized;
            float targetAngle = Vector3.Angle(transform.forward, dirToTarget);

            if((-maxAngle) < targetAngle && targetAngle < maxAngle)
            {
                if(!Physics.Raycast(transform.position + Vector3.up, dirToTarget, obstacleMask))
                {
                    if (!visibleTargets.Contains(tempTarget))
                    {
                        visibleTargets.Add(tempTarget);
                    }
                    
                }
            }
        }
    }

    void defaultState()
    {
        _agent.SetDestination(playerPos.position);
        _agent.stoppingDistance = attackDistance;
    }

    void Update()
    {
        if(visibleTargets.Count > 0)
        {
            patrol = false;
            target = visibleTargets[0];
            float DistToTarget = Vector3.Distance(transform.position, target.position);
            if(DistToTarget > maxRadius)
            {
                visibleTargets.Clear();
            }
        }
        else
        {
            patrol = true;
            //Debug.Log("PATROLLING");
            //_agent.stoppingDistance = stopDistanse; // My code !!!!
            //_agent.SetDestination(wayPoints[pointCounter]); // My code !!!!
        }

        //if ( _agent.isOnOffMeshLink )
        //{
        //   transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.005f, ForceMode.Impulse);
        //}
        
        //_agent.SetDestination(playerPos.position); // Commented by me !!!!

        if(_agent.remainingDistance > _agent.stoppingDistance)
        {
            GoAnimator.SetBool("move", true);
        }
        else
        {
            GoAnimator.SetBool("move", false);
        }
        

        if (patrol)
        {
            if (wayPoints.Count > 1)
            {
                _agent.stoppingDistance = stopDistanse;
                _agent.SetDestination(wayPoints[pointCounter]);
                if (!_agent.hasPath)
                {
                    timeOut += 0.1f;
                    if (timeOut > timeWait)
                    {
                        timeOut = 0;
                        if (pointCounter < wayPoints.Count-1)
                        {
                            pointCounter++;
                        }
                        else
                        {
                            pointCounter = 0;
                        }
                    }
                }
            }

            else
            {
                defaultState();
            }
        }
        else
        {
            _agent.SetDestination(target.position);
            _agent.stoppingDistance = attackDistance;
            if (!Dead)
            {
                Debug.Log("Rotate");
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotationRes = Quaternion.LookRotation(direction);
                lookRotationRes.x = 0f;
                lookRotationRes.z = 0f;

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotationRes, Time.deltaTime *rotationSpeed);
            }


            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);

            if(Physics.Raycast(ray, out hit, shootDistance,targetMask))
            {
                if (hit.collider.tag == "Player" && !shooting)
                {
                    _agent.ResetPath();
                    GoAnimator.SetBool("shoot", true);
                    shooting = true;
                    StartCoroutine(Shoot(hit));
                }
                else
                {
                    //GoAnimator.SetBool("shoot", false);
                }
            }

            else
            {
                defaultState();
                GoAnimator.SetBool("shoot", false);

            }
        }


        if (Dead)
        {
            _agent.ResetPath();
            GoRigidbody.isKinematic = true; 
            GoAnimator.SetBool("die", true); 
            Destroy(gameObject, 5f); 
            return;
        }
        
    }
}
