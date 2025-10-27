using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("PREFABS DE UI")]
    public GameObject menuPausaPrefab;
    public GameObject gameOverPrefab;

    // Instancias
    private GameObject menuPausaInstance;
    public Button botonPausa;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CrearInterfazUsuario();
    }

    void CrearInterfazUsuario()
    {
        // Instanciar men� pausa si el prefab est� asignado
        if (menuPausaPrefab != null)
        {
            menuPausaInstance = Instantiate(menuPausaPrefab);
            DontDestroyOnLoad(menuPausaInstance);
            menuPausaInstance.SetActive(false);
        }
    }

    public void MostrarMenuPausa()
    {
        if (menuPausaInstance != null)
        {
            menuPausaInstance.SetActive(true);
            Time.timeScale = 0f;
            if (botonPausa != null)
                botonPausa.gameObject.SetActive(false);
        }
    }

    public void OcultarMenuPausa()
    {
        if (menuPausaInstance != null)
        {
            menuPausaInstance.SetActive(false);
            Time.timeScale = 1f;
            if (botonPausa != null)
                botonPausa.gameObject.SetActive(true);
        }
    }

    // M�todo para configurar el bot�n de pausa desde cualquier escena
    public void ConfigurarBotonPausa(Button nuevoBoton)
    {
        botonPausa = nuevoBoton;
        if (botonPausa != null)
        {
            botonPausa.onClick.RemoveAllListeners();
            botonPausa.onClick.AddListener(MostrarMenuPausa);
        }
    }
}