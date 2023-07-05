using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = .1f;
    //10 puta po sekundi ce se cuti zvuk
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            //kako dodje do 0 resetujemo ga na max
            footstepTimer = footstepTimerMax;

            if (player.DaLiHoda())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
            
            }
    }
}
