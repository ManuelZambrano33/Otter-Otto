using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausaManager : MonoBehaviour
{
    [Header("REFERENCIAS")]
    public Button botonReanudar;
    public Button botonMenuPrincipal;

    [Header("SONIDOS")]
    public AudioClip sonidoClic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Configurar listeners
        botonReanudar.onClick.AddListener(ReanudarJuego);
        botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);

        // Hacer este objeto persistente
        DontDestroyOnLoad(gameObject);
    }

    void ReanudarJuego()
    {
        PlaySonidoClic();
        GameManager.Instance.OcultarMenuPausa();
    }


    void IrAlMenuPrincipal()
    {
        PlaySonidoClic();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
        // Ocultar este menú cuando volvamos al menú principal
        gameObject.SetActive(false);
    }

    void PlaySonidoClic()
    {
        if (audioSource != null && sonidoClic != null)
            audioSource.PlayOneShot(sonidoClic);
    }
}