using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedAttackRadius : AttackRadius
{
    private ObjectPool bulletPool;
    [SerializeField]
    private float spherecastRadius = 0.1f;
    private RaycastHit hit;
    private IDamageable targetDamageable;
    private EnemyBullet bullet;

    public NavMeshAgent agent;
    public EnemyBullet bulletPrefab;
    public Vector3 bulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask mask;

    public void CreateBulletPool()
    {
        if(bulletPool == null)
        {
            bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackDelay) * bulletPrefab.autoDestroyTime));
        }
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(attackDelay);

        yield return wait;

        while (damageables.Count > 0)
        {
            for (int i = 0; i < damageables.Count; i++)
            {
                if (HasLineOfSightTo(damageables[i].GetTransform()))
                {
                    targetDamageable = damageables[i];
                    onAttack?.Invoke(damageables[i]);
                    agent.enabled = false;
                    break;
                }
            }

            if (targetDamageable != null)
            {
                PoolableObject poolableObject = bulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<EnemyBullet>();

                    bullet.damage = damage;
                    bullet.transform.position = transform.position + bulletSpawnOffset;
                    bullet.transform.rotation = agent.transform.rotation;
                    bullet.rb.AddForce(agent.transform.forward * bulletPrefab.moveSpeed, ForceMode.VelocityChange);
                }
            }
            else
            {
                agent.enabled = true; // no target in line of sight, keep trying to get closer
            }

            yield return wait;

            if (targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                agent.enabled = true;
            }

            damageables.RemoveAll(DisabledDamageables);
        }

        agent.enabled = true;
        attackCoroutine = null;
    }

    private bool HasLineOfSightTo(Transform Target)
    {
        if (Physics.SphereCast(transform.position + bulletSpawnOffset, spherecastRadius, ((Target.position + bulletSpawnOffset) - (transform.position + bulletSpawnOffset)).normalized, out hit, coll.radius, mask))
        {
            IDamageable damageable;
            if (hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == Target;
            }
        }

        return false;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (attackCoroutine == null)
        {
            agent.enabled = true;
        }
    }
}
