using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoreManager : MonoBehaviour
{

    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public GameObject[] digits = new GameObject[4];
    public Sprite[] numbersLarge = new Sprite[10];
    public Sprite[] numbersSmall = new Sprite[10];

    public int highScore;
    public int curScore;

    //public AudioClip pointAudio;

    public GameObject[] gameOverScoreDigits = new GameObject[4];
    public GameObject[] gameOverHighScoreDigits = new GameObject[4];
    public Sprite[] medalSprites = new Sprite[5];
    public GameObject medalPlaceholder;

    public GameObject newHighscoreNote;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScore()
    {
        // Reset score
        curScore = 0;

        digits[3].SetActive(false);
        digits[2].SetActive(false);
        digits[1].SetActive(false);
        digits[0].GetComponent<Image>().sprite = numbersLarge[0];

        gameOverScoreDigits[3].SetActive(false);
        gameOverScoreDigits[2].SetActive(false);
        gameOverScoreDigits[1].SetActive(false);
        gameOverScoreDigits[0].GetComponent<Image>().sprite = numbersSmall[0];

        highScore = PlayerPrefs.GetInt("highscore", 0);
        newHighscoreNote.SetActive(false);
        medalPlaceholder.GetComponent<Image>().sprite = medalSprites[0];
    }

    public void IncrementScore()
    {
        curScore++;
        GetComponent<AudioSource>().Play();

        if (curScore > highScore)
        {
            highScore = curScore;
            newHighscoreNote.SetActive(true);
        }

        UpdateDigits(digits, curScore, numbersLarge);

        medalPlaceholder.GetComponent<Image>().sprite = medalSprites[Mathf.FloorToInt(Mathf.Min(curScore, 49) / 10)];
    }

    int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        return listOfInts.ToArray();
    }

    public void UpdateScorecard()
    {
        UpdateDigits(gameOverScoreDigits, curScore, numbersSmall);
        UpdateDigits(gameOverHighScoreDigits, highScore, numbersSmall);

        PlayerPrefs.SetInt("highscore", highScore);
        PlayerPrefs.Save();
    }


    public void UpdateDigits(GameObject[] digitSet, int score, Sprite[] numberSprites)
    {
        int[] scoreDigits = GetIntArray(score);

        int i = 0;
        foreach (int digit in scoreDigits)
        {
            digitSet[i].SetActive(true);
            digitSet[i].GetComponent<Image>().sprite = numberSprites[digit];
            i++;
        }
    }
    
}
