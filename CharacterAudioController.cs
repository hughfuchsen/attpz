// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(AudioSource))]
// public class CharacterAudioController : MonoBehaviour
// {
//     [Header("Footstep Settings")]

//     CharacterMovement characterMovement;
//     public AudioClip[] footstepClips;     // Array of footstep sounds
//     public float stepInterval = 0.19f;     // Time between steps
//     private float stepTimer;

//     [Header("Landing Settings")]
//     public AudioClip landingClip;         // Landing sound
//     private bool isGrounded = true;       // Track if character is on the ground

//     private AudioSource audioSource;
//     private Rigidbody2D rb;

//     void Awake()
//     {
//         if(this.gameObject.CompareTag("Player"))
//         {
//             characterMovement = GetComponent<CharacterMovement>();  
//         }
//         audioSource = GetComponent<AudioSource>();
//         rb = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         HandleFootsteps();
//         HandleLanding();
//     }

//     private void HandleFootsteps()
//     {
//         // Example: simple horizontal movement check
//         if (characterMovement.change != Vector3.zero)
//         {
//             stepTimer -= Time.deltaTime;
//             if (stepTimer <= 0f)
//             {
//                 PlayFootstep();
//                 stepTimer = stepInterval;
//             }
//         }
//         else
//         {
//             stepTimer = 0f;
//         }
//     }

//     private void PlayFootstep()
//     {
//         if (footstepClips.Length == 0) return;

//         int index = Random.Range(0, footstepClips.Length);
//         audioSource.PlayOneShot(footstepClips[index]);
//     }

//     private void HandleLanding()
//     {
//         // Placeholder: if you implement ground detection, trigger landing sounds here
//     }

//     // Optional: call this from your ground check
//     public void OnLand()
//     {
//         if (landingClip != null)
//         {
//             audioSource.PlayOneShot(landingClip);
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioController : MonoBehaviour
{
    [Header("Footstep Settings")]
    CharacterMovement characterMovement;
    public AudioClip[] footstepClips;
    public AudioClip[] grassFootstepClips;
    public float stepInterval = 0.19f;
    private float stepTimer;

    [Header("Landing Settings")]
    public AudioClip landingClip;
    private bool isGrounded = true;

    [Header("Surface Detection")]
    public Camera mainCamera;
    public float regionWidth = 5f;
    public float regionHeight = 1f;
    public float grassHueMin = 0.22f;
    public float grassHueMax = 0.42f;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Texture2D samplingTexture;

    void Awake()
    {
        if (CompareTag("Player"))
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        samplingTexture = new Texture2D(8, 2, TextureFormat.RGB24, false); // Low-res sampling
    }

    void Update()
    {
        HandleFootsteps();
        HandleLanding();
    }

    private void HandleFootsteps()
    {
        if (characterMovement != null && characterMovement.change != Vector3.zero)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        AudioClip[] clipsToUse = footstepClips;

        if (IsOnGrass())
            clipsToUse = grassFootstepClips;
        else    
            clipsToUse = footstepClips;

        if (clipsToUse.Length == 0) return;

        int index = Random.Range(0, clipsToUse.Length);
        audioSource.PlayOneShot(clipsToUse[index]);
    }

    private bool IsOnGrass()
    {
        // Define region under character
        Vector3 bottomLeft = transform.position + new Vector3(-regionWidth / 2f, -regionHeight / 2f, 0);
        Vector3 topRight = transform.position + new Vector3(regionWidth / 2f, regionHeight / 2f, 0);

        // Convert to screen coordinates
        Vector3 blScreen = mainCamera.WorldToScreenPoint(bottomLeft);
        Vector3 trScreen = mainCamera.WorldToScreenPoint(topRight);

        int width = Mathf.Clamp(Mathf.CeilToInt(trScreen.x - blScreen.x), 1, 64);
        int height = Mathf.Clamp(Mathf.CeilToInt(trScreen.y - blScreen.y), 1, 64);

        Rect readRect = new Rect(blScreen.x, blScreen.y, width, height);
        samplingTexture.ReadPixels(readRect, 0, 0);
        samplingTexture.Apply();

        Color[] pixels = samplingTexture.GetPixels();
        float hueSum = 0f;

        foreach (Color c in pixels)
        {
            Color.RGBToHSV(c, out float h, out _, out _);
            hueSum += h;
        }

        float avgHue = hueSum / pixels.Length;
        return (avgHue >= grassHueMin && avgHue <= grassHueMax);
    }

    private void HandleLanding()
    {
        // implement later
    }

    public void OnLand()
    {
        if (landingClip != null)
        {
            audioSource.PlayOneShot(landingClip);
        }
    }
}
