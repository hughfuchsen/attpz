using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraWalkThrough : MonoBehaviour
{
    [SerializeField] GameObject Player;
    CharacterMovement characterMovement;
    [SerializeField] BoxCollider2D characterCollider;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
        // characterMovement = Player.GetComponent<CharacterMovement>();        
        // characterCollider = Player.GetComponent<BoxCollider2D>();
        StartCoroutine(RandomSway());
    }

    void Update()
    {

    }

    void Animate(string state) 
    {
        animator.SetTrigger(state);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider" || collision.gameObject.tag == "NPCCollider" )
        {
            Bounds playerBounds = collision.bounds;

            // Find the point of contact
            Vector2 contactPoint = collision.ClosestPoint(transform.position);

            // Define regions within the player's collider
            Vector2 center = playerBounds.center;
            Vector2 max = playerBounds.max;
            Vector2 min = playerBounds.min;

            // Check if the contact point is within the top-right quarter
            if (contactPoint.x >= center.x && contactPoint.y >= center.y)
            {
                // Execute your desired code here for top-right 
                Animate("BendRight");
            }
            // Check if the contact point is within the top-left quarter
            else if (contactPoint.x <= center.x && contactPoint.y >= center.y)
            {
                // Execute your desired code here for top-left
                Animate("BendLeft");
            }
            // Check if the contact point is within the bottom-left quarter
            else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
            {
                // Execute your desired code here for bottom-left
                Animate("BendLeft");
            }
            // Check if the contact point is within the bottom-right quarter
            else if (contactPoint.x >= center.x && contactPoint.y <= center.y)
            {
                // Execute your desired code here for bottom-right
                Animate("BendRight");
            }
            
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // if (collision.gameObject.tag == "Player")
        // {
            Animate("Idle");
        // }
    }








    private IEnumerator RandomSway()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 250f));
            Animate("Sway");
            yield return new WaitForSeconds(5f);
            Animate("Idle");
        }
    }



    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().change.x < 0;
    }
}
