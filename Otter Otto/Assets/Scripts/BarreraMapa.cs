using UnityEngine;

public class BarreraMapa : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        // Guardamos la posición inicial del jugador al inicio de la escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            startPosition = player.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Reiniciamos su posición
            other.transform.position = startPosition;

            // Opcional: resetear velocidad si tiene Rigidbody2D
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
