using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NodeController nodeController;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyCombatTester combatTester;
    [SerializeField] private float pathRefreshTimeout = 2f;
    [SerializeField] private float targetMeleeDistance = 2f;
    [SerializeField] private float maximumMeleeDistanceX = 2.5f;
    [SerializeField] private float maximumMeleeDistanceY = 1f;
    private Node closestNode = null;
    public List<Node> path = new List<Node>();

    public float pathTime = 0f;
    public Vector3 displacementFromPlayer;
    public bool canMove = true;

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
        UpdatePath();
        FaceTheRightDirection(EnemyManager.instance.playerNodeController.transform.position);

    }

    public void UpdatePath()
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
            CheckMelee();
            CreatePath();
        }
    }

    private void CheckMelee()
    {
        displacementFromPlayer = transform.position - EnemyManager.instance.playerNodeController.transform.position;
        // if close enough to and in line with player, attack
        if (Mathf.Abs(displacementFromPlayer.x) <= maximumMeleeDistanceX && Mathf.Abs(displacementFromPlayer.y) <= maximumMeleeDistanceY && !combatTester.isPunching)
        {
            Debug.Log("attack");
            animator.SetTrigger("BasicAttack");
        }
    }

    private void FaceTheRightDirection(Vector3 target)
    {
        if (target.x > transform.position.x)
        {
            // face right
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // face left
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void CreatePath()
    {
        Node playerNode = EnemyManager.instance.playerNodeController.closestNode;
        if (playerNode == null) return;
        Node targetNode = null;
        if (playerNode.transform.position.x > transform.position.x)
        {
            // target node to the left of the player
            targetNode = AStarManager.instance.FindNearestNode(playerNode.transform.position - new Vector3(targetMeleeDistance, 0, 0));
        }
        else
        {
            // target node to the right of the player
            targetNode = AStarManager.instance.FindNearestNode(playerNode.transform.position + new Vector3(targetMeleeDistance, 0, 0));
        }
        if (targetNode)
        {
            path = AStarManager.instance.GeneratePath(closestNode, targetNode);
        }
        pathTime = 0;
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
