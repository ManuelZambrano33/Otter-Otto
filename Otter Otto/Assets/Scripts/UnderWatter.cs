using UnityEngine;

public class UnderWatter : MonoBehaviour
{
    [Header("Audio a reproducir")]
    public AudioSource musica;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica que sea el jugador
        if (other.CompareTag("Player"))
        {
            if (musica != null && !musica.isPlaying)
            {
                musica.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el jugador salga, detener la música
        if (other.CompareTag("Player"))
        {
            if (musica != null && musica.isPlaying)
            {
                musica.Stop();
            }
        }
    }
}
