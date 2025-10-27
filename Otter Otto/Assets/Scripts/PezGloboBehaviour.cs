using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PezGloboBehaviour : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Segundos que tarda en explotar después de encogerse")]
    public float tiempoParaExplosion = 5f;

    [Tooltip("Tiempo que el jugador debe permanecer dentro del rango para recibir daño")]
    public float tiempoParaDanio = 0.8f;

    private Animator animator;
    private AudioSource audioSource;
    private Coroutine explosionCoroutine;
    private bool jugadorCerca = false;
    private bool yaExplotado = false; // Para evitar que se repita la explosión

    private float tiempoJugadorDentro = 0f; // Temporizador para daño

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Lógica de daño por permanencia
        if (jugadorCerca && !yaExplotado)
        {
            tiempoJugadorDentro += Time.deltaTime;

            if (tiempoJugadorDentro >= tiempoParaDanio)
            {
                // Aplica daño
                GameManager.Instance.PerderVida();

                // Reinicia el temporizador si quieres que pueda aplicar daño nuevamente
                tiempoJugadorDentro = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !yaExplotado)
        {
            jugadorCerca = true;
            tiempoJugadorDentro = 0f; // Empieza a contar desde cero
            animator.SetBool("isDamaged", true);

            if (explosionCoroutine == null)
                explosionCoroutine = StartCoroutine(EsperarYExplotar());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !yaExplotado)
        {
            jugadorCerca = false;
            tiempoJugadorDentro = 0f; // Resetea temporizador si sale
            animator.SetBool("isDamaged", false);

            if (explosionCoroutine != null)
            {
                StopCoroutine(explosionCoroutine);
                explosionCoroutine = null;
            }
        }
    }

    IEnumerator EsperarYExplotar()
    {
        yield return new WaitForSeconds(tiempoParaExplosion);

        if (jugadorCerca && !yaExplotado)
        {
            yaExplotado = true;
            animator.SetBool("isExploding", true);

            if (audioSource != null)
                audioSource.Play();

            yield return new WaitForSeconds(1.2f);

            Destroy(gameObject);
        }

        explosionCoroutine = null;
    }
}
