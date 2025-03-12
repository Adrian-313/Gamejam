using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    float currentSpeed = 5;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;
    public AudioClip death;

    Animator animator;

    private bool isGame;

    private void Awake()
    {
        isGame = false;

        GameManager.OnStart += StartGame;
        GameManager.OnEnd += PlayerDied;
    }

    private void OnDestroy()
    {
        GameManager.OnStart -= StartGame;
        GameManager.OnEnd -= PlayerDied;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void FixedUpdate()
    {
        if (isGame)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            bool isMoving = (horizontalMovement != 0 || verticalMovement != 0);

            if (isMoving)
            {
                currentSpeed = runSpeed;
                animator.SetBool("isRunning", true);
            }
            else
            {
                currentSpeed = 0;
                animator.SetBool("isRunning", false);
            }

            Vector3 moveDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);

            // Rotar en la direcci�n del movimiento
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void StartGame()
    {
        isGame = true;
    }

    private void PlayerDied()
    {
        isGame = false;
        animator.SetBool("isDead", true);
        GameManager.Instance.PlayASound(death);
    }
}
