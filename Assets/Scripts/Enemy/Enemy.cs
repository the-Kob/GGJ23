using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public int health = 10;

    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }

    public virtual void SetupAgentFromConfiguration()
    {
        agent.acceleration = enemyScriptableObject.acceleration;
        agent.angularSpeed = enemyScriptableObject.angularSpeed;
        agent.areaMask = enemyScriptableObject.areaMask;
        agent.avoidancePriority = enemyScriptableObject.avoidancePriority;
        agent.baseOffset = enemyScriptableObject.baseOffset;
        agent.height = enemyScriptableObject.height;
        agent.obstacleAvoidanceType = enemyScriptableObject.obstacleAvoidanceType;
        agent.radius = enemyScriptableObject.radius;
        agent.speed = enemyScriptableObject.speed;
        agent.stoppingDistance = enemyScriptableObject.stoppingDistance;

        movement.updateRate = enemyScriptableObject.aiUpdateInterval;

        health = enemyScriptableObject.health;
    }
}
