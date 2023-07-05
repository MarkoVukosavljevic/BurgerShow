using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
  public static KitchenGameManager Instance { get; private set; }


    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    
    //stanja u kojim igrica moze da bude
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 60f;
    private bool isGamePaused = false;
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        //slusamo event za pauzu
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;

        //slusamo kad igrac stisne E sa tutorial ekrana da pocne partija
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, new EventArgs());
        }

    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();

    }

    private void Update()
    {

        switch (state)
        {
            case State.WaitingToStart:
                
                break;

                case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
                case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
                case State.GameOver:
               
                break;
        }
        Debug.Log(state);

    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1-( gamePlayingTimer / gamePlayingTimerMax);
        //oduzimamo od keca jer gore oduzimamo vreme a ne dodajemo pa ce biti obrnuto
    }

    public void TogglePauseGame()
    {
        //time ima mnozac i to je timescale ako njega stavimo na 0, svuda cemo pauzirati time.deltatime
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);


        }
    }
}
