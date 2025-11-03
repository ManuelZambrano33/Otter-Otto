using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GoblinAI : MonoBehaviour
{
    [Header("Stats")]
    public int vidaGoblin = 3;
    public int danioGoblin = 1;
    public float moveSpeed = 2f;

    [Header("Detección")]
    public float rangoVision = 4f;
    public float rangoAtaque = 1f;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float tiempoEntreDanio = 3f;

    private Transform player;
    private Animator animator;
    private Vector2 posicionInicial;
    private float cooldownDanio = 0f;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        posicionInicial = transform.position; // Guardamos el punto de origen
    }

    void Update()
    {
        if (isDead) return;

        float distanciaJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaJugador <= rangoAtaque)
        {
            Atacar();
        }
        else if (distanciaJugador <= rangoVision)
        {
            Perseguir();
        }
        else
        {
            VolverOrigen();
        }

        if (cooldownDanio > 0f)
            cooldownDanio -= Time.deltaTime;
    }

    void Perseguir()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttack", false);

        // Solo mover en X, mantener Y fija
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Flip sprite
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void VolverOrigen()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttack", false);

        // Solo mover en X, mantener Y fija
        Vector2 target = new Vector2(posicionInicial.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            animator.SetBool("isWalking", false);
        }
    }

    void Atacar()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);

        if (cooldownDanio <= 0f)
        {
            ApplyDamage();
            cooldownDanio = tiempoEntreDanio;
        }
    }

    void ApplyDamage()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D col in hitObjects)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.PerderVida(); // usa el mismo sistema de corazones
            }
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (isDead) return;

        vidaGoblin -= damage;
        Debug.Log("Goblin recibió daño. Vida restante: " + vidaGoblin);

        if (vidaGoblin <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", false);
        Destroy(gameObject, 0.6f);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
