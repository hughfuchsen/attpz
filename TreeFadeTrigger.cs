using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFadeTrigger : MonoBehaviour

{
  public GameObject gameObject;
  public float fadeSpeed = 10;
  public float alpha = 0.15f;
  private Coroutine fadeCoro;

  void Start()
  {
    if (gameObject != null)
    {
      setTreeAlpha(gameObject, 1);
    }
  }

  void OnTriggerEnter2D()
  {
    if (fadeCoro != null)
    {
        StopCoroutine(fadeCoro);
    }   

    fadeCoro = StartCoroutine(treeFade(gameObject, 1, alpha, fadeSpeed));
  }
  void OnTriggerExit2D()
  {
    if (fadeCoro != null)
    {
        StopCoroutine(fadeCoro);
    }   

    fadeCoro = StartCoroutine(treeFade(gameObject, alpha, 1, fadeSpeed));
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
 }
