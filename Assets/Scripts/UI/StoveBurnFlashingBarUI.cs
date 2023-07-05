using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        animator.SetBool("IsFlashing", false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        //prikazacemo flash crveni na baru kada je progressnormalized preko odredjene granice progresa
        //ali ne zelim da prikazem ovo kad se meso kuva, vec kad se skuvalo pa treba da se preprzi
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        if (show)
        {
            animator.SetBool("IsFlashing", true);
                }
        else
        {
            animator.SetBool("IsFlashing", false);
           
        }
    }

  
}
