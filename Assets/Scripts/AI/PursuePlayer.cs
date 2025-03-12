using UnityEngine;
using UnityEngine.AI;

public class PursuePlayer : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public Animator animator;
    public float attackRadius;

    [SerializeField] Transform hitbox;
    [SerializeField] LayerMask attackLayer;
    private bool isGame;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isGame = false;
        GameManager.OnStart += StartChase;
    }

    private void OnDestroy()
    {
        GameManager.OnStart -= StartChase;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGame)
        {
            ChasePlayer();
            TransitionAnimation();
            Attack();
        }
    }

    private void ChasePlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        else
            agent.SetDestination(player.position);
    }

    private void TransitionAnimation()
    {
        bool isMoving = agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance;
        // Set animation parameter
        animator.SetBool("isRunning", isMoving);
    }

    private void Attack()
    {
        Collider[] objetos = Physics.OverlapSphere(hitbox.position, attackRadius, attackLayer);

        if (objetos.Length > 0)
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            TransitionAnimation();
            EndChase();
            GameManager.Instance.PlayerCaught();
        }
    }

    private void StartChase()
    {
        isGame = true;
    }

    private void EndChase()
    {
        isGame = false;
    }
}
