using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCharacterUITrigger : MonoBehaviour
{
    public GameObject chrctrCuzzyObj;

    [SerializeField] GameObject backdrop;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject platform;
    [SerializeField] LoadCSVData loadCSVData;

    private Coroutine backdropFadeCoroutine;

    private bool fadeFirst;

    private bool playerInRange = false;




    public bool steppedOnTrigger;





    // Start is called before the first frame update
    void Start () 
    {
        StartCoroutine (LateStart());

        Player = GameObject.FindGameObjectWithTag("Player");

        backdrop = GameObject.FindGameObjectWithTag("InnerBuildingBackdrop");

        loadCSVData = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu").GetComponent<LoadCSVData>();

        



        // playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.JoystickButton0)) // A button
            {
                loadCSVData.DisplayRandomRow();
            }
        }
    }


    IEnumerator LateStart() 
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // wait for children to switch off separately
        
        chrctrCuzzyObj.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if(player.CompareTag("PlayerCollider"))
        {
            if(backdropFadeCoroutine != null)
            {
                StopCoroutine(this.backdropFadeCoroutine);
            }

            playerInRange = true;

            backdropFadeCoroutine = StartCoroutine(FadeBackdropAndSetSortingLayersEtc(false, 1f));

            // cameraMovement.smoothing = 3f;
            // cameraMovement.target = characterCuzzyTarget;
        }
    }

    // void OnTriggerStay2D(Collider2D player)
    // {
    //     if(player.CompareTag("PlayerCollider"))
    //     {

    //         if ((Input.GetKeyDown(KeyCode.Space) || 
    //         Input.GetKeyDown(KeyCode.JoystickButton0) // A button
    //         //   Input.GetKeyUp(KeyCode.JoystickButton1) ||  // B button
    //         //   Input.GetKeyUp(KeyCode.JoystickButton2) ||  // X button
    //         //   Input.GetKeyUp(KeyCode.JoystickButton3)
    //         ))
    //         {
    //             loadCSVData.DisplayRandomRow();   
    //         }
    //     }
    // }

    void OnTriggerExit2D(Collider2D player)
    {
        if(player.CompareTag("PlayerCollider"))
        {
            if(backdropFadeCoroutine != null)
            {
                StopCoroutine(this.backdropFadeCoroutine);
            }

            playerInRange = false;

            backdropFadeCoroutine = StartCoroutine(FadeBackdropAndSetSortingLayersEtc(true,0f));

            // cameraMovement.smoothing = 1000f;
            // cameraMovement.target = player.transform;
        }
    }

    private IEnumerator FadeBackdropAndSetSortingLayersEtc(bool fadeFirst, float alpha)
    {
        // if (backdrop != null)
        // {
            SpriteRenderer sr = backdrop.GetComponent<SpriteRenderer>();

            // Calculate duration based on the alpha difference
            float alphaDiff = Mathf.Abs(sr.color.a - alpha);
            float duration = alphaDiff; // Assuming alpha ranges from 0 to 1
            float elapsedTime = 0.0f;

            if(fadeFirst == false)
            {
                // chrctrCuzzyObj.SetActive(true);
                steppedOnTrigger = true;

                SetTreeSortingLayer(backdrop, "UI1");
                SetTreeSortingLayer(Player, "UI2");
                SetTreeSortingLayer(platform, "UI2");

                chrctrCuzzyObj.SetActive(true);

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;

                    float nextAlpha = Mathf.Lerp(sr.color.a, alpha, t);
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);

                    yield return null;
                }

                // yield return new WaitForSeconds(0.1f);



                // yield return new WaitForSeconds(1f);

                // Ensure the final alpha value is set
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
            else
            {
                if(chrctrCuzzyObj != null)
                {
                    chrctrCuzzyObj.SetActive(false);
                }
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;

                    float nextAlpha = Mathf.Lerp(sr.color.a, alpha, t);
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);

                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);




                steppedOnTrigger = false;

                SetTreeSortingLayer(backdrop, "Backdrop");
                SetTreeSortingLayer(Player, "Default");
                SetTreeSortingLayer(platform, "Default");



                // Ensure the final alpha value is set
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);  
            }
        // }
    }

    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            ActivateCharacterUITrigger.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }    
    }
}
