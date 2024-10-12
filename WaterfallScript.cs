using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallScript : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine(RandomStart());
    }
    private IEnumerator RandomStart()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 4f));
            animator.SetTrigger("Start"); 
            yield return new WaitForSeconds(Random.Range(0f, 4f));
            // animator.SetTrigger("Stop"); 
            // animator.SetTrigger("Start"); 
        }
  
    }

}
