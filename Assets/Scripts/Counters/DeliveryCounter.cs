using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    //posto cu imati jedan delivery counter mogu singleton da ga napravim
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObj())
        {
            if(player.GetKitchenObj().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //samo prihvatamo tanjire
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObj().DestroySelf();

            }
        }
    }


}
