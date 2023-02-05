using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private NavMeshAgent agent;
    private Coroutine followCoroutine;
    private AgentLinkMover linkMover;

    public Transform target;
    public float updateRate = 0.1f;

    private const string isWalking = "isWalking";
    private const string jump = "jump";
    private const string landed = "landed";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        linkMover = GetComponent<AgentLinkMover>();

        linkMover.OnLinkEnd += HandleLinkEnd;
        linkMover.OnLinkStart += HandleLinkStart;
    }

    private void HandleLinkStart()
    {
        animator.SetTrigger(jump);
    }

    private void HandleLinkEnd()
    {
        animator.SetTrigger(landed);
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

    private void Update()
    {
        animator.SetBool(isWalking, agent.velocity.magnitude > 0.01f);
    }


    IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while(enabled)
        {
            agent.SetDestination(target.transform.position);

            yield return wait;
        }
    }

}
