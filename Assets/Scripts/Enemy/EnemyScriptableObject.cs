using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName ="ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // Enemy stats
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

}
