using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioController : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioClip[] footstepClips;       // Concrete/default
    public AudioClip[] grassFootstepClips;
    public AudioClip[] sandFootstepClips;
    public AudioClip[] dirtFootstepClips;
    public AudioClip[] carpetFootstepClips;
    public AudioClip[] tileFootstepClips;
    public float stepInterval = 0.18f;

    [Header("Landing Settings")]
    public AudioClip landingClip;

    [Header("Surface Detection")]
    public Camera mainCamera; // player camera
    public Camera surfaceCamera;
    private RenderTexture surfaceRT;
    private Texture2D samplingTexture;

    [Range(0f,1f)] public float grassHueMin = 0.22f;
    [Range(0f,1f)] public float grassHueMax = 0.42f;
    [Range(0f,1f)] public float sandHueMin = 0.10f;
    [Range(0f,1f)] public float sandHueMax = 0.15f;
    [Range(0f,1f)] public float dirtHueMin = 0.05f;
    [Range(0f,1f)] public float dirtHueMax = 0.08f;
    [Range(0f,1f)] public float carpetHueMin = 0.0f;
    [Range(0f,1f)] public float carpetHueMax = 0.05f;
    [Range(0f,1f)] public float tilesHueMin = 0.5f;
    [Range(0f,1f)] public float tilesHueMax = 0.6f;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private CharacterMovement characterMovement;
    private CharacterAnimation characterAnimation;

    [HideInInspector] public CameraMovement cameraMovement;
    private float stepTimer;

    SurfaceType lastDetectedSurface = SurfaceType.Concrete;

    private Vector3 leftFootPos;
    private Vector3 rightFootPos;

    public RoomScript roomScript;

    private int lastScreenWidth;
    private int lastScreenHeight;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        characterMovement = GetComponent<CharacterMovement>();
        characterAnimation = GetComponent<CharacterAnimation>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Create hidden camera for surface detection
        GameObject camObj = new GameObject("SurfaceDetectionCamera");
        camObj.hideFlags = HideFlags.HideAndDontSave;
        surfaceCamera = camObj.AddComponent<Camera>();
        surfaceCamera.CopyFrom(mainCamera);
        surfaceCamera.cullingMask = LayerMask.GetMask("Nothing");
        surfaceCamera.cullingMask = LayerMask.GetMask("SurfaceMap");
        surfaceCamera.clearFlags = CameraClearFlags.SolidColor;
        surfaceCamera.backgroundColor = Color.black;
        surfaceCamera.enabled = false;

        // Create RenderTexture
        surfaceRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        surfaceCamera.targetTexture = surfaceRT;

        // 1x1 texture for sampling
        samplingTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
    }

    void Start()
    {
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();

        // Optional: continuously sample the surface once per frame (after rendering)
        StartCoroutine(SampleSurfaceCoroutine());
    }

    private IEnumerator SampleSurfaceCoroutine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            EnsureRenderTextureIsValid();

            if (surfaceCamera != null)
                surfaceCamera.Render();

            lastDetectedSurface = GetLeadingFootSurface();
        }
    }


    void Update()
    {
        SyncSurfaceCamera();
        HandleFootsteps();
    }

    private void SyncSurfaceCamera()
    {
        if (surfaceCamera == null || mainCamera == null || characterMovement == null) return;

        // Match position & orthographic size
        surfaceCamera.transform.position = mainCamera.transform.position;
        surfaceCamera.transform.rotation = mainCamera.transform.rotation;
        surfaceCamera.orthographicSize = mainCamera.orthographicSize;

        // Switch layers based on whether the player is inside
        if (characterMovement.playerIsOutside)
        {
            // Only render the SurfaceMap layer
            surfaceCamera.cullingMask = LayerMask.GetMask("SurfaceMap");
        }
        else
        {
            // Render default layers for indoor surfaces
            if (cameraMovement.currentRoom != null)
            {  
                surfaceCamera.cullingMask = LayerMask.GetMask("Everything");
            }    
        }
    }


    private void HandleFootsteps()
    {
        if(characterAnimation.movementStartIndex == characterAnimation.run) { stepInterval = 0.22f; }
        else { stepInterval = 0.18f; }

        if (characterMovement != null && characterMovement.change != Vector3.zero)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else stepTimer = 0f;
    }

    private void PlayFootstep()
    {
        SurfaceType surface = SurfaceType.Carpet;

        if(characterMovement.playerIsOutside)
        {
            surface = GetLeadingFootSurface();
        }
        else if (cameraMovement.currentRoom != null)
        {
            surface = cameraMovement.currentRoom.roomSurface;
        }

            AudioClip[] clipsToUse = footstepClips;
            switch (surface)
            {
                case SurfaceType.Grass: clipsToUse = grassFootstepClips; break;
                case SurfaceType.Sand: clipsToUse = sandFootstepClips; break;
                case SurfaceType.Dirt: clipsToUse = dirtFootstepClips; break;
                case SurfaceType.Carpet: clipsToUse = carpetFootstepClips; break;
                case SurfaceType.Tiles: clipsToUse = tileFootstepClips; break;
            }

            if (clipsToUse.Length == 0) return;
            int index = Random.Range(0, clipsToUse.Length);
            audioSource.PlayOneShot(clipsToUse[index]);
    }

    private SurfaceType GetLeadingFootSurface()
    {
        if (mainCamera == null || characterMovement == null || characterMovement.boxCollider == null)
            return SurfaceType.Concrete;

        Bounds bounds = characterMovement.boxCollider.bounds;
        leftFootPos = new Vector3(bounds.min.x - 1f, bounds.min.y + 0.5f, 0f);
        rightFootPos = new Vector3(bounds.max.x + 1f, bounds.min.y + 0.5f, 0f);

        Vector3 leftScreen = mainCamera.WorldToScreenPoint(leftFootPos);
        Vector3 rightScreen = mainCamera.WorldToScreenPoint(rightFootPos);

        // Render and sample safely from RenderTexture
        surfaceCamera.Render();
        Color leftColor = SamplePixel(leftScreen);
        Color rightColor = SamplePixel(rightScreen);

        Color.RGBToHSV(leftColor, out float hL, out _, out _);
        Color.RGBToHSV(rightColor, out float hR, out _, out _);

        float leadingHue = (hL + hR) / 2f;
        switch (characterMovement.controlDirection)
        {
            case Direction.Left:
            case Direction.UpLeft:
            case Direction.DownLeft:
            case Direction.UpFacingLeft:
            case Direction.DownFacingLeft:
                leadingHue = hL; break;
            case Direction.Right:
            case Direction.UpRight:
            case Direction.RightDown:
            case Direction.UpFacingRight:
            case Direction.DownFacingRight:
                leadingHue = hR; break;
        }

        SurfaceType surface = SurfaceType.Concrete;

        if (leadingHue >= grassHueMin && leadingHue <= grassHueMax) surface = SurfaceType.Grass;
        else if (leadingHue >= sandHueMin && leadingHue <= sandHueMax) surface = SurfaceType.Sand;
        else if (leadingHue >= dirtHueMin && leadingHue <= dirtHueMax) surface = SurfaceType.Dirt;
        else if (!characterMovement.playerIsOutside && leadingHue >= carpetHueMin && leadingHue <= carpetHueMax) surface = SurfaceType.Carpet;
        else if (!characterMovement.playerIsOutside && leadingHue >= tilesHueMin && leadingHue <= tilesHueMax) surface = SurfaceType.Tiles;

        lastDetectedSurface = surface;
        return surface;
    }

    private Color SamplePixel(Vector3 screenPos)
    {
        if (surfaceRT == null)
        {
            Debug.LogWarning("SurfaceRenderTexture missing!");
            return Color.black;
        }

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = surfaceRT;

        samplingTexture.ReadPixels(new Rect(screenPos.x, screenPos.y, 1, 1), 0, 0);
        samplingTexture.Apply();

        RenderTexture.active = currentRT;
        return samplingTexture.GetPixel(0, 0);
    }

    private void OnDrawGizmos()
    {
        if (characterMovement == null || characterMovement.boxCollider == null) return;

        Gizmos.color = GetGizmoColorForSurface(lastDetectedSurface);
        Gizmos.DrawCube(leftFootPos, Vector3.one * 0.1f);
        Gizmos.DrawCube(rightFootPos, Vector3.one * 0.1f);
    }

    private Color GetGizmoColorForSurface(SurfaceType surface)
    {
        return surface switch
        {
            SurfaceType.Grass => Color.green,
            SurfaceType.Sand => Color.yellow,
            SurfaceType.Dirt => new Color(0.6f, 0.3f, 0f),
            SurfaceType.Carpet => Color.red,
            SurfaceType.Tiles => Color.cyan,
            _ => Color.black,
        };
    }

    private void EnsureRenderTextureIsValid()
    {
        if (surfaceRT == null || Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            if (surfaceRT != null)
                surfaceRT.Release();

            surfaceRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            surfaceCamera.targetTexture = surfaceRT;

            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;

            Debug.Log("Recreated surface RenderTexture: " + Screen.width + "x" + Screen.height);
        }
    }
}
