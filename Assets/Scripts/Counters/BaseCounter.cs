using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    //objekte dropujemo na countere, pa cemo napraviti static event za zvuk dropanja kako ne bi morali da slusamo svaki counter odvojeno
    public static event EventHandler OnAnyObjectPlacedHere;

    //ovo je za brisanje staticnih eventa pri pokretanju igrice nakon pauziranja i odlaska na main menu, unistila se igrica ali su ostali static eventi
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform counterTopPoint;

    private KitchenObj kitchenObj;


    public virtual void Interact(Player player)
    {

    }

    public virtual void InteractAlternate(Player player)
    {

    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObj(KitchenObj kitchenObj)
    {
        this.kitchenObj = kitchenObj;

        if(kitchenObj!=null )
        {//ako je razlicito od null stavlja se nesto na counter - treba zvuk
            OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty); 

        }
    }

    public KitchenObj GetKitchenObj()
    {
        return kitchenObj;
    }

    public void ClearKitchenObj()
    {
        kitchenObj = null;
    }

    public bool HasKitchenObj()
    {
        return (kitchenObj != null);
    }


}
