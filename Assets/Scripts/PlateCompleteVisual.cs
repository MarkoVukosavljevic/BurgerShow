using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]  public struct KitchenObject_GameObject
    {
        public KitchenObject kitchenObject;
        public GameObject GameObject;   
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObject_GameObject> kitchenObjectGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        //ocu da stavim da se pre nego sto dodam bilo koji sastojak svi budu nevidljivi na tanjiru
        foreach (KitchenObject_GameObject k in kitchenObjectGameObjectList)
        {
                           k.GameObject.SetActive(false);
           
        }

    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObject_GameObject k in kitchenObjectGameObjectList)
        {
            if( e.kitchenObject == k.kitchenObject )
            {
                k.GameObject.SetActive(true);
                //sastojak koji smo dodali ocu da postavim da bude vidljiv
            }

        }
    }
}
