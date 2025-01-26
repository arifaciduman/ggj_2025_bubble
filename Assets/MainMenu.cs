using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPanel;
    public GameObject creditsPanel;
    public GameObject mainPanel;

    public VideoPlayer videoPlayer;

    public GameObject cinematicParent;
    public GameObject blackImage;

    public CanvasGroup canvasGroup;

    public void PlayButton()
    {
        blackImage.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        void DelayCinematic()
        {
            cinematicParent.SetActive(true);
            videoPlayer.playbackSpeed = 1;
            videoPlayer.Play();
            System.Action action = () => SceneManager.LoadScene(1);
            DelayUtility.ExecuteAfterSeconds(action, (float)videoPlayer.clip.length, true);
        }
        DelayUtility.ExecuteAfterFrames(DelayCinematic, 3, true);
    }

    public void HowTo()
    {
        howToPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void QuitHowto()
    {
        howToPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void QuitCredits()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
