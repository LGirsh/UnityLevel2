using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Unit
{

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake(); // переписанный эвэйк
        Health = 100; // здоровье равно 100
        Dead = false; // разрешается умереть
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)// если разешается умереть
        { 
            GoRigidbody.isKinematic = true; //ставится галочка кинематик в ригидбоди
            GoAnimator.SetBool("die", true);//триггер по имени дай равен правда 
            Destroy(gameObject, 5f);//вызов функции дестрой с параметрами геймобжект и 5 
            return;//выйти
        }
        
    }
}
