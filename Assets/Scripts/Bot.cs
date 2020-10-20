using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Bot : Unit
{
    private NavMeshAgent _agent;
    private Transform playerPos;
    private int stoppingDistance = 3;
    private Transform target;
    

    [SerializeField] private float stopDistanse = 0.2f;
    [SerializeField] private float seekDistance = 2f;
    [SerializeField] private float attackDistance = 8f;


    [SerializeField] private List<Vector3> wayPoints = new List<Vector3>();
    private int pointCounter;
    [SerializeField] private GameObject wayPointMain;// Change it to load data from file

    private float timeWait = 3;
    private float timeOut;

    [SerializeField] private bool patrol;
    [SerializeField] private bool shooting;

    //Target
    [SerializeField] private Collider[] targetInViewRadius;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private float maxAngle = 30;
    [SerializeField] private float maxRadius = 20;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;


#if UNITY_EDITOR    

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up;
        Handles.color = new Color(1,0,1,0.1f);
        Handles.DrawSolidArc(pos, transform.up, transform.forward, maxAngle, maxRadius);
        Handles.DrawSolidArc(pos, transform.up, transform.forward, -maxAngle, maxRadius);

    }

#endif

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
        playerPos = GameObject.FindObjectOfType<SinglePlayer>().transform;
        _agent.stoppingDistance = stopDistanse;

        Health = 100; 
        Dead = false;

        patrol = true;

        foreach(Transform item in wayPointMain.transform)
        {
            wayPoints.Add(item.position);
        }

        StartCoroutine(FindTargets(0.1f));
        //gun
    }

    private void FindVisibleTargets()
    {
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
        }

        if( _agent.isOnOffMeshLink )
        {
            transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.005f, ForceMode.Impulse);
        }
        
        _agent.SetDestination(playerPos.position);

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
                transform.LookAt(new Vector3(target.position.x, 1, transform.position.z));
            }



        }

        if (Dead)
        {
            GoRigidbody.isKinematic = true; 
            GoAnimator.SetBool("die", true); 
            Destroy(gameObject, 5f); 
            return;
        }
        
    }
}
