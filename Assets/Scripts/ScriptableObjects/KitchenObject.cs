using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()] // ovako ubacujem kitchen object u assete koje mogu da dodam u unity
public class KitchenObject : ScriptableObject
{

    public Transform prefab;
    public Sprite sprite;
    public string objectName;

}
