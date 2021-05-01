using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject ExitPanel;
    public bool ispausemenu;
    public void Start()
    {
        PausePanel.SetActive(false);
        ExitPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispausemenu == false)
            {
                ispausemenu = true;
                Time.timeScale = 0.01f;
                PausePanel.SetActive(true);
            }
            else
            {
                ispausemenu = false;
                Time.timeScale = 1f;
                PausePanel.SetActive(false);
                ExitPanel.SetActive(false);
            }
        }
    }
    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        ispausemenu = false;
    }
    public void HomeButton()
    {
        ExitPanel.SetActive(true);
    }
    public void No()
    {
        ExitPanel.SetActive(false);
    }
    public void Yes()
    {
        SceneManager.LoadScene(0);
    }

}
