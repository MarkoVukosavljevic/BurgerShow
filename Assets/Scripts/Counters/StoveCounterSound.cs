using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //za zvuk przenja
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;

        //za zvuk warninga
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {

        //pusticemo warning zvuk kada je progressnormalized preko odredjene granice progresa
        //ali ne zelim da pustim ovo kad se meso kuva, vec kad se skuvalo pa treba da se preprzi
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        //pustacemo zvuk ako je stanje fried ili frying
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();


        }
        else
        {
            audioSource.Pause();
        }

    }
    private void Update()
    {
        if (playWarningSound)
        {
            //koliko puta ocu da cujem zvuk warninga, 5 puta po sekundi
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer < 0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);

            }



        }
    }

}
