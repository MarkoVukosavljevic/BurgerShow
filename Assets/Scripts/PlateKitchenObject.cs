using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObj
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObject kitchenObject;
    
    }

    [SerializeField] private List<KitchenObject> validKitchenObjectList;

    private List<KitchenObject> kitchenObjectList;

    private void Awake()
    {
        kitchenObjectList = new List<KitchenObject>();
    }

    public bool TryAddIngridient(KitchenObject kitchenObject)
    {
        //nije validan sastojak
        if(!validKitchenObjectList.Contains(kitchenObject)) return false;


        //necemo dozvoliti duplikat sastojka
        if (kitchenObjectList.Contains(kitchenObject)) return false;

        else { 


        kitchenObjectList.Add(kitchenObject);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObject = kitchenObject
            });


            return true;
         }
    }

    public  List<KitchenObject> GetKitchenObjectList()
    {
        return kitchenObjectList;
    }
}
