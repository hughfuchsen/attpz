using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 lastKeyPressed;
    public float speed;
    public Rigidbody2D myRigidbody;

    public string motionDirection = "normal";
    public Vector3 change;
    public Animator animator;

    private bool shouldAnimate;

    public bool isPlayerInside;


    // Start is called before the first frame update
    void Start()    
      {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        shouldAnimate = true;
      }

    // Update is called once per frame
    void Update()
      {
        UpdateAnimationAndMove();
      }

    public void UpdateAnimationAndMove()
      {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(change != Vector3.zero)
        {
          if(motionDirection == "normal") 
            {MoveCharacter();} 
          else if (motionDirection == "inclineLeftAway")
            {MoveCharacterVerticalInclineLeftAway();} 
          else if (motionDirection == "inclineRightAway") 
            {MoveCharacterVerticalInclineRightAway();}
          else if (motionDirection == "inclineLeftToward") 
            {MoveCharacterVerticalInclineLeftToward();}
          else if (motionDirection == "inclineRightToward") 
            {MoveCharacterVerticalInclineRightToward();}
          else if (motionDirection == "backSlash") 
            {MoveCharacterBackSlashDirection();}
          else if (motionDirection == "forwardSlash") 
            {MoveCharacterForwardSlashDirection();}

          if(shouldAnimate) {
          animator.SetFloat("moveX", change.x);
          animator.SetFloat("moveY", change.y);
          animator.SetBool("moving", true);
          }
        }
        else
        {
          animator.SetBool("moving", false);
        }
      }

      public void UpdateAnimationAndMoveOnVerticalInclineLeft()
      {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(change != Vector3.zero)
        {
          MoveCharacterVerticalInclineLeftAway
          ();
          animator.SetFloat("moveX", change.x);
          animator.SetFloat("moveY", change.y);
          animator.SetBool("moving", true);
        }
        else
        {
          animator.SetBool("moving", false);
        }
      }

      public void UpdateAnimationAndMoveOnVerticalInclineRight()
      {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(change != Vector3.zero)
        {
          MoveCharacterVerticalInclineRightAway();
          animator.SetFloat("moveX", change.x);
          animator.SetFloat("moveY", change.y);
          animator.SetBool("moving", true);
        }
        else
        {
          animator.SetBool("moving", false);
        }
      }
        void MoveCharacterVerticalInclineLeftAway()
    {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-0.8f,1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-0.8f,1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(0.8f,-1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(0.8f,-1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }

        void MoveCharacterVerticalInclineRightAway()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(0.8f,1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-0.8f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(0.8f,1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(0.8f,-1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-0.8f,-1f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-0.8f,-1,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
        void MoveCharacterVerticalInclineLeftToward()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }

        void MoveCharacterVerticalInclineRightToward()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-1f,-0.2f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }



 
        void MoveCharacterBackSlashDirection()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(0f,0f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
       animator.SetBool("moving", false);       
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(0f,0f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
       animator.SetBool("moving", false);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(0f,0f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);       
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(0f,0f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);       
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
 
        void MoveCharacterForwardSlashDirection()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
 
        void MoveCharacter()
   {
      if (change == Vector3.right+Vector3.up)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.up)
      {
       change = new Vector3(-1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right)
      {
       change = new Vector3(1f,0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.right+Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left+Vector3.down)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.down)
      {
       change = new Vector3(1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      if (change == Vector3.left)
      {
       change = new Vector3(-1f,-0.5f,0f);
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
      else
      {
       myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
 

}
