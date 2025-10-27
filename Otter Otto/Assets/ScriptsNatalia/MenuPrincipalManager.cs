using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public static MenuPrincipalManager Instance;

    [Header("BOTONES MENÚ PRINCIPAL")]
    public Button botonJugar;
    public Button botonSalir;

    [Header("CONFIGURACIÓN ESCENAS")]
    public string nombreEscenaJuego = "Nivel1"; // Cambia por tu escena

    [Header("REFERENCIAS")]
    public SoundManager soundManager;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("MenuPrincipalManager iniciado - Persistente");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Configurar botones
        if (botonJugar != null)
        {
            botonJugar.onClick.RemoveAllListeners();
            botonJugar.onClick.AddListener(Jugar);
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.RemoveAllListeners();
            botonSalir.onClick.AddListener(Salir);
        }

        // Buscar SoundManager si no está asignado
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }

        // Mostrar menú al inicio
        MostrarMenu();

        Debug.Log("Menú Principal configurado. Escena de juego: " + nombreEscenaJuego);
    }

    public void Jugar()
    {
        // Reproducir sonido de clic
        ReproducirSonidoClic();

        Debug.Log("Cargando escena: " + nombreEscenaJuego);

        // Cargar la escena de juego
        SceneManager.LoadScene(nombreEscenaJuego);

        // Ocultar el menú cuando se inicia el juego
        OcultarMenu();
    }

    public void Salir()
    {
        // Reproducir sonido de clic
        ReproducirSonidoClic();

        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Método para mostrar el menú (cuando se vuelve desde game over)
    public void MostrarMenu()
    {
        if (botonJugar != null) botonJugar.gameObject.SetActive(true);
        if (botonSalir != null) botonSalir.gameObject.SetActive(true);

        gameObject.SetActive(true);
        Debug.Log("Menú Principal mostrado");
    }

    // Método para ocultar el menú (cuando se inicia el juego)
    public void OcultarMenu()
    {
        gameObject.SetActive(false);
        Debug.Log("Menú Principal ocultado");
    }

    // Método llamado cuando se vuelve desde game over u otras escenas
    public void VolverAlMenuPrincipal()
    {
        MostrarMenu();

        // Asegurarse de que el tiempo sea normal
        Time.timeScale = 1f;
    }

    // Método para cambiar la escena de juego dinámicamente
    public void CambiarEscenaJuego(string nuevaEscena)
    {
        nombreEscenaJuego = nuevaEscena;
        Debug.Log("Escena de juego cambiada a: " + nuevaEscena);
    }

    void ReproducirSonidoClic()
    {
        // Usar el SoundManager si está disponible y el sonido está activado
        if (soundManager != null && soundManager.IsSoundEnabled())
        {
            // Aquí podrías reproducir un sonido de clic específico
            // soundManager.PlayOneShot(sonidoClic);
        }
    }

    // Método para verificar si el menú está activo
    public bool IsMenuActivo()
    {
        return gameObject.activeInHierarchy;
    }
}