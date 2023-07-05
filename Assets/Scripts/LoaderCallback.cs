using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{

    //ova skripta iima ulogu da prodje neko vreme izmedju loading scene i glavne game scene- scenaburger
    //zelim samo da renderujem prvi frame od update fje i onda da odemo na scenaburger
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            Loader.LoaderCallBack();
        }
    }
}
