using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable
{
    private const string ATTACK_TRIGGER = "Attack";
    private Coroutine lookCoroutine;
    private bool tookDamageFlag = false;

    GameManager manager;
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public int health = 100;
    public AttackRadius attackRadius;
    public Animator animator;
    public delegate void DeathEvent(Enemy enemy);
    public DeathEvent OnDie;
    

    private void Awake()
    {
        attackRadius.onAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        animator.SetTrigger(ATTACK_TRIGGER);

        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt(target.GetTransform()));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
        OnDie = null;
    }

    public void TakeDamage(int damage)
    {
        if(!tookDamageFlag)
        {
            tookDamageFlag = true;
            movement.ChangeTarget(manager.player, manager.objective, true);
        }

        health -= damage;

        if(health <= 0)
        {
            OnDie?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
