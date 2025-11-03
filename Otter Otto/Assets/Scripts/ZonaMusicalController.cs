using UnityEngine;

public class ZonaMusicalController : MonoBehaviour
{
    [Header("Audio a reproducir")]
    public AudioSource musicaAmbiente;

    private void Start()
    {
        Debug.LogWarning("SCRIPT INICIADO");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("Player ENTRA a la zona");
        if (other.CompareTag("Player"))
            musicaAmbiente.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player SALE de la zona");
        if (other.CompareTag("Player"))
            musicaAmbiente.Stop();
    }
}
