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

    #region
    [Header("Inventory")]
    [SerializeField]
    private TextMeshProUGUI _keys;
    [SerializeField]
    private TextMeshProUGUI _beartraps;
    #endregion

    #region Game Over
    // [Header("Game Over")]

    #endregion

    #region Progress bar
    [Header("ProgressBar")]
    [SerializeField]
    private Slider _progressBarPrefab;

    [SerializeField]
    private Canvas _screenCanvas;

    private Slider _prgressBarInstantiated;
    #endregion
    void Start()
    {
        General_Game.Timer = 60;
        General_Game.PoliceTimer = 60;
        General_Game.CurrentDoneObjectives = 0;
        General_Game.ObjectivesCount = 5;
        General_Game.ObjectiveRadius = 2;
        General_Game.PoliceCalledThievesMultiplier = 2f;
        General_Game.KeyCount = 0;
        General_Game.StunItemCount = 0;

        General_Game.IsHidden = false;
        General_Game.IsPoliceCalled = false;

        _prgressBarInstantiated = Instantiate(_progressBarPrefab);
        _prgressBarInstantiated.transform.position += new Vector3(_screenCanvas.renderingDisplaySize.x, 0, 0);
        _prgressBarInstantiated.transform.SetParent(_screenCanvas.transform);
        _prgressBarInstantiated.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_policeIsCalled)
        {
            ObjectivesLeft();

        }

        if (_policeCallingTimer <= _policeCallingDuration)
        {
            CountTimer();
            SetTimer();
            GameOver();
        }

            CallPolice();


        if(_policeIsCalled)
        {
            CompletedGame();
        }

        InventoryDisplay();
    }

    private void InventoryDisplay()
    {
        _keys.text = $"Keys: {General_Game.KeyCount}";
        _beartraps.text = $"Bear traps: {General_Game.StunItemCount}";
    }

    private void CountTimer()
    {
        General_Game.Timer -= Time.deltaTime;
    }

    private void SetTimer()
    {
        _timer.text = $"Time left: {(int)General_Game.Timer}";
    }

    private void ObjectivesLeft()
    {
        _objectives.text = $"Objectives to do: {General_Game.ObjectivesCount - General_Game.CurrentDoneObjectives}";
    }

    private void GameOver()
    {
        if (General_Game.Timer <= 0)
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

            if(_policeCallingTimer < _policeCallingDuration)
            {
                _prgressBarInstantiated.gameObject.SetActive(true);
                ProgressBarUpdate();
            }

        }

        if(Input.GetKey(KeyCode.F))
        {
            _policeCallingTimer += Time.deltaTime;
            _policeIsCalled = true;
        }



        if(_policeCallingTimer >= _policeCallingDuration)
        {
            _timer.color = Color.green;
            _timer.text = $"Time left until police arrives: {(int)General_Game.PoliceTimer}";
            _objectives.text = "Try to survive";
            _objectives.faceColor = Color.red;
            General_Game.PoliceTimer -= Time.deltaTime;
            General_Game.IsPoliceCalled = true;

            _prgressBarInstantiated.gameObject.SetActive(false);
        }

       
    }
    private void CompletedGame()
    {
        if (General_Game.PoliceTimer <= 0)
        {
            SceneManager.LoadScene("GameWon");
        }
    }
    private void ProgressBarUpdate()
    {
        _prgressBarInstantiated.value = 1 - _policeCallingTimer / _policeCallingDuration;
    }
}
