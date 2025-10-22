using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PezGloboBehaviour : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Segundos que tarda en explotar después de encogerse")]
    public float tiempoParaExplosion = 5f;

    private Animator animator;
    private AudioSource audioSource;
    private Coroutine explosionCoroutine;
    private bool jugadorCerca = false;
    private bool yaExplotado = false; //Para evitar que se repita la explosión

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !yaExplotado)
        {
            jugadorCerca = true;
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
        // Esperar el tiempo antes de explotar
        yield return new WaitForSeconds(tiempoParaExplosion);

        // Solo explota si el jugador sigue cerca y no ha explotado antes
        if (jugadorCerca && !yaExplotado)
        {
            yaExplotado = true; // ✅ Marca que ya se activó la explosión
            animator.SetBool("isExploding", true);

            // Reproducir sonido de explosión
            if (audioSource != null)
                audioSource.Play();

            // Esperar un tiempo antes de destruir el objeto
            yield return new WaitForSeconds(1.2f);

            Destroy(gameObject);
        }

        explosionCoroutine = null; // Limpiar la referencia
    }
}
