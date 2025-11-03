using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento en tierra")]
    public float moveSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Movimiento en agua")]
    public float waterMoveSpeed = 1.5f;
    public float waterGravity = 0.3f;
    public float normalGravity = 1f;
    public float swimForce = 3f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool isGrounded;
    private bool isInWater = false;

    // SE AGREGO NUEVOOOOOOO
    [Header("Ataque")]
    public Transform attackPoint;       // Empty delante del jugador
    public float attackRange = 0.5f;    // radio del ataque
    public int attackDamage = 1;        // daño que inflige


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalGravity = rb.gravityScale;
    }

    void Update()
    {
        // Movimiento horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // Voltear sprite según dirección
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // Actualizar animación
        animator.SetFloat("Blend", Mathf.Abs(moveInput));

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(DoAttack());
        }

        if (isInWater)
        {
            // Activar animación de natación
            animator.SetBool("isSwimming", true);

            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, swimForce);
            }
        }
        else
        {
            // Desactivar animación de natación
            animator.SetBool("isSwimming", false);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        float currentSpeed = isInWater ? waterMoveSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        // Comprobar si está en el suelo (solo cuando no está en agua)
        if (!isInWater)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // LO NUEVOOOOOOOOOOOO
    private IEnumerator DoAttack()
    {
        animator.SetBool("isFighting", true);

        // Aquí aplicas el daño (puedes poner un pequeño delay si quieres que coincida con el frame del golpe)
        Attack();

        yield return new WaitForSeconds(0.4f); // duración de la animación

        animator.SetBool("isFighting", false);
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                GoblinAI goblin = enemy.GetComponent<GoblinAI>();
                if (goblin != null)
                {
                    goblin.ReceiveDamage(attackDamage);
                }
            }
        }
    }

    // TERMINA LO NUEVOOOOOOOOOOO



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Underwater"))
        {
            isInWater = true;
            rb.gravityScale = waterGravity;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Underwater"))
        {
            isInWater = false;
            rb.gravityScale = normalGravity;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
    