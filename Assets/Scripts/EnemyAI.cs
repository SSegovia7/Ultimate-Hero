using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NodeController nodeController;
    [SerializeField] private float pathRefreshTimeout = 1f;
    private Node closestNode = null;
    public List<Node> path = new List<Node>();

    public float pathTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        pathTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        pathTime += Time.deltaTime;
        CreatePath();
    }

    public void CreatePath()
    {
        closestNode = nodeController.closestNode;
        if (path.Count > 0 && pathTime < pathRefreshTimeout)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, 0), 0.1f);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                closestNode = path[x];
                path.RemoveAt(x);
            }
        }
        else if (pathTime >= pathRefreshTimeout)
        {
            Node target = EnemyManager.instance.playerNodeController.closestNode;
            if (target != null)
            {
                path = AStarManager.instance.GeneratePath(closestNode, target);
            }
            pathTime = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < path.Count - 1; ++i)
        {
            Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
        }
    }
}
