using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip startFirst, startAfter;
    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            audioSource.PlayOneShot(startFirst);
            PlayerPrefs.SetInt("FirstTime", 0);
        }
        else
        {
            audioSource.PlayOneShot(startAfter);
        }

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        else if (Input.anyKeyDown && !audioSource.isPlaying)
        {
            LoadScene("Topic");
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
