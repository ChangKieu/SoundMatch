using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isFlipped = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip nameSound, flipSound;
    [SerializeField] private TextMeshPro nameCard;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nameCard.gameObject.SetActive(false);
    }

    public void Flip()
    {
        if (!isFlipped)
        {
            nameCard.gameObject.SetActive(true);
            isFlipped = true;
            StartCoroutine(PlaySound());
        }
        else
        {
            nameCard.gameObject.SetActive(false);
            isFlipped = false;
        }
    }
    IEnumerator PlaySound()
    {
        yield return null;
        audioSource.PlayOneShot(flipSound);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.PlayOneShot(nameSound);
        
    }
    public AudioClip NameSound()
    {
        return nameSound;
    }
    public bool IsFlipped
    {
        get { return isFlipped; }
    }


}
