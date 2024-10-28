using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFadeTrigger : MonoBehaviour

{
  PlayerMovement playerMovement;
  [SerializeField] GameObject Player;  
  public GameObject obj;
  public float alpha = 0.15f;
  private Coroutine fadeCoro;
  private Coroutine resetTagsCoro;

  private List<float> initialAlphaFloatList = new List<float>();
  private List<float> fadedAlphaFloatList = new List<float>();
  private List<GameObject> objListToFade = new List<GameObject>();

  void Awake()
  {
    Player = GameObject.FindGameObjectWithTag("Player");
    playerMovement = Player.GetComponent<PlayerMovement>(); 
    

    if (obj != null)
    {
      GetSprites(obj, objListToFade);
      setTreeAlpha(gameObject, 1);
    }

    //populate the objListToFade list
    for (int i = 0; i < objListToFade.Count; i++)
    {
        Color initialObjColor = objListToFade[i].GetComponent<SpriteRenderer>().color;
        initialAlphaFloatList.Add(initialObjColor.a);
        Color fadedAlphaObjColor = objListToFade[i].GetComponent<SpriteRenderer>().color;
        fadedAlphaObjColor.a = alpha;
        fadedAlphaFloatList.Add(fadedAlphaObjColor.a);

    }  
  }

  void OnTriggerEnter2D()
  {
    StopAllCoros();

    if(playerMovement.playerOnThresh == false) // if player is not on building threshold
    {
      fadeCoro = StartCoroutine(treeFade(objListToFade, fadedAlphaFloatList));
    }
    else
    {
      obj.tag = "AlphaZeroEntExt";
      TagChildrenOfTaggedParents("AlphaZeroEntExt");
    }
  }
  void OnTriggerExit2D()
  {
    StopAllCoros();

    if(playerMovement.playerOnThresh == false)
    {
      fadeCoro = StartCoroutine(treeFade(objListToFade, initialAlphaFloatList));
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


  void setTreeAlpha(GameObject treeNode, float alpha) 
  {
    SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
    if ((sr != null))
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
  }   
  
  IEnumerator treeFade(
  List<GameObject> spriteList,
  // List<float> fadeFrom,
  List<float> fadeTo) 
  {
    for (int i = 0; i < spriteList.Count; i++)
    {
      // for (float t = 0.0f; t < 1; t += Time.deltaTime) 
      // {
        // float currentAlpha = Mathf.Lerp(fadeFrom[i], fadeTo[i], t * 10);
        // setTreeAlpha(spriteList[i], currentAlpha);
        // float currentAlpha = Mathf.Lerp(fadeFrom[i], fadeTo[i], t * 10);
        setTreeAlpha(spriteList[i], fadeTo[i]);
      // }
    }
    yield return null;
  }
  
  IEnumerator resetTagsToUntaggedAfterWait()
  {
    yield return new WaitForSeconds(2f);
    obj.tag = "Untagged";
    TagChildrenOfTaggedParents("Untagged");
    yield return null;
  }

  void StopAllCoros()
  {
      if(fadeCoro != null)
      {
          StopCoroutine(fadeCoro);
      }

      if(resetTagsCoro != null)
      {
          StopCoroutine(resetTagsCoro);
      }

  }

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
              }

              foreach (Transform child in currentNode.transform)
              {
                  stack.Push(child.gameObject);
              }
          }
      }
  }

 }
