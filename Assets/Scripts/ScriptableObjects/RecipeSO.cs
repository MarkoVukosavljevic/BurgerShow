using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObject> kitchenObjectList;

    public string recipeName;
}
