using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cards, cardPrefab;
    [SerializeField] private GameObject cardContainer, firstCardFlip, panelPause, panelWin;
    [SerializeField] private int numbers, rows, colunms, currentIndex, flippedCount, firstCard;
    [SerializeField] private bool isGameOver, canMove, checkFirstCard, isGameStart;
    [SerializeField] private AudioClip[] startSound, moveSound, checkSound, winSound, levelSound;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        PlayerPrefs.SetInt("win", 1);
        audioSource = GetComponent<AudioSource>();
        switch(PlayerPrefs.GetInt("level",1))
        {
            case 1:
                {
                    numbers = 2;
                    rows = 2;
                    colunms = 2;
                    break;
                }
            case 2:
                {
                    numbers = 3;
                    rows = 2;
                    colunms = 3;
                    break;
                }
            case 3:
                {
                    numbers = 4;
                    rows = 2;
                    colunms = 4;
                    break;
                }
            default: 
                break;
        }
        StartCoroutine(GameStart());
        InitializeCards();
    }
    IEnumerator GameStart()
    {
        yield return null;
        for (int i = 0; i < startSound.Length; i++)
        {
            yield return PlaySound(startSound[i]);

            if (i == 0)
            {
                yield return PlaySound(levelSound[rows - 1]);
            }
            if (i == 1)
            {
                yield return PlaySound(levelSound[colunms - 1]);
            }
            if (i == 3)
            {
                for (int j = 0; j < numbers; j++)
                {
                    yield return PlaySound(cardPrefab[j].GetComponent<Card>().NameSound());
                }
            }
        }
        isGameStart = true;
    }
    void InitializeCards()
    {
        List<int> availableIndices = new List<int>();

        for (int i = 0; i < numbers; i++)
        {
            for(int j = 0; j < rows*colunms/numbers; j++)
            {
                availableIndices.Add(i);
            }
        }

        cards = new GameObject[rows * colunms];
        Vector3 containerPosition = cardContainer.transform.position;
        Vector3 containerSize = cardContainer.GetComponent<Renderer>().bounds.size;

        float rowSpacing = containerSize.y / (rows);
        float colSpacing = containerSize.x / (colunms);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colunms; j++)
            {
                int randomIndex = Random.Range(0, availableIndices.Count);
                int prefabIndex = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);

                float xPos = containerPosition.x - containerSize.x / 2 + (j + 0.5f) * colSpacing ;
                float yPos = containerPosition.y + containerSize.y / 2 - (i + 0.5f) * rowSpacing;

                cards[i * colunms + j] = Instantiate(cardPrefab[prefabIndex], new Vector3(xPos, yPos, 0), Quaternion.identity);
                cards[i * colunms + j].transform.parent = cardContainer.transform;
            }
        }
    }


    void Update()
    {
        if(isGameOver)
        {
            for(int i=0;i<winSound.Length;i++)
            {
                audioSource.PlayOneShot(winSound[i]);
            }
            panelWin.SetActive(true);
            PlayerPrefs.SetInt("win", 2);
            gameObject.GetComponent<CardManager>().enabled= false;
        }
        else if(!isGameOver && isGameStart && Time.timeScale != 0f)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i]!=null)
                {
                    if (i == currentIndex)
                    {
                        cards[i].GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1f);
                    }
                    else
                    {
                        cards[i].GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveToCard(0, -1);
                if (canMove)
                {
                    PlayMoveSound(0); 
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveToCard(0, 1);
                if (canMove)
                {
                    PlayMoveSound(1);
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToCard(-1, 0);
                if (canMove)
                {
                    PlayMoveSound(2);
                }

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToCard(1, 0);
                if (canMove)
                {
                    PlayMoveSound(3); 
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                FlipCard(cards[currentIndex]);
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 0f;
                panelPause.SetActive(true);
            }
        }
    }
    void PlayMoveSound(int direction)
    {
        if (direction >= 0 && direction < moveSound.Length)
        {
            audioSource.clip = moveSound[direction];
            audioSource.Play();
        }
    }
    void MoveToCard(int xOffset, int yOffset)
    {
        canMove = true;
        int currentRow = currentIndex / colunms;
        int currentCol = currentIndex % colunms;

        int newRow = currentRow + yOffset;
        int newCol = currentCol + xOffset;

        while (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < colunms && cards[newRow * colunms + newCol] == null)
        {
            newRow += (yOffset != 0) ? (int) Mathf.Sign(yOffset) : 0; 
            newCol += (xOffset != 0) ? (int) Mathf.Sign(xOffset) : 0;
        }

        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= colunms || cards[newRow * colunms + newCol] == null)
        {
            canMove = false;
            bool foundCardNearby = false;

            if (currentRow > 0 && cards[(currentRow - 1) * colunms + currentCol] != null)
            {
                foundCardNearby = true;
            }
            if (currentRow < rows - 1 && cards[(currentRow + 1) * colunms + currentCol] != null)
            {
                foundCardNearby = true;
            }
            if (currentCol > 0 && cards[currentRow * colunms + (currentCol - 1)] != null)
            {
                foundCardNearby = true;
            }
            if (currentCol < colunms - 1 && cards[currentRow * colunms + (currentCol + 1)] != null)
            {
                foundCardNearby = true;
            }

            if (!foundCardNearby)
            {
                for (int i = 0; i < rows * colunms; i++)
                {
                    if (cards[i] != null && i != currentIndex)
                    {
                        currentIndex = i;
                        int Row = currentIndex / colunms;
                        int Col = currentIndex % colunms;
                        Debug.Log(Row);
                        Debug.Log(Col);
                        StartCoroutine(TransitionSound(Row, Col));
                        return;
                    }
                }
            }

            return;
        }
        
        currentIndex = newRow * colunms + newCol;
    }
    IEnumerator TransitionSound(int row, int colunm)
    {
        yield return PlaySound(startSound[1]);
        yield return PlaySound(levelSound[row]);
        yield return PlaySound(startSound[2]);
        yield return PlaySound(levelSound[colunm]);
    }

    IEnumerator PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
    }
    void FlipCard(GameObject card)
    {
        
        if (!card.GetComponent<Card>().IsFlipped)
        {
            card.GetComponent<Card>().Flip();
            if (!checkFirstCard)
            {
                firstCard = currentIndex;
                checkFirstCard = true;
            }
            else
                CheckMatch(card);
        }
    }
    void CheckMatch(GameObject card)
    {
        bool matchFound = false;
        foreach (GameObject otherCard in cards)
        {
            if(otherCard!=null)
            {
                if (otherCard != card && otherCard.GetComponent<Card>().IsFlipped)
                {
                    if (otherCard.gameObject.name == card.gameObject.name)
                    {
                        Destroy(card);
                        Destroy(otherCard);
                        flippedCount += 2;
                        matchFound = true;
                        checkFirstCard = false;
                        StartCoroutine(PlayCheckSound(0));
                        if (flippedCount == rows*colunms)
                        {
                            isGameOver = true;
                        }
                    }
                    else
                    {
                        matchFound = false;
                    }
                }
            }
        }
        if (!matchFound)
        {
            StartCoroutine(PlayCheckSound(1));
            StartCoroutine(FlipBack(card));
        }
    }
    IEnumerator FlipBack(GameObject card)
    {
        yield return new WaitForSeconds(1f); 
        card.GetComponent<Card>().Flip();
        if(cards[firstCard]!=null)
            cards[firstCard].GetComponent<Card>().Flip();
        checkFirstCard = false;
        firstCard = -1;
    }
    IEnumerator PlayCheckSound(int direction)
    {
        yield return new WaitForSeconds(0.7f);
        if (direction >= 0 && direction < checkSound.Length)
        {
            audioSource.clip = checkSound[direction];
            audioSource.Play();
        }
    }
}
