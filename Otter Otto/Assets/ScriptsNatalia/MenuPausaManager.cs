using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausaManager : MonoBehaviour
{
    [Header("BOTONES - Arrastra aqu� tus botones")]
    public Button botonPausa;        // Bot�n para ABRIR el men� pausa
    public Button botonReanudar;     // Bot�n para CERRAR el men� pausa
    public Button botonMenuPrincipal;

    [Header("PANELES")]
    public GameObject panelPausa;    // El panel completo del men� de pausa

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

        // Asegurar que el panel de pausa est� oculto al inicio
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    void Update()
    {
        // Tecla ESC tambi�n funciona para pausar/reanudar
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

        // Ocultar el bot�n de pausa cuando el men� est� abierto
        if (botonPausa != null)
            botonPausa.gameObject.SetActive(false);
    }

    public void ReanudarJuego()
    {
        PlaySonidoClic();
        Time.timeScale = 1f; // Reanudar tiempo normal
        if (panelPausa != null)
            panelPausa.SetActive(false);

        // Mostrar el bot�n de pausa cuando el men� est� cerrado
        if (botonPausa != null)
            botonPausa.gameObject.SetActive(true);
    }

    public void IrAlMenuPrincipal()
    {
        PlaySonidoClic();
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        Debug.Log("Volviendo al men� principal...");
        // SceneManager.LoadScene("MenuPrincipal"); // Descomenta cuando tengas la escena
    }

    void PlaySonidoClic()
    {
        if (audioSource != null && sonidoClic != null)
            audioSource.PlayOneShot(sonidoClic);
    }
}