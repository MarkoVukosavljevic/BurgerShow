using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume=1f;

    private void Awake()
    {
        Instance = this;

        volume =PlayerPrefs.GetFloat("SoundEffectsVolume", 1f);
        //druga vrednost je default vrednost koja ce se vratiti ako nije bilo sacuvane vrednosti za playerprefs
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {

        TrashCounter tc = (TrashCounter)sender;
        PlaySound(audioClipRefsSO.trash, tc.transform.position);

    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);

    }

    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
       
        PlaySound(audioClipRefsSO.objectPickup,Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        //sender je zapravo ovo gde cemo postaviti position
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFailed, deliveryCounter.transform.position);
        //uzimamo poziciju delivery counter, znaci zvuk ce se emitovati kod njega, bice jaci ako smo mu blizi
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    //volume je optional ulazni parametar
    public void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volume =1f)
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
     
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {

        //svirace taj clip na toj poziciji sa tim volumenom
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier*volume);
    }

    public void PlayFootstepsSound(Vector3 position,float volume)
    {
        PlaySound(audioClipRefsSO.footsteps, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero,4f);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position, 4f);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        //kada preskoci moram da ga vratim na 0
        if(volume > 1f)
        {
         
            volume = 0f;
        }

        //playerprefs se koristi da kada podesimo volumene na muzici i sfxima da ako opet kliknem play sa
        //main menija da se ne vrati na defaultno nego da ostane zabelezeno kako smo ga ostavili
        PlayerPrefs.SetFloat("SoundEffectsVolume",volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
