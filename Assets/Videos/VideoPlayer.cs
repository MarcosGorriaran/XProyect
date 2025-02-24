using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public VideoClip[] videoClips; // Lista de videos disponibles

    void Start()
    {
        string selectedVideoName = PlayerPrefs.GetString("SelectedVideo", "");
        PlayVideo(selectedVideoName);
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayVideo(string videoName)
    {
        VideoClip clip = videoClips.FirstOrDefault(v => v.name == videoName);
        if (clip != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();

        }
        else
        {
            Debug.LogError("No se encontró un video con el nombre: " + videoName);
        }
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("El video ha terminado. Cargando la siguiente escena...");
        SceneManager.LoadScene("Lobby");
    }



}
