using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class VerticalPatrolByRange : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento vertical (unidades/segundo)")]
    public float speed = 2f;

    [Tooltip("Altura máxima (en unidades locales) desde el punto inicial")]
    public float rangoArriba = 2f;

    [Tooltip("Altura mínima (en unidades locales) desde el punto inicial")]
    public float rangoAbajo = 2f;

    [Tooltip("Si true empieza moviéndose hacia arriba; si false hacia abajo")]
    public bool startUp = true;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isDamaging = false;
    private int direction = 1;
    private float limiteSuperior;
    private float limiteInferior;
    private float posInicialY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        posInicialY = transform.position.y;
        limiteSuperior = posInicialY + rangoArriba;
        limiteInferior = posInicialY - rangoAbajo;
        direction = startUp ? 1 : -1;
    }

    void Update()
    {
        if (isDamaging)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //Movimiento continuo
        rb.velocity = new Vector2(rb.velocity.x, direction * speed);

        //Cambiar dirección al llegar a los límites
        if (transform.position.y >= limiteSuperior && direction > 0)
        {
            direction = -1;
            rb.velocity = Vector2.zero;
        }
        else if (transform.position.y <= limiteInferior && direction < 0)
        {
            direction = 1;
            rb.velocity = Vector2.zero;
        }
    }

    //Cuando el jugador entra al trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDamaging = true;
            rb.velocity = Vector2.zero;
            animator.SetBool("isDamaged", true);
        }
    }

    //Cuando el jugador sale del trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDamaging = false;
            animator.SetBool("isDamaged", false);
        }
    }

    // Gizmos para visualizar el rango
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        float y0 = Application.isPlaying ? posInicialY : transform.position.y;
        float yTop = y0 + rangoArriba;
        float yBottom = y0 - rangoAbajo;

        Gizmos.DrawLine(new Vector3(transform.position.x, yTop, 0),
                        new Vector3(transform.position.x, yBottom, 0));
        Gizmos.DrawSphere(new Vector3(transform.position.x, yTop, 0), 0.05f);
        Gizmos.DrawSphere(new Vector3(transform.position.x, yBottom, 0), 0.05f);
    }
}
