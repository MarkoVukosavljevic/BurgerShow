using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    //event za zvucni efekat bacanja u smece
    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(Player player)
    {
       if(player.HasKitchenObj())
        {
            player.GetKitchenObj().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

    //ovo je za brisanje staticnih eventa pri pokretanju igrice nakon pauziranja i odlaska na main menu, unistila se igrica ali su ostali static eventi
  new  public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

}
