using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }   

    private float volume = .3f;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("MusicVolume", .3f);
        //druga vrednost je default vrednost koja ce se vratiti ako nije bilo sacuvane vrednosti za playerprefs
        //muzika krece odma tako da moram odmah i da promenim:
        audioSource.volume = volume;
    }
    public void ChangeVolume()
    {
        volume += .1f;
        //kada preskoci moram da ga vratim na 0
        if (volume > 1f)
        {
            volume = 0f;
        }

        audioSource.volume = volume;

        //playerprefs se koristi da kada podesimo volumene na muzici i sfxima da ako opet kliknem play sa
        //main menija da se ne vrati na defaultno nego da ostane zabelezeno kako smo ga ostavili
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
