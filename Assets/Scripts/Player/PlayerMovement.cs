using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    //Declaro la variable de tipo RigidBody que luego asociaremos a nuestro Jugador
    Rigidbody rb;

    //Declaro la variable p�blica velocidad para poder modificarla desde la Inspector window
    float currentSpeed = 5;

    public float walkSpeed;
    public float runSpeed;
    public float rotationSpeed;

    Animator animator;


    void Start()
    {

        //Capturo el rigidbody del jugador al iniciar el juego
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void FixedUpdate()
    {

        //Capturo el movimiento en horizontal y vertical de nuestro teclado
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        bool isMoving = (horizontalMovement != 0 || verticalMovement != 0);
        animator.SetBool("isWalking", isMoving);

        if (Input.GetKey(KeyCode.LeftShift)&& isMoving)
        {
            currentSpeed = runSpeed;
            animator.SetBool("isRunning", true);
        }
        else
        {
            currentSpeed = walkSpeed;
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
