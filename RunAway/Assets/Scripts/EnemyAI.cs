using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.Playing && player != null)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}