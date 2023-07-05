using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); //jer su na istom objektu skripta i animator mogu da dodjem do animatora na ovaj nacin
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;

    //dodali smo event listenera
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Cut");
    }

  
}
