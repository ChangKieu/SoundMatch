using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectTopic : MonoBehaviour
{
    [SerializeField] private Button[] topics;
    [SerializeField] private int scenes;
    [SerializeField] private int currentIndex;
    private void Start()
    {
        currentIndex = -1;
    }
    private void Update()
    {
        for (int i = 0; i < topics.Length; i++)
        {
            if (topics[i] != null)
            {
                if (i == currentIndex)
                {
                    topics[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                else
                {
                    topics[i].GetComponent<Image>().color = Color.white;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if(currentIndex<0) currentIndex= topics.Length - 1;
            topics[currentIndex].GetComponent<AudioSource>().Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if(currentIndex>=topics.Length) currentIndex = 0;
            topics[currentIndex].GetComponent<AudioSource>().Play();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(currentIndex==-1)
            {
                GetComponent<AudioSource>().Play();
            }
            else if (scenes == 0)
            {
                PlayerPrefs.SetInt("topic", currentIndex + 1);
                SceneManager.LoadScene("Level");
            }
            else if(scenes == 1)
            {
                PlayerPrefs.SetInt("level", currentIndex + 1);
                switch (PlayerPrefs.GetInt("topic", 1))
                {
                    case 1:
                        {
                            SceneManager.LoadScene("Number");
                            break;
                        }
                    case 2:
                        {
                            SceneManager.LoadScene("Animal");
                            break;
                        }
                    case 3:
                        {
                            SceneManager.LoadScene("Music");
                            break;
                        }
                    default:
                        break;
                }
                
            }
        }
    }
}
