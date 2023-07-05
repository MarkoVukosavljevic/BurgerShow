using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);

    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        //ovde pozivam da se ne bi videli prethodni recepti
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {

        UpdateVisual();

    }

    private void UpdateVisual()
    {
        //brisem sve iz containera osim recipetemplate
        foreach(Transform child in container)
        {
            if (child == recipeTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
           Transform recipeTransform =  Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
