using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounter : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;


    //bitno da bude start a ne awake jer na taj nacin se prvo obavlja awake iz player klase pa iz start iz ove klase
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        //ako se selektovani counter jednak ovom counteru ocu da pobeli
        if(e.selectedCounter == baseCounter)
        {
            Prikazi();
        }
        else
        {
            Sakrij();
        }
    }
    private void Prikazi()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
        //setactive fja pali i gasi gameobject
    }

    private void Sakrij()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
