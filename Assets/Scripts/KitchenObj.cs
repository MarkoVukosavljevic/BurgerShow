using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObj : MonoBehaviour
{
    [SerializeField] private KitchenObject kitchenObject;
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    //fja za menjanje countera za kitchen object
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        { // ako je trenutni roditelj odnosno counter nije null, brisemo mu kitchen obj jer menjamo counter
            this.kitchenObjectParent.ClearKitchenObj();
        }
       this. kitchenObjectParent = kitchenObjectParent;

        //kod novog roditelja setujem kitchen object
        if (kitchenObjectParent.HasKitchenObj())
        {
            Debug.LogError("novi counter vec ima objekat kuhinjski na sebi");
        }
        kitchenObjectParent.SetKitchenObj(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        //pre unistavanje objekta moramo da ocistimo roditelja
        kitchenObjectParent.ClearKitchenObj();
        Destroy(gameObject);
    }


    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        plateKitchenObject = null; //kada koristim out parametre uvek moram da ih setujem na nesto
        return false;
    }

    //fja za spawnanje kitchen obj i dodeljivanje parenta
    public static KitchenObj SpawnKitchenObject(KitchenObject kitchenObject,IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObject.prefab);
        KitchenObj kitchenObj = kitchenObjectTransform.GetComponent<KitchenObj>();
            kitchenObj.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObj;
    
    }
}
