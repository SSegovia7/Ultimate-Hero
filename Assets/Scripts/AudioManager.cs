
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio Source
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // Audio Clip
    [Header("--------- Audio Clip ---------")]
    public AudioClip enemyDeath;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
