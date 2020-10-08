using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Unit
{

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Health = 100;
        Dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        { 
            GoRigidbody.isKinematic = true;
            GoAnimator.SetBool("die", true);
            Destroy(gameObject, 5f);
            return;
        }
        
    }
}
