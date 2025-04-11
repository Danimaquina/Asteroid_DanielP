using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelAdvancePanel : ActiveOnlyDuringSomeGameStates
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI InfoText; 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void DetermineActive()
    {
        base.DetermineActive();
        if (AsteraX.GAME_STATE == AsteraX.eGameState.transition) {
            AsteraX.CaracterOfLevel(AsteraX.level);
            gameObject.SetActive(true);
            levelText.text = "Level: " + AsteraX.level.ToString();
            InfoText.text = "Asteroids: " + AsteraX.numAsteroidesPadres.ToString() + " / " + "Children: " + AsteraX.numAsteroidesHijos.ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
