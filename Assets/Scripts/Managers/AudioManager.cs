using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    AudioClip _increaseWallHitSound,_decreaseWallHitSound,_enemyHitsound,_loseGameSound,_winGameSound;
    
    public AudioClip IncreaseWallHitSound => _increaseWallHitSound;
    public AudioClip DecreaseWallHitSound => _decreaseWallHitSound;
    public AudioClip EnemyHitSound =>_enemyHitsound;
    public AudioClip LoseGameSound =>_loseGameSound;
    public AudioClip WinGameSound =>_winGameSound;


    [SerializeField]
    private AudioSource _audioSource;


    public void PlaySound(AudioClip clip) => _audioSource.PlayOneShot(clip);

    public void OnGameWin()
    {
        PlaySound(WinGameSound);
    }

    public void OnGameLose()
    {
        PlaySound(LoseGameSound);
    }

}
