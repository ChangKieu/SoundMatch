using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ControlScenes : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private int currentIndex;
    [SerializeField] GameObject panelPause;
    private void Start()
    {
        currentIndex = -1;
    }
    private void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                if (i == currentIndex)
                {
                    buttons[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                else
                {
                    buttons[i].GetComponent<Image>().color = Color.white;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = buttons.Length - 1;
            buttons[currentIndex].GetComponent<AudioSource>().Play();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= buttons.Length) currentIndex = 0;
            buttons[currentIndex].GetComponent<AudioSource>().Play();
        }


        else if(Input.GetKeyDown(KeyCode.Return))
        {
            if (currentIndex == -1)
            {
                GetComponent<AudioSource>().Play();
            }
            else if(currentIndex== 0)
            {
                if (PlayerPrefs.GetInt("win", 1) == 1)
                {
                     Resume();
                }
                else
                {
                    LoadScene("Topic");
                } 
                    
            }
            
            else if(currentIndex== 1)
            {
                Reload();
            }
            else if (currentIndex == 2)
            {
                LoadScene("Menu");
            }
        }
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        panelPause.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        panelPause.SetActive(true);
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}