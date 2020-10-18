using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Unit
{
    private NavMeshAgent _agent;
    private Transform _playerT;
    private int stoppingDistance = 3;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _playerT = GameObject.FindObjectOfType<SinglePlayer>().transform;
        Health = 100; 
        Dead = false; 
    }

    void Update()
    {

        if( _agent.isOnOffMeshLink )
        {
            transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.005f, ForceMode.Impulse);
        }

        _agent.stoppingDistance = stoppingDistance;
        _agent.SetDestination(_playerT.position);

        if(_agent.remainingDistance > _agent.stoppingDistance)
        {
            GoAnimator.SetBool("move", true);
        }
        else
        {
            GoAnimator.SetBool("move", false);
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
