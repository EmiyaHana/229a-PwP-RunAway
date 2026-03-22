using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private bool canHunt = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void Update()
    {
        if (canHunt && GameManager.Instance.currentState == GameState.Playing && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void StartHunting()
    {
        agent.enabled = true;
        canHunt = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy caught the player!");
            GameManager.Instance.GameOver(false); // false = â´¹¨Ñº á¾é!
        }
    }
}