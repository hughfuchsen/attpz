using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFadeTrigger : MonoBehaviour

{
  PlayerMovement playerMovement;
  [SerializeField] GameObject Player;  
  public GameObject obj;
  public float fadeSpeed = 10;
  public float alpha = 0.15f;
  private Coroutine fadeCoro;
  private Coroutine resetTagsCoro;


  void Start()
  {
    Player = GameObject.FindGameObjectWithTag("Player");
    playerMovement = Player.GetComponent<PlayerMovement>(); 

    if (obj != null)
    {
      setTreeAlpha(gameObject, 1);
    }
  }

  void OnTriggerEnter2D()
  {
    StopAllCoros();

    if(playerMovement.playerOnThresh == false) // if player is not on building threshold
    {
      fadeCoro = StartCoroutine(treeFade(obj, 1, alpha, fadeSpeed));
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
      fadeCoro = StartCoroutine(treeFade(obj, alpha, 1, fadeSpeed));
      obj.tag = "Untagged";
      TagChildrenOfTaggedParents("Untagged");
    }
    else
    {
      obj.tag = "AlphaZeroEntExt";
      TagChildrenOfTaggedParents("AlphaZeroEntExt");
      resetTagsCoro = StartCoroutine(resetTagsToUntaggedAfterWait());
    }
  }


  void setTreeAlpha(GameObject treeNode, float alpha) 
  {
    SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
    if ((sr != null))
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
    foreach (Transform child in treeNode.transform) 
    {
        setTreeAlpha(child.gameObject, alpha);
    }
  }   
  
  IEnumerator treeFade(
  GameObject gameObject,
  float fadeFrom, 
  float fadeTo,
  float fadeSpeed) 
  {
    for (float t = 0.0f; t < 1; t += Time.deltaTime) 
    {
      float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
      setTreeAlpha(gameObject, currentAlpha);
      yield return null;
    }
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


 }
