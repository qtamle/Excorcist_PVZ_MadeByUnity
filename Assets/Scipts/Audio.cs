using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource music;
    public AudioSource vfx;
    public AudioClip musicMain;

    private void Start()
    {
        music.clip = musicMain;
        music.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        vfx.PlayOneShot(sfxClip);
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
