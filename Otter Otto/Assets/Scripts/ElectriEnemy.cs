using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ElectricEnemy : MonoBehaviour
{
    [Header("Daño al jugador")]
    public float tiempoEntreDanio = 0.8f;

    [Header("Audio eléctrico")]
    public AudioSource audioElectricidad;

    private bool jugadorEnRango = false;
    private float cooldownDanio = 0f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (audioElectricidad != null)
        {
            audioElectricidad.loop = true;
            audioElectricidad.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (jugadorEnRango)
        {
            // ⚡ Daño constante
            cooldownDanio += Time.deltaTime;
            if (cooldownDanio >= tiempoEntreDanio)
            {
                GameManager.Instance.PerderVida();
                cooldownDanio = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            cooldownDanio = 0f;

            // Activar animación
            animator.SetBool("isDamaged", true);

            // Activar sonido
            if (audioElectricidad != null && !audioElectricidad.isPlaying)
                audioElectricidad.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;

            // Desactivar animación
            animator.SetBool("isDamaged", false);

            // Detener sonido
            if (audioElectricidad != null && audioElectricidad.isPlaying)
                audioElectricidad.Stop();
        }
    }
}
