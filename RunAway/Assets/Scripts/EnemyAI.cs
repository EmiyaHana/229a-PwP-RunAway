using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private bool canHunt = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    void Update()
    {
        if (canHunt && GameManager.Instance.currentState == GameState.Playing)
        {
            agent.SetDestination(player.position);
        }
    }

    public void StartHunting()
    {
        agent.enabled = true;
        canHunt = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(false);
        }
    }
}