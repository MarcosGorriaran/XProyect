using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;
    private AudioSource audioSource;

    [SerializeField] private string[] muteScenes; // Escenas donde la música no debe sonar

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (audioSource == null) return;

        // Verificar si la escena está en la lista de muteo
        bool shouldMute = false;
        foreach (var muteScene in muteScenes)
        {
            if (scene.name == muteScene)
            {
                shouldMute = true;
                break;
            }
        }

        audioSource.mute = shouldMute;
    }
}
