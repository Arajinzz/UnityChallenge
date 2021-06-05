using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverChanger : MonoBehaviour
{

    public Animator animator;

    public TextMeshProUGUI Score_T;
    public TextMeshProUGUI BestScore_T;

    private void Start()
    {
        Score_T.text = PlayerPrefs.GetInt("Score", 0).ToString();
        BestScore_T.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Fade()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
