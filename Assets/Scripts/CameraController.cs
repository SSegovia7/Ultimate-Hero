using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [Header("Camera Attributes")]
    [Tooltip("Maximum distance that the target can be from the center before the camera starts moving")]
    [SerializeField] private float maxDistanceFromCenter = 1.0f;
    [SerializeField] private float leftLimit = -5.0f;
    [SerializeField] private float rightLimit = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = transform.position.x - targetTransform.position.x;
        if (distance > maxDistanceFromCenter)
        {
            transform.position = new Vector3(Mathf.Max(targetTransform.position.x + maxDistanceFromCenter, leftLimit), transform.position.y, transform.position.z);
        } else if (distance < -maxDistanceFromCenter)
        {
            transform.position = new Vector3(Mathf.Min(targetTransform.position.x - maxDistanceFromCenter, rightLimit), transform.position.y, transform.position.z);
        }
    }
}
