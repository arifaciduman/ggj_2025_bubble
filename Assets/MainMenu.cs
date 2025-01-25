using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPanel;
    public GameObject creditsPanel;
    public GameObject mainPanel;
    
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
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
