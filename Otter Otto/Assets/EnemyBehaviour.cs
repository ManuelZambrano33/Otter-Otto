using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float timeToDamage = 1f; // tiempo que el jugador debe permanecer en el rango
    private float playerTimer = 0f; // temporizador de permanencia del jugador
    private bool playerInside = false;

    private void Update()
    {
        if (playerInside)
        {
            // incrementa el tiempo que el jugador ha estado dentro
            playerTimer += Time.deltaTime;

            // si supera el tiempo requerido, aplica daño y reinicia temporizador
            if (playerTimer >= timeToDamage)
            {
                GameManager.Instance.PerderVida();
                playerTimer = 0f; // reinicia temporizador
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerTimer = 0f; // empieza a contar desde cero
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerTimer = 0f; // se resetea si el jugador sale antes
        }
    }
}
