
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    public EventReference musicEvent;

    private EventInstance musicInstance;

    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    public void SetBeatStyle(float value) // 0 = bossanova, 1 = straight
    {
        musicInstance.setParameterByName("BeatStyle", value);
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SoundtrackScript: MonoBehaviour
// {
//     public AudioSource track1;
//     public AudioSource track2;
    
//     public float fadeDuration = 3f;

//     // To store the currently running coroutine
//     private Coroutine activeFadeCoroutine = null;


//     void Start()
//     {
//         // Get current DSP time
//         double startTime = AudioSettings.dspTime + 0.5;  // Start after 0.5 seconds for example

//         // Play both tracks at the same future time
//         track1.PlayScheduled(startTime);
//         track2.PlayScheduled(startTime);
//     }



//     // Call this to fade out track1 and fade in track2
//     public void FadeOutIn(AudioSource fadeOutTrack, AudioSource fadeInTrack)
//     {
//         // Stop the currently active coroutine if it's running
//         if (activeFadeCoroutine != null)
//         {
//             StopCoroutine(activeFadeCoroutine);
//         }

//         // Start the new fade coroutine and store its reference
//         activeFadeCoroutine = StartCoroutine(FadeVolumes(fadeOutTrack, fadeInTrack, fadeDuration));
//     }


//     // Coroutine to adjust only the volumes of the tracks
//     private IEnumerator FadeVolumes(AudioSource fadeOutTrack, AudioSource fadeInTrack, float duration)
//     {
//         float startVolumeFadeOut = fadeOutTrack.volume;
//         float startVolumeFadeIn = fadeInTrack.volume;

//         float timer = 0.0f;

//         while (timer < duration)
//         {
//             timer += Time.deltaTime;
//             float normalizedTime = timer / duration;

//             // Adjust volumes based on normalized time
//             fadeOutTrack.volume = Mathf.Lerp(startVolumeFadeOut, 0f, normalizedTime);
//             fadeInTrack.volume = Mathf.Lerp(startVolumeFadeIn, 1f, normalizedTime);

//             yield return null; // wait for the next frame
//         }

//         // Ensure fadeOut track volume is fully at 0 and fadeIn track is fully at 1
//         fadeOutTrack.volume = 0f;
//         fadeInTrack.volume = 1f;

//         // Clear the active coroutine reference once it's finished
//         activeFadeCoroutine = null;
//     }
// }



