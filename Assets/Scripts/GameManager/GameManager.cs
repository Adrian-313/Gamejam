using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public ManagementData managementData; // Referencia a los datos de configuración
    public Animator OpenCloseScene; // Controla las animaciones de transición
    public GameObject panelCongratulation; // Panel de victoria
    public GameObject panelGameOver; // Panel de derrota
    public Text timerText; // Referencia al texto del reloj
    public float gameDuration = 60f; // Duración del juego en segundos

    private float timer; // Temporizador interno
    private bool gameEnded; // Bandera para verificar si el juego ha terminado

    public void Start()
    {
        // Configuración inicial del juego
        managementData.SetAudioMixerData();
        timer = gameDuration;
        gameEnded = false;

        // Desactiva los paneles de victoria y derrota
        panelCongratulation.SetActive(false);
        panelGameOver.SetActive(false);

        // Inicializa el texto del reloj
        if (timerText != null)
        {
            timerText.text = FormatTime(timer);
        }
    }

    public void Update()
    {
        if (!gameEnded)
        {
            timer -= Time.deltaTime; // Reduce el tiempo restante

            // Actualiza el texto del reloj
            if (timerText != null)
            {
                timerText.text = FormatTime(timer);
            }

            // Verifica si el jugador ha ganado
            if (timer <= 0)
            {
                PlayerWins();
            }
        }
    }

    public void PlayerCaught()
    {
        if (!gameEnded)
        {
            PlayerLoses();
        }
    }

    private void PlayerWins()
    {
        gameEnded = true; // Marca el juego como terminado
        panelCongratulation.SetActive(true); // Muestra el panel de victoria
        Debug.Log("¡Has ganado!");
        StartCoroutine(ChangeScene(TypeScene.HomeScene)); // Cambia a la escena principal
    }

    private void PlayerLoses()
    {
        gameEnded = true; // Marca el juego como terminado
        panelGameOver.SetActive(true); // Muestra el panel de derrota
        Debug.Log("Has perdido.");
        StartCoroutine(ChangeScene(TypeScene.HomeScene)); // Cambia a la escena principal
    }

    public void ChangeSceneSelector(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.HomeScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.OptionsScene:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case TypeScene.GameScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.Exit:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
        }
    }

    public IEnumerator FadeIn()
    {
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        float currentVolumen = 0;
        float volume = 0;
        if (ManagementData.audioMixer.GetFloat(ManagementOptions.TypeSound.Master.ToString(), out volume))
        {
            currentVolumen = volume;
        }
        else
        {
            currentVolumen = -80f;
        }
        while (currentVolumen < decibelsMaster)
        {
            if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
            currentVolumen++;
            ManagementData.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), currentVolumen);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public IEnumerator ChangeScene(TypeScene typeScene)
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(2);
        if (typeScene != TypeScene.Exit)
        {
            SceneManager.LoadScene(typeScene.ToString());
        }
        else
        {
            Application.Quit();
        }
        OpenCloseScene.SetBool("Out", false);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        while (decibelsMaster > -80)
        {
            if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
            decibelsMaster -= 1;
            ManagementData.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), decibelsMaster);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public void PlayASound(AudioClip audioClip)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }

    public void PlayASound(AudioClip audioClip, float initialRandomPitch)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.pitch = Random.Range(initialRandomPitch - 0.1f, initialRandomPitch + 0.1f);
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }

    internal void SetAudioMixerData()
    {
        managementData.SetAudioMixerData();
    }

    // Formatea el tiempo en minutos y segundos (MM:SS)
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public enum TypeScene
    {
        HomeScene = 0,
        OptionsScene = 1,
        GameScene = 2,
        Exit = 3
    }
}
