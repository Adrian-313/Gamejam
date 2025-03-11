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

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // For the lack of player still, this is for mere testing purposes
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if(Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}
        //---

        ChasePlayer();
        TransitionAnimation();
        Attack();
    }

    private void ChasePlayer()
    {
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
            // TODO: Player attacked
        }
    }
}
