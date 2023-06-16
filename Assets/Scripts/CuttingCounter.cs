using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;


    private int cuttingProgress;

    //ovaj deo je samo za stavljanje kao clear counter
    public override void Interact(Player player)
    {

        if (!HasKitchenObj())
        {
            if (player.HasKitchenObj() && HasRecipeWithInput(player.GetKitchenObj().GetKitchenObject()))
            {

                //ubacicemo proveru da li uopste ta hrana moze da se sece, hleb ne bi trebalo seci
                cuttingProgress = 0;
                
                    player.GetKitchenObj().SetKitchenObjectParent(this);
                
            }
        }
        else
        {
            if (!player.HasKitchenObj())
            {
                this.GetKitchenObj().SetKitchenObjectParent(player);
            }
        }
    }

    //za secenje metoda
    public override void InteractAlternate(Player player)
    {

        if (HasKitchenObj() && HasRecipeWithInput(GetKitchenObj().GetKitchenObject()))
        {
            cuttingProgress ++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObj().GetKitchenObject());
          //ako je nas broj seckanja presao potreban broj onda spawnamo slices
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {


                //secemo samo ukoliko ima objekat na counteru i ako je u receptima za secenje
                //za secenje cemo unistiti objekat koji je na stolu i dodati iseckan objekat
                KitchenObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObj().GetKitchenObject());
                GetKitchenObj().DestroySelf();
                //spawn
                /// Transform kitchenObjectTransform = Instantiate(cutKitchenObject.prefab);
                /// kitchenObjectTransform.GetComponent<KitchenObj>().SetKitchenObjectParent(this);
                //imam duplikaciju koda, ovaj isti kod ovde i na container counteru
                //pa idemo preko fje
                KitchenObj.SpawnKitchenObject(outputKitchenObjectSO, this);

                //sada cemo napraviti secenje preko recepata, da se od kupusa ne dobije tomato slices
            }
        }

    }
    private bool HasRecipeWithInput(KitchenObject inputKitchenObject)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObject);
        if (cuttingRecipeSO != null)
        {
            return true;
        }
        //ovo je provera ako stavim hleb na secenje da ne moze, nema ga u receptima
    
        return false;
    }
    public KitchenObject GetOutputForInput(KitchenObject inputKitchenObject)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObject);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObject inputKitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObject)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}


