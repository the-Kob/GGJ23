using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Coroutine followCoroutine;

    public Transform target;
    public float updateSpeed = 0.1f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartChasing()
    {
        if(followCoroutine == null)
        {
            followCoroutine = StartCoroutine(FollowTarget());
        } else
        {
            Debug.LogWarning("Called StartChasing on enemy that is already chasing!");
        }
    }


    IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while(enabled)
        {
            agent.SetDestination(target.transform.position);

            yield return wait;
        }
    }

}
