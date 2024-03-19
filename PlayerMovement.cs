using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
  public float speed;
  public Rigidbody2D myRigidbody;

  public string motionDirection = "normal";
  public Vector3 change;
  public Animator animator;

  IsoSpriteSorting IsoSpriteSorting; 

  public bool fixedDirectionLeft;
  public bool fixedDirectionRight;
  
  public bool playerIsOutside = false;

  public bool playerIsInside()
  {
    if (!playerIsOutside)
    {
      return true;
    }
    return false;
  }
  void Animate() {
    animator.SetFloat("moveX", change.x);
    animator.SetFloat("moveY", change.y);
    animator.SetBool("moving", true);
  }

  // Start is called before the first frame update
  void Start()    
    {
      IsoSpriteSorting = GetComponent<IsoSpriteSorting>();
      animator = GetComponent<Animator>();
      myRigidbody = GetComponent<Rigidbody2D>();
      fixedDirectionLeft = false;
      fixedDirectionRight = false;
      playerIsOutside = true;
    }

  // Update is called once per frame
  void FixedUpdate()
    {
      UpdateAnimationAndMove();

      // Check if the spacebar is held down
      if (Input.GetKey(KeyCode.Space))
      {
          // Execute your code here
          Debug.Log("Spacebar is held down!");
      }   
    }

  public void UpdateAnimationAndMove()
    {
      change = Vector3.zero;
      change.x = Input.GetAxisRaw("Horizontal");
      change.y = Input.GetAxisRaw("Vertical");
      if(change != Vector3.zero)
      {
        if(motionDirection == "normal") {
            MoveCharacter();} 
        else if (motionDirection == "inclineLeftAway") {
            MoveCharacterVerticalInclineLeftAway();} 
        else if (motionDirection == "inclineRightAway") 
          {MoveCharacterVerticalInclineRightAway();}
        else if (motionDirection == "inclineLeftToward") 
          {MoveCharacterVerticalInclineLeftToward();}
        else if (motionDirection == "inclineRightToward") 
          {MoveCharacterVerticalInclineRightToward();}

        // // if(shouldAnimate) {
        //   Animate();
        // // }
      }
      else
      {
        animator.SetBool("moving", false);
      }
    }

    // public void UpdateAnimationAndMoveOnVerticalInclineLeft()
    // {
    //   change = Vector3.zero;
    //   change.x = Input.GetAxisRaw("Horizontal");
    //   change.y = Input.GetAxisRaw("Vertical");
    //   if(change != Vector3.zero)
    //   {
    //     MoveCharacterVerticalInclineLeftAway();
    //     animator.SetFloat("moveX", change.x);
    //     animator.SetFloat("moveY", change.y);
    //     animator.SetBool("moving", true);
    //   }
    //   else
    //   {
    //     animator.SetBool("moving", false);
    //   }
    // }

    // public void UpdateAnimationAndMoveOnVerticalInclineRight()
    // {
    //   change = Vector3.zero;
    //   change.x = Input.GetAxisRaw("Horizontal");
    //   change.y = Input.GetAxisRaw("Vertical");
    //   if(change != Vector3.zero)
    //   {
    //     MoveCharacterVerticalInclineRightAway();
    //     animator.SetFloat("moveX", change.x);
    //     animator.SetFloat("moveY", change.y);
    //     animator.SetBool("moving", true);
    //   }
    //   else
    //   {
    //     animator.SetBool("moving", false);
    //   }
    // }
      void MoveCharacterVerticalInclineLeftAway()
  {
    if(!fixedDirectionLeft)
    {
      if (change == Vector3.right+Vector3.up)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.up)
      {
      change = new Vector3(-0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.up)
      {
      change = new Vector3(-0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right+Vector3.down)
      {
      change = new Vector3(0.7f,-1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.down)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.down)
      {
      change = new Vector3(0.7f,-1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      else
      {
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
    }
    else
    // fixedDirectionLeft
    {
      if (change == Vector3.right+Vector3.up)
        {
        change = new Vector3(-1f,0.5f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        animator.SetBool("moving", false); 
        }
        if (change == Vector3.left+Vector3.up)
        {
        change = new Vector3(-0.7f,1f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        Animate();
        }
        if (change == Vector3.up)
        {
        change = new Vector3(-0.7f,1f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        Animate();
        }
        if (change == Vector3.right)
        {
        change = new Vector3(-1f,0.5f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        animator.SetBool("moving", false); 
        }
        if (change == Vector3.right+Vector3.down)
        {
        change = new Vector3(0.7f,-1f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        Animate();
        }
        if (change == Vector3.left+Vector3.down)
        {
        change = new Vector3(1f,-0.5f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        animator.SetBool("moving", false); 
        }
        if (change == Vector3.down)
        {
        change = new Vector3(0.7f,-1f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        Animate();
        }
        if (change == Vector3.left)
        {
        change = new Vector3(1f,-0.5f,0f);
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        animator.SetBool("moving", false); 
        }
        else
        {
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        Animate();
        }      
    } 

  }

  void MoveCharacterVerticalInclineRightAway()
  {
    if(!fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)
      {
      change = new Vector3(0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.up)
      {
      change = new Vector3(-1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.up)
      {
      change = new Vector3(-0.7f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right)
      {
      change = new Vector3(0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right+Vector3.down)
      {
      change = new Vector3(0.7f,-1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.down)
      {
      change = new Vector3(-0.7f,-1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.down)
      {
      change = new Vector3(1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left)
      {
      change = new Vector3(-0.7f,-1,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      else
      {
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
    else
    {
      if (change == Vector3.right+Vector3.up)
      {
      change = new Vector3(0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.up)
      {
      change = new Vector3(0f,0f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false); 
      }
      if (change == Vector3.up)
      {
      change = new Vector3(0f,0f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false); 
      }
      if (change == Vector3.right)
      {
      change = new Vector3(0.7f,1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right+Vector3.down)
      {
      change = new Vector3(0f,0f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false); 
      }
      if (change == Vector3.left+Vector3.down)
      {
      change = new Vector3(-0.7f,-1f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.down)
      {
      change = new Vector3(0f,0f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false); 
      }
      if (change == Vector3.left)
      {
      change = new Vector3(-0.7f,-1,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      else
      {
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
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
  void MoveCharacter()
  { 
    if(!fixedDirectionLeft && !fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); }
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
    else if(fixedDirectionLeft)
    {
      if (change == Vector3.right+Vector3.up)
      {
      change = new Vector3(-1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);       
      }
      if (change == Vector3.left+Vector3.up)
      {
      change = new Vector3(-1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.up)
      {
      change = new Vector3(-1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.right)
      {
      change = new Vector3(-1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);
      }
      if (change == Vector3.right+Vector3.down)
      {
      change = new Vector3(1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.down)
      {
      change = new Vector3(1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);       
      }
      if (change == Vector3.down)
      {
      change = new Vector3(1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left)
      {
      change = new Vector3(1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);       
      }
      else
      {
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }     
    }
  else if (fixedDirectionRight)  
    {
      if (change == Vector3.right+Vector3.up)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.left+Vector3.up)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);  
      }
      if (change == Vector3.up)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);  
      }
      if (change == Vector3.right)
      {
      change = new Vector3(1f,0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate(); 
      }
      if (change == Vector3.right+Vector3.down)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);  
      }
      if (change == Vector3.left+Vector3.down)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      if (change == Vector3.down)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      animator.SetBool("moving", false);  
      }
      if (change == Vector3.left)
      {
      change = new Vector3(-1f,-0.5f,0f);
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
      }
      else
      {
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      }
    }
  }


}
