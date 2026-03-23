using UnityEngine;

public class Updraft : MonoBehaviour
{
    public float windForce = 200f;

    private void OnTriggerStay(Collider other) //Unity Physics 3D (Trigger) -> use OnTriggerStay (For Non-solid collision)
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