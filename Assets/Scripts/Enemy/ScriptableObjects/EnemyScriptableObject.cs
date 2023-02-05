using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName ="ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy prefab;
    public AttackScriptableObject attackConfiguration;

    // Enemy Stats
    public int health = 100;

    // NavMeshAgent Config
    public float aiUpdateInterval = 0.1f;

    public float acceleration = 8;
    public float angularSpeed = 120;
    public int areaMask = -1; // -1 is everything
    public int avoidancePriority = 50;
    public float baseOffset = 0;
    public float height = 2f;
    public ObstacleAvoidanceType obstacleAvoidanceType;
    public float radius = 0.5f;
    public float speed = 3f;
    public float stoppingDistance = 0.5f;

    public EnemyScriptableObject ScaleUpForLevel(ScalingScriptableObject scaling, int round)
    {
        EnemyScriptableObject scaledUpEnemy = CreateInstance<EnemyScriptableObject>();

        scaledUpEnemy.name = name;
        scaledUpEnemy.prefab = prefab;

        scaledUpEnemy.attackConfiguration = attackConfiguration.ScaleUpForLevel(scaling, round);

        scaledUpEnemy.health = Mathf.FloorToInt(health * scaling.healthCurve.Evaluate(round));

        scaledUpEnemy.aiUpdateInterval = aiUpdateInterval;
        scaledUpEnemy.acceleration = acceleration;
        scaledUpEnemy.angularSpeed = angularSpeed;

        scaledUpEnemy.areaMask = areaMask;
        scaledUpEnemy.avoidancePriority = avoidancePriority;

        scaledUpEnemy.baseOffset = baseOffset;
        scaledUpEnemy.height = height;
        scaledUpEnemy.obstacleAvoidanceType = obstacleAvoidanceType;
        scaledUpEnemy.radius = radius;
        scaledUpEnemy.speed = speed; // * scaling.speedCurve.Evaluate(round);
        scaledUpEnemy.stoppingDistance = stoppingDistance;

        return scaledUpEnemy;
    }

    public void SetupEnemy(Enemy enemy)
    {
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;
        enemy.agent.areaMask = areaMask;
        enemy.agent.avoidancePriority = avoidancePriority;
        enemy.agent.baseOffset = baseOffset;
        enemy.agent.height = height;
        enemy.agent.obstacleAvoidanceType = obstacleAvoidanceType;
        enemy.agent.radius = radius;
        enemy.agent.speed = speed;
        enemy.agent.stoppingDistance = stoppingDistance;

        enemy.movement.updateRate = aiUpdateInterval;

        enemy.health = health;

        attackConfiguration.SetupEnemy(enemy);
    }
}
