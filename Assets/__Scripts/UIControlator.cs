using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIControlator : MonoBehaviour
{
    public GameObject GameOver;
    public TextMeshProUGUI FinalScoreText;
    public GameObject GameUI;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI JumpsText;
    public int Jumps;
    private int Score = 0;
    public int value;
    private PlayerShip playerShip;
    


    
    // Start is called before the first frame update
    void Start()
    {
        GameOver.SetActive(false);
        GameUI.SetActive(true);
        ScoreText.text = Score.ToString();
        JumpsText.text = "Jumps: " + Jumps;
        FinalScoreText.text = "Final Score: " + 0.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        FinalScoreText.text = "Final Score: " + Score;
    }

    public void AugmentScore()
    {
        Debug.Log("Le he dado!!");
        Score += value;
        ScoreText.text = Score.ToString();

    }
    
    public void Hit()
    {
        Debug.Log("He chocado");
        Jumps -= 1;
        JumpsText.text = "Jumps: " + Jumps;

        if (Jumps <= 0)
        {
            GameOver.SetActive(true);
            GameUI.SetActive(false);
            FinalScoreText.text = Score.ToString();
            Invoke("Reiniciar", 5);
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reiniciar la escena
    }
}
