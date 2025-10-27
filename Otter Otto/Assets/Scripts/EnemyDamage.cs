using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public float tiempoParaDanio = 0.8f;
    public float cantidadDanio = 1;
    public bool destruirDespuesDeDanio = true;

    private bool jugadorEnRango = false;
    private float tiempoDentro = 0f;
    private bool yaHizoDanio = false;

    public AudioClip musicaClip; // asigna aquí el clip de audio

    private void Update()
    {
        if (jugadorEnRango && !yaHizoDanio)
        {
            tiempoDentro += Time.deltaTime;

            if (tiempoDentro >= tiempoParaDanio)
            {
                // Aplica daño al jugador
                GameManager.Instance.PerderVida();

                yaHizoDanio = true;

                // Reproduce música aunque el objeto se destruya
                if (musicaClip != null)
                {
                    AudioSource.PlayClipAtPoint(musicaClip, transform.position);
                }

                // Destruye el enemigo si corresponde
                if (destruirDespuesDeDanio)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            tiempoDentro = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            tiempoDentro = 0f;
        }
    }
}
