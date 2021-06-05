using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;

    public TextMeshProUGUI BestScore_T;

    private void Start()
    {
        BestScore_T.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }


    public void ClearBestScore()
    {
        BestScore_T.text = "0";
        PlayerPrefs.SetInt("BestScore", 0);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
