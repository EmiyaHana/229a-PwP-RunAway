using System.Collections;
using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    [Header("Platform Settings")]
    public float fallDelay = 1f;
    public float respawnDelay = 3f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private bool isFalling = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) //Unity Physics 3D (Trigger) -> use OnTriggerEnter function
    {
        if (other.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(PlatformFall());
        }
    }

    IEnumerator PlatformFall()
    {
        isFalling = true;

        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = false; 

        yield return new WaitForSeconds(respawnDelay);

        ResetPlatform();
    }

    void ResetPlatform()
    {
        rb.isKinematic = true; 
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        transform.position = startPosition; 
        transform.rotation = startRotation;
        
        isFalling = false;
    }
}