using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] private float proximityRadius = 1f;
    public Node closestNode { get; private set; } = null;
    public int layerMask;
    

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Navigation");
        Debug.Log(layerMask);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SetClosestNode();
    }

    private void SetClosestNode()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, proximityRadius, Vector2.zero, 0f, layerMask);
        Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            float closestDistance = proximityRadius;
            for (int i = 0; i < hits.Length; ++i)
            {
                GameObject hitGameObject = hits[i].collider.gameObject;
                Node node;
                float distance = Vector2.Distance(hitGameObject.transform.position, transform.position);
                if (hitGameObject.TryGetComponent(out node) &&
                    distance < closestDistance)
                {
                    closestNode = node;
                    closestDistance = distance;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
        if (closestNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, closestNode.transform.position);
        }
    }
}
