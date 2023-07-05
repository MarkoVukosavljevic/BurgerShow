using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;
    private void Awake()
    {
        //da ugasim da se prvobitno ne vidi icontemplate na tanjiru
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        //pre spawnovanjatreba da pocistimo ikonice od proslog jela, brisemo sve childove od plateicons
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            //zelim prethodne slikice da unistim ali ne i icontemplate,
            Destroy(child.gameObject);
        }



        List<KitchenObject> kitchenObjectList = plateKitchenObject.GetKitchenObjectList();
        foreach (KitchenObject ko in kitchenObjectList)
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            //transform znaci da ce icontemplate biti dete od ovog objekat

            //kako dodam sastojak treba da dodam i iconicu da bude vidljiva
            iconTransform.gameObject.SetActive(true);


            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObject(ko);
            //uzimam od spawnovanog icona skriptu i saljem joj kitchenobject a ona podesava sprite
        }
    }
}

