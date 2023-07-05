using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    //interfejs se ne prikazuje u Unityu pa cu morati preko gameobjecta da ga stavim kao serializefield
    private IHasProgress hasProgress;




    //bitno na start a ne awake ako pristupam stranim referencama
    private void Start()
    {
        //inicijalizovanje hasProgress interfejsa
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        //trazimo komponenetu koja je interfejs
        if(hasProgress == null)
        {
            Debug.LogError("Game object " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }



        hasProgress.OnProgressChanged += IHasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
        //sakrivam ga na pocetku partije kad tek krecemo,bitno da je na kraju
    }

    private void IHasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {

        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
            //ako je skroz prazan bar ili je full ocemo da ga sakrijemo
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
       gameObject.SetActive(true);

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
