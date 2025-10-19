using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimiento horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // Actualizar animaci�n
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Voltear sprite seg�n direcci�n
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Fight");
        }
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

    }

    void FixedUpdate()
    {
        // Movimiento f�sico
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}
