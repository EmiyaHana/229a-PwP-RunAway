using UnityEngine;

public class Updraft : MonoBehaviour
{
    public float windForce = 15f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyUpdraft(windForce);
            }
        }
    }
}