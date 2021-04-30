using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject PauseBTN;
    public void Start()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void PauseButton()
    {
        Time.timeScale = 0.01f;
        PausePanel.SetActive(true);
        PauseBTN.SetActive(false);
    }
    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        PauseBTN.SetActive(true);
    }
    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }

}
