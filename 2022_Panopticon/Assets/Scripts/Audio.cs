using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Audio
{
    void PlaySound(AudioClip audioClip);
    void StopSound(int ID);
    void StopAllSounds();

}
