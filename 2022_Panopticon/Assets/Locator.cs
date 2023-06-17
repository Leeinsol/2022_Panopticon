using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Locator
{
    static Audio audioService;

    public static Audio GetAudio()
    {
        if (audioService == null)
        {
            Debug.LogWarning("Audio service is not provided. Please provide an audio service using Locator.Provide().");
            return null;
        }

        return audioService;
    }

    public static void Provide(Audio service)
    {
        audioService = service;
    }
}
