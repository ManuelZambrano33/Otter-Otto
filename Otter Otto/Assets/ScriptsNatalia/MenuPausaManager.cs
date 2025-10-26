using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausaManager : MonoBehaviour
{
    [Header("BOTONES - Arrastra aquí tus botones")]
    public Button botonPausa;        // Botón para ABRIR el menú pausa
    public Button botonReanudar;     // Botón para CERRAR el menú pausa
    public Button botonMenuPrincipal;

    [Header("PANELES")]
    public GameObject panelPausa;    // El panel completo del menú de pausa

    [Header("SONIDOS")]
    public AudioClip sonidoClic;
    private AudioSource audioSource;

    void Start()
    {
        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Configurar listeners de botones
        botonPausa.onClick.AddListener(PausarJuego);
        botonReanudar.onClick.AddListener(ReanudarJuego);
        botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);

        // Asegurar que el panel de pausa está oculto al inicio
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    void Update()
    {
        // Tecla ESC también funciona para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
                ReanudarJuego();
            else
                PausarJuego();
        }
    }

    public void PausarJuego()
    {
        PlaySonidoClic();
        Time.timeScale = 0f; // Pausar el tiempo del juego
        if (panelPausa != null)
            panelPausa.SetActive(true);

        // Ocultar el botón de pausa cuando el menú está abierto
        if (botonPausa != null)
            botonPausa.gameObject.SetActive(false);
    }

    public void ReanudarJuego()
    {
        PlaySonidoClic();
        Time.timeScale = 1f; // Reanudar tiempo normal
        if (panelPausa != null)
            panelPausa.SetActive(false);

        // Mostrar el botón de pausa cuando el menú está cerrado
        if (botonPausa != null)
            botonPausa.gameObject.SetActive(true);
    }

    public void IrAlMenuPrincipal()
    {
        PlaySonidoClic();
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        Debug.Log("Volviendo al menú principal...");
        // SceneManager.LoadScene("MenuPrincipal"); // Descomenta cuando tengas la escena
    }

    void PlaySonidoClic()
    {
        if (audioSource != null && sonidoClic != null)
            audioSource.PlayOneShot(sonidoClic);
    }
}