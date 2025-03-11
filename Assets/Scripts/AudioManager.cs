
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio Source
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // Audio Clip
    [Header("--------- Audio Clip ---------")]
    public AudioClip enemyAttack;
    public AudioClip enemyDamaged;
    public AudioClip enemyDeath;
    public AudioClip playerDamaged;
    public AudioClip playerDeath;
    public AudioClip playerPunch;
    public AudioClip pose1;
    public AudioClip pose2;
    public AudioClip pose3;
    public AudioClip poseChime;
    public AudioClip slash;
    public AudioClip whoosh;
    public AudioClip kick;


    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
