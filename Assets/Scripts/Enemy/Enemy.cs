using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public int health = 10;

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }
}
