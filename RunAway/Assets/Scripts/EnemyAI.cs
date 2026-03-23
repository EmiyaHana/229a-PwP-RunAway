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

    public void StartHunting()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        
        Invoke("DelayedHunt", 0.1f);
    }

    void DelayedHunt()
    {
        if (agent != null && agent.isOnNavMesh) 
        {
            agent.isStopped = false;
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }
}