using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFadeTrigger : MonoBehaviour

{
  CharacterMovement myCharacterMovement;
  GameObject Player;
  public GameObject foliage;
  public GameObject canopy;
  public float defaultFadeAlphaFloat = 0.15f;
  private Coroutine leafFadeCoro;
  private Coroutine canopyFadeCoro;
  private Coroutine resetTagsCoro;

  private List<float> initialFoliageAlphaFloatList = new List<float>();
  private List<float> initialCanopyAlphaFloatList = new List<float>();
  private List<float> fadedFoliageFloatList = new List<float>();
  private List<float> fadedCanopyFloatList = new List<float>();
  private List<GameObject> foliageListToFade = new List<GameObject>();
  private List<GameObject> canopyListToFade = new List<GameObject>();




  void Awake()
  {
      Player = GameObject.FindGameObjectWithTag("Player");
      myCharacterMovement = Player.GetComponent<CharacterMovement>();


      if (foliage != null)
      {
          GetSpritesAndApplyToLists(foliage, foliageListToFade, initialFoliageAlphaFloatList, fadedFoliageFloatList);
          leafFadeCoro = StartCoroutine(SetTreeAlpha(foliageListToFade, initialFoliageAlphaFloatList, false));
      }
      if (canopy != null)
      {
          GetSpritesAndApplyToLists(canopy, canopyListToFade, initialCanopyAlphaFloatList, fadedCanopyFloatList);
          canopyFadeCoro = StartCoroutine(SetTreeAlpha(canopyListToFade, initialCanopyAlphaFloatList, false));
      }

  }

  void OnTriggerEnter2D(Collider2D other)
  {
      if (other.CompareTag("PlayerCollider"))
      {
          StopAllCoros();

          if (myCharacterMovement.playerOnBuildingThresh == false) // if player is not on building threshold
          {
              leafFadeCoro = StartCoroutine(SetTreeAlpha(foliageListToFade, fadedFoliageFloatList, false));
              canopyFadeCoro = StartCoroutine(SetTreeAlpha(canopyListToFade, null, false));

          }
          else
          {
              transform.parent.tag = "AlphaZeroEntExt";
              TagChildrenOfTaggedParents("AlphaZeroEntExt");
          }
      }
  }
  void OnTriggerExit2D(Collider2D other)
  {
      if (other.CompareTag("PlayerCollider"))
      {
          if (myCharacterMovement.playerOnBuildingThresh == false)
          {
              // leafFadeCoro = StartCoroutine(SetTreeAlpha(foliageListToFade, initialFoliageAlphaFloatList, true));
              if (!myCharacterMovement.playerOnFurniture)
              {
                  leafFadeCoro = StartCoroutine(SetTreeAlpha(foliageListToFade, initialFoliageAlphaFloatList, true));
                  canopyFadeCoro = StartCoroutine(SetTreeAlpha(canopyListToFade, initialCanopyAlphaFloatList, true));
              }
              transform.parent.tag = "Untagged";
              TagChildrenOfTaggedParents("Untagged");
          }
          else
          {
              transform.parent.tag = "AlphaZeroEntExt";
              TagChildrenOfTaggedParents("AlphaZeroEntExt");
              resetTagsCoro = StartCoroutine(resetTagsToUntaggedAfterWait()); //maybe something to do with balcony door?
          }
      }
  }


  IEnumerator SetTreeAlpha(List<GameObject> spriteList, List<float> fadeTo = null, bool shouldWait = false)
  {
      if (shouldWait)
          yield return new WaitForSeconds(0.3f);

      for (int i = 0; i < spriteList.Count; i++)
      {
          SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
          if (sr != null)
          {
              float alpha = (fadeTo != null && i < fadeTo.Count)
                  ? fadeTo[i]
                  : 1f;

              sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
          }
      }

      yield return null;
  }


  IEnumerator resetTagsToUntaggedAfterWait()
  {
      yield return new WaitForSeconds(2f);
      foliage.tag = "Untagged";
      TagChildrenOfTaggedParents("Untagged");
      yield return null;
  }

  void StopAllCoros()
  {
      if (leafFadeCoro != null)
      {
          StopCoroutine(leafFadeCoro);
      }
      if (canopyFadeCoro != null)
      {
          StopCoroutine(canopyFadeCoro);
      }
      if (resetTagsCoro != null)
      {
          StopCoroutine(resetTagsCoro);
      }
  }

  private void TagChildrenOfTaggedParents(string tag)
  {
      // for (int i = 0; i < spriteList.Count; i++)
      // {
      Transform tr = this.transform.parent;

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


  private void GetSpritesAndApplyToLists(
    GameObject root,
    List<GameObject> spriteList,
    List<float> initialAlphaList,
    List<float> fadedAlphaList
  )
  {
      if (root == null) return;

      Stack<GameObject> stack = new Stack<GameObject>();
      stack.Push(root);

      while (stack.Count > 0)
      {
          GameObject currentNode = stack.Pop();
          SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

          if (sr != null)
          {
              spriteList.Add(currentNode);

              float initialAlpha = sr.color.a;
              initialAlphaList.Add(initialAlpha);

              fadedAlphaList.Add(defaultFadeAlphaFloat);
          }

          foreach (Transform child in currentNode.transform)
          {
              stack.Push(child.gameObject);
          }
      }
  }
}
