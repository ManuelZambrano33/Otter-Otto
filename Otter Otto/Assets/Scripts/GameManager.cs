using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefab del HUD")]
    public HUD hudPrefab;       // Asigna el prefab del HUD desde el inspector
    private HUD hudInstance;    // Instancia persistente del HUD

    [Header("Vidas iniciales")]
    [SerializeField] private int maxVidas = 3;
    private int vidas;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializamos vidas
        vidas = maxVidas;

        // Instanciamos HUD persistente
        if (hudInstance == null && hudPrefab != null)
        {
            hudInstance = Instantiate(hudPrefab);
            DontDestroyOnLoad(hudInstance.gameObject);
        }

        // Sincronizamos HUD
        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cada vez que se carga la escena, aseguramos que el HUD muestre las vidas actuales
        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);
    }

    // Llamar cuando el jugador pierde una vida
    public void PerderVida()
    {
        vidas--;

        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        // Si no quedan vidas, recargamos la escena
        if (vidas <= 0)
        {
            vidas = maxVidas;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Llamar cuando el jugador recoge vida
    // Devuelve true si se recuperó vida, false si ya estaba al máximo
    public bool RecuperarVida()
    {
        if (vidas >= maxVidas)
            return false; // No se puede recuperar

        vidas++; // Incrementamos la vida

        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        return true; // Vida recuperada
    }

    // Obtener vidas actuales
    public int GetVidas()
    {
        return vidas;
    }
}
