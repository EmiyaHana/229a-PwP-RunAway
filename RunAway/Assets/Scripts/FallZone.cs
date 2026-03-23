using UnityEngine;
using UnityEngine.SceneManagement;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameScene");
        }
    }
}