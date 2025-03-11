using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NodeController nodeController;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyCombatTester combatTester;
    [SerializeField] private Health health;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float pathRefreshTimeout = 2f;
    [SerializeField] private float targetMeleeDistance = 2f;
    [SerializeField] private float targetIdleDistance = 8f;

    // allowed variance in target position when idling
    [SerializeField] private float idleRangeX = 1f;
    [SerializeField] private float idleRangeY = 3f;

    [SerializeField] private float maximumMeleeDistanceX = 3f;
    [SerializeField] private float maximumMeleeDistanceY = 1f;
    public EnemyManager enemyManager;
    private Node closestNode = null;
    public List<Node> path = new List<Node>();

    public float pathTimeOffset = 0f;
    public float pathTime = 0f;
    public Vector3 displacementFromPlayer;
    public bool canMove = true;
    public bool isAttacking = false;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = EnemyManager.instance;
        pathTime = pathTimeOffset;
    }

    private void OnEnable()
    {
        health.onDie.AddListener(OnDie);
        health.onDamaged.AddListener(OnDamaged);
    }

    private void OnDisable()
    {
        health.onDie.RemoveListener(OnDie);
        health.onDamaged.RemoveListener(OnDamaged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDamaged()
    {
        animator.SetTrigger("Damaged");
    }

    private void OnDie()
    {
        enemyManager.OnDeadEnemy(this);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // check if alive
        if (health.isLiving)
        {

            pathTime += Time.deltaTime;
            if (canMove) // should probably replace this with a fsm down the line
            {
                FaceTheRightDirection(EnemyManager.instance.playerNodeController.transform.position);
                if (!CheckMelee())
                {
                    UpdatePath();
                }
            }
        }
        // else
        // {
        //     enemyManager.OnDeadEnemy(this);
        //     Destroy(gameObject);
        // }
    }

    public void UpdatePath()
    {
        closestNode = nodeController.closestNode;
        if (path.Count > 0 && pathTime < pathRefreshTimeout)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, 0), speed);
            animator.SetBool("IsMoving", true);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                closestNode = path[x];
                path.RemoveAt(x);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            if (pathTime >= pathRefreshTimeout)
            {

                CreatePath();
                pathTime = 0;
            }
        }
    }

    // if in range, attacks and returns true; otherwise returns false
    private bool CheckMelee()
    {
        if (!isAttacking) return false;
        displacementFromPlayer = transform.position - EnemyManager.instance.playerNodeController.transform.position;
        // if close enough to and in line with player, attack
        if (Mathf.Abs(displacementFromPlayer.x) <= maximumMeleeDistanceX && Mathf.Abs(displacementFromPlayer.y) <= maximumMeleeDistanceY && canMove)
        {
            audioManager.PlaySFX(audioManager.enemyDeath);
            Debug.Log("attack");
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("BasicAttack");
            enemyManager.OnEnemyAttack();
            isAttacking = false;
            return true;
        }
        return false;
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
        Node targetNode = SelectTargetNode();
        if (targetNode)
        {
            path = AStarManager.instance.GeneratePath(closestNode, targetNode);
        }
    }

    public Node SelectTargetNode()
    {
        Node playerNode = EnemyManager.instance.playerNodeController.closestNode;
        if (playerNode == null) return null;
        if (isAttacking)
        {
            if (playerNode.transform.position.x > transform.position.x)
            {
                // target node to the left of the player
                return AStarManager.instance.FindNearestNode(playerNode.transform.position - new Vector3(targetMeleeDistance, 0, 0));
            }
            else
            {
                // target node to the right of the player
                return AStarManager.instance.FindNearestNode(playerNode.transform.position + new Vector3(targetMeleeDistance, 0, 0));
            }
        }
        else
        {
            float dx = targetIdleDistance + Random.Range(-idleRangeX, idleRangeX);
            float dy = transform.position.y - playerNode.transform.position.y + Random.Range(-idleRangeY, idleRangeY);
            if (playerNode.transform.position.x > transform.position.x)
            {
                // target node to the left of the player
                return AStarManager.instance.FindNearestNode(playerNode.transform.position - new Vector3(dx, dy, 0));
            }
            else
            {
                // target node to the right of the player
                return AStarManager.instance.FindNearestNode(playerNode.transform.position + new Vector3(dx, dy, 0));
            }
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
