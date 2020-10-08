using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : BaseObject, ISetDamage
{
    [SerializeField] private int _health;
    [SerializeField] private bool _dead;

    public int Health { get => _health; set => _health = value; }
    public bool Dead { get => _dead; set => _dead = value; }

    public void SetDamage(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
        if (Health <= 0)
        {
            Health = 0;
            Dead = true;
        }
    }
}
