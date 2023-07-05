using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObject kitchenObject;

    public event EventHandler OnPlayerGrabbedObject;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObj()) // ako nema kitchen object spawnacemo ga
        {
            /// Transform kitchenObjectTransform = Instantiate(kitchenObject.prefab);
            ///  kitchenObjectTransform.GetComponent<KitchenObj>().SetKitchenObjectParent(player);// odmah ga spawnamo i dajemo playeru jer je container
            // moze i preko fje
            KitchenObj.SpawnKitchenObject(kitchenObject, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
          
        }
  


    }

   
}
