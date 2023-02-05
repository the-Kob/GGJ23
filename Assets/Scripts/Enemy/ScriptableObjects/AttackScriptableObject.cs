using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Configuration", menuName = "ScriptableObject/Attack Configuration")]
public class AttackScriptableObject : ScriptableObject
{
    public bool isRanged = false;
    public int damage = 5;
    public float attackRadius = 1.5f;
    public float attackDelay = 1.5f;

    // Ranged Configs
    public EnemyBullet bulletPrefab;
    public Vector3 bulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask lineOfSightLayers;

    public AttackScriptableObject ScaleUpForLevel(ScalingScriptableObject scaling, int round)
    {
        AttackScriptableObject scaledUpConfiguration = CreateInstance<AttackScriptableObject>();

        scaledUpConfiguration.isRanged = isRanged;
        scaledUpConfiguration.damage = Mathf.FloorToInt(damage * scaling.damageCurve.Evaluate(round));
        scaledUpConfiguration.attackRadius = attackRadius;
        scaledUpConfiguration.attackDelay = attackDelay;

        scaledUpConfiguration.bulletPrefab = bulletPrefab;
        scaledUpConfiguration.bulletSpawnOffset = bulletSpawnOffset;
        scaledUpConfiguration.lineOfSightLayers = lineOfSightLayers;

        return scaledUpConfiguration;
    }

    public void SetupEnemy(Enemy enemy)
    {
        (enemy.attackRadius.coll == null ? enemy.attackRadius.GetComponent<SphereCollider>() : enemy.attackRadius.coll).radius = attackRadius;
        enemy.attackRadius.attackDelay = attackDelay;
        enemy.attackRadius.damage = damage;

        if (isRanged)
        {
            RangedAttackRadius rangedAttackRadius = enemy.attackRadius.GetComponent<RangedAttackRadius>();

            rangedAttackRadius.bulletPrefab = bulletPrefab;
            rangedAttackRadius.bulletSpawnOffset = bulletSpawnOffset;
            rangedAttackRadius.mask = lineOfSightLayers;

            rangedAttackRadius.CreateBulletPool();
        }
    }
}
