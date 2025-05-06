using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Logic : MonoBehaviour
{
    #region Timer
    [Header("Timer")]
    [SerializeField]
    private TextMeshProUGUI _timer;
    #endregion

    #region Objectives
    [Header("Objectives")]
    [SerializeField]
    private TextMeshProUGUI _objectives;
    [SerializeField]
    private float _policeCallingDuration;

    private float _policeCallingTimer;

    private bool _policeIsCalled;
    #endregion

    #region Game Over
   // [Header("Game Over")]

    #endregion
    void Start()
    {
        General_Game._timer = 60;
        General_Game._policeTimer = 60;
        General_Game.CurrentDoneObjectives = 0;
        General_Game.ObjectivesCount = 5;
        General_Game.ObjectiveRadius = 2;

        General_Game.IsHidden = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_policeIsCalled)
        {
            CountTimer();
            ObjectivesLeft();
            GameOver();
        }

        if (_policeCallingTimer <= _policeCallingDuration)
        {
            SetTimer();
        }

            CallPolice();


        if(_policeIsCalled)
        {
            CompletedGame();
        }
    }

    private void CountTimer()
    {
        General_Game._timer -= Time.deltaTime;
    }

    private void SetTimer()
    {
        _timer.text = $"Time left: {(int)General_Game._timer}";
    }

    private void ObjectivesLeft()
    {
        _objectives.text = $"Objectives done: {General_Game.ObjectivesCount - General_Game.CurrentDoneObjectives}";
    }

    private void GameOver()
    {
        if (General_Game._timer <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void CallPolice()
    {
        if(General_Game.CurrentDoneObjectives == General_Game.ObjectivesCount)
        {
            _objectives.faceColor = Color.green;
            _objectives.text = $"Call police: Hold F";
        }

        if(Input.GetKey(KeyCode.F))
        {
            _policeCallingTimer += Time.deltaTime;
            _policeIsCalled = true;
        }



        if(_policeCallingTimer >= _policeCallingDuration)
        {
            _timer.color = Color.green;
            _timer.text = $"Time left until police arrives: {(int)General_Game._policeTimer}";
            General_Game._policeTimer -= Time.deltaTime;
        }

       
    }
    private void CompletedGame()
    {
        if (General_Game._policeTimer <= 0)
        {
            SceneManager.LoadScene("GameWon");
        }
    }
}
