using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFadeTrigger : MonoBehaviour

{
  PlayerAnimationAndMovement PlayerAnimationAndMovement;
  [SerializeField] GameObject Player;  
  public GameObject obj;
  public float defaultFadeAlphaFloat = 0.15f;
  private Coroutine fadeCoro;
  private Coroutine resetTagsCoro;

  private List<float> initialAlphaFloatList = new List<float>();
  private List<float> fadedAlphaFloatList = new List<float>();
  private List<GameObject> objListToFade = new List<GameObject>();




  void Awake()
  {
    Player = GameObject.FindGameObjectWithTag("Player");
    PlayerAnimationAndMovement = Player.GetComponent<PlayerAnimationAndMovement>(); 
    

    if (obj != null)
    {
      GetSprites(obj, objListToFade);
      setTreeAlpha(objListToFade, initialAlphaFloatList);
    }


  }

  void OnTriggerEnter2D()
  {
    // StopAllCoros();
    setTreeAlpha(objListToFade, fadedAlphaFloatList);


    if(PlayerAnimationAndMovement.playerOnThresh == false) // if player is not on building threshold
    {
      // fadeCoro = StartCoroutine(treeFade(objListToFade, fadedAlphaFloatList));
      setTreeAlpha(objListToFade, fadedAlphaFloatList);

    }
    else
    {
      obj.tag = "AlphaZeroEntExt";
      TagChildrenOfTaggedParents("AlphaZeroEntExt");
    }
  }
  void OnTriggerExit2D()
  {
    // StopAllCoros();

    if(PlayerAnimationAndMovement.playerOnThresh == false)
    {
      // fadeCoro = StartCoroutine(treeFade(objListToFade, initialAlphaFloatList));
      setTreeAlpha(objListToFade, initialAlphaFloatList);
      obj.tag = "Untagged";
      TagChildrenOfTaggedParents("Untagged");
    }
    else
    {
      obj.tag = "AlphaZeroEntExt";
      TagChildrenOfTaggedParents("AlphaZeroEntExt");
      resetTagsCoro = StartCoroutine(resetTagsToUntaggedAfterWait()); //maybe something to do with balcony door?
    }
  }


  void setTreeAlpha(List<GameObject> spriteList, List<float> fadeTo) 
  {
    for (int i = 0; i < spriteList.Count; i++)
    {
      SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
      if ((sr != null))
      {
          sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo[i]);
      }
    }


  }   
  
  // IEnumerator treeFade(
  // List<GameObject> spriteList,
  // // List<float> fadeFrom,
  // List<float> fadeTo) 
  // {
  //   for (int i = 0; i < spriteList.Count; i++)
  //   {
  //     // for (float t = 0.0f; t < 1; t += Time.deltaTime) 
  //     // {
  //       // float currentAlpha = Mathf.Lerp(fadeFrom[i], fadeTo[i], t * 10);
  //       // setTreeAlpha(spriteList[i], currentAlpha);
  //       // float currentAlpha = Mathf.Lerp(fadeFrom[i], fadeTo[i], t * 10);
  //       setTreeAlpha(spriteList[i], fadeTo[i]);
  //     // }
  //   }
  //   yield return null;
  // }
  
  IEnumerator resetTagsToUntaggedAfterWait()
  {
    yield return new WaitForSeconds(2f);
    obj.tag = "Untagged";
    TagChildrenOfTaggedParents("Untagged");
    yield return null;
  }

  // void StopAllCoros()
  // {
  //     if(fadeCoro != null)
  //     {
  //         StopCoroutine(fadeCoro);
  //     }

  //     if(resetTagsCoro != null)
  //     {
  //         StopCoroutine(resetTagsCoro);
  //     }

  // }

  private void TagChildrenOfTaggedParents(string tag)
  {
      // for (int i = 0; i < spriteList.Count; i++)
      // {
          Transform tr = obj.transform;

          Stack<Transform> stack = new Stack<Transform>();
          stack.Push(tr);



          while (stack.Count > 0)
          {
              Transform current = stack.Pop();

              // Check if the current parent game object has the specified tag
              if (current.CompareTag(tag))
              {
                  // Tag the children of the current parent game object with the specified tag
                  foreach (Transform child in current)
                  {
                      if (child.gameObject.CompareTag("Untagged") || child.GetComponent<SpriteRenderer>() != null)
                      {
                          child.gameObject.tag = tag;
                      }
                  }
              }

              // Push the children of the current parent game object to the stack
              for (int j = 0; j < current.childCount; j++)
              {
                  stack.Push(current.GetChild(j));
              }
          }              
            
      // }   
  }


  private void GetSprites(GameObject root, List<GameObject> spriteList) // gets the sprites and adds to lists
  {
      if(root != null)
      {
          Stack<GameObject> stack = new Stack<GameObject>();
          stack.Push(root);

          while (stack.Count > 0)
          {
              GameObject currentNode = stack.Pop();
              SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

              if (sr != null)
              {
                  spriteList.Add(currentNode);

                  Color initialObjColor = sr.color;
                  initialAlphaFloatList.Add(initialObjColor.a);
                  Color fadedAlphaObjColor = sr.color;
                  fadedAlphaObjColor.a = defaultFadeAlphaFloat;
                  fadedAlphaFloatList.Add(fadedAlphaObjColor.a);              
              }

              foreach (Transform child in currentNode.transform)
              {
                  stack.Push(child.gameObject);
              }
          }
      }
  }

 }
