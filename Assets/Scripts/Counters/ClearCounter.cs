using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
   [SerializeField] private KitchenObject kitchenObject;
    



    //na clear counteru ocu da spustim objekat ako stiskam E  drzim ga u ruci

    public override void Interact(Player player)
    {
        //prosledjujem mu playera zato sto zelim da ako nesto ima spawbnano na counteru, da opet klik na E znaci uzimanje tog objekta
        //instantiate je fja za spawn za pojavljivanje, countertoppoint je gameobject u prefabu samo da bi znali vrh countera
        //spawnamo paradajz na vrhu counter

        //Transform tomatoTransform =  Instantiate(tomatoPrefab,counterTopPoint);
        //tomatoTransform.localPosition = Vector3.zero;

        //gledam da je null, jer tad ne treba da ga spawnam ako nije null onda to znaci da je vec spawnovan - resenje problema kad stisnem E 100 puta ispred counter
        /// if (kitchenObj == null)
        ///   {
        ///   Transform kitchenObjectTransform = Instantiate(kitchenObject.prefab, counterTopPoint);
        ///    kitchenObjectTransform.GetComponent<KitchenObj>().SetKitchenObjectParent(this);
        //   kitchenObjectTransform.localPosition = Vector3.zero;

        //  kitchenObj = kitchenObjectTransform.GetComponent<KitchenObj>();
        //  kitchenObj.SetClearCounter(this);
        /// }
        ///  else
        ///  {
        //ima nesto na ovom counteru , kitchenobj nije null, opet klik na E rezultuje tome da objekat ide kod platyera
        //  Debug.Log(kitchenObj.GetClearCounter());
        ///   kitchenObj.SetKitchenObjectParent(player);

        //--------------
        if (!HasKitchenObj())
        {
            if (player.HasKitchenObj())
            {
                player.GetKitchenObj().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //ima nesto na counteru
            if (player.HasKitchenObj())
            {
                //nosi igrac nesto
                if (player.GetKitchenObj().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                { // drzi tanjir igrac

                    if (plateKitchenObject.TryAddIngridient(GetKitchenObj().GetKitchenObject())){

                        GetKitchenObj().DestroySelf();
                    }
                
                
                }
                else
                {
                    //igrac ne drzi tanjir vec nesto drugo
                    if(GetKitchenObj().TryGetPlate(out  plateKitchenObject))
                    {
                        //na counteru je tanjir
                        if (plateKitchenObject.TryAddIngridient(player.GetKitchenObj().GetKitchenObject()))
                        {
                            //ako je tanjir na counteru a igrac ima u rukama neki sastojak koji moze da se doda, onda ga stavljamo na tanjir
                            player.GetKitchenObj().DestroySelf();

                        }
                    }
                }
          
            }
            else {
                //ne nosi nista player
                this.GetKitchenObj().SetKitchenObjectParent(player);
            }
        }
        }


    }



