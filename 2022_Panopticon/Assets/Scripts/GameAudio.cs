using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour, Audio
{
    AudioSource audioSource;
    private Dictionary<int, AudioClip> audioClips = new Dictionary<int, AudioClip>();
    public AudioClip FireSound, oneByOneReloadSound, allReloadSound, getItemSound, changeWaeponSound, energySound;

    public void Awake()
    {
        audioSource = StageSetting.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        //AddSound(1, FireSound);
        //AddSound(2, oneByOneReloadSound);
        //AddSound(3, allReloadSound);
        //AddSound(4, getItemSound);
        //AddSound(5, changeWaeponSound);
        //AddSound(6, energySound);
        Debug.Log(audioClips[1]);

    }
    public void AddSound(int soundID, AudioClip audioClip)
    {
        if (!audioClips.ContainsKey(soundID))
        {
            audioClips.Add(soundID, audioClip);
            Debug.Log(audioClips[soundID]);
        }
        else
        {
            Debug.LogWarning("Sound with ID " + soundID + " already exists.");
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
        //if (audioClips.TryGetValue(ID, out AudioClip audioClip))
        //{
        //    Debug.Log(audioClips[ID]);

        //    Debug.Log("Playing sound with ID: " + ID);
        //    // Play sound using audioClip
        //    audioSource.PlayOneShot(audioClip);
        //}
    }

    public void StopSound(int ID)
    {
        //audioSource.Stop(ID);
        Debug.Log("Stopping sound with ID: " + ID);
    }

    public void StopAllSounds()
    {
        Debug.Log("Stopping all sounds");
    }

}
