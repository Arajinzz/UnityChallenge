using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


// Handle Score Calculation And win lose condition

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI Score_T;
    public TextMeshProUGUI BestScore_T;

    private int Score;

    private bool GameEnd;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        GameEnd = false;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameEnd)
        {
            Score_T.text = Score.ToString();

            if (Score < 0)
            {
                StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>().UniqueDestroy());
                Score = 0;
                GameEnd = true;
            }

            PlayerPrefs.SetInt("Score", Score);

            int ScoreDifference = Score - PlayerPrefs.GetInt("BestScore");

            if (ScoreDifference > 0)
            {
                BestScore_T.text = Score.ToString();
            }
            else
            {
                BestScore_T.text = PlayerPrefs.GetInt("BestScore").ToString();
            }

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (Enemies.Length <= 0)
            {
                // Win
                if (Score > PlayerPrefs.GetInt("BestScore"))
                {
                    PlayerPrefs.SetInt("BestScore", Score);
                }

                GameEnd = true;
            }


        }
        else
        {
            Time.timeScale = 1f;
            StartCoroutine(TransitionToNextScene());
        }
        


        
    }


    IEnumerator TransitionToNextScene()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void UpdateScore(int value)
    {
        Score += value;
    }

}
