using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private string blendTreeName;


  [SerializeField] GameObject Player;
  public List<GameObject> playerSpriteList = new List<GameObject>();
  public List<Color> initialPlayerColorList = new List<Color>();

  public float speed;
  public Rigidbody2D myRigidbody; 

  public string motionDirection = "normal";
  public Vector3 change;
  public Animator animator;

  // private float initialColliderOffsetX;
  // private float initialColliderOffsetY;
  // public float colliderOffsetMovementX;
  // public float colliderOffsetMovementY;
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
      Player = GameObject.FindGameObjectWithTag("Player");
      GetSpritesAndAddToLists(Player, playerSpriteList, initialPlayerColorList);
      IsoSpriteSorting = GetComponent<IsoSpriteSorting>();
      animator = GetComponent<Animator>();
      myRigidbody = GetComponent<Rigidbody2D>();
      fixedDirectionLeft = false;
      fixedDirectionRight = false;
      // playerIsOutside = true;
      // initialColliderOffsetX = Player.GetComponent<BoxCollider2D>().offset.x;
      // initialColliderOffsetY = Player.GetComponent<BoxCollider2D>().offset.y;
    }

  // Update is called once per frame
  void FixedUpdate()
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
        // animator.speed = 0.1f;
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
        else if (motionDirection == "upDownLadder") 
          {MoveCharacterUpDownLadder();}
      }
      else
      {
        // animator.speed = 1f;
        animator.SetBool("moving", false);
      }
    }
      void MoveCharacterVerticalInclineLeftAway()
  {
    if(!fixedDirectionLeft)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-0.7f,1f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(-0.7f,1f,0f); }
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); }
      if (change == Vector3.down)               { change = new Vector3(0.7f,-1f,0f); }
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
    else
    // fixedDirectionLeft
    {    
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-0.7f,1f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(-0.7f,1f,0f); }
      if (change == Vector3.right)              { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.down)               { change = new Vector3(0.7f,-1f,0f); }
      if (change == Vector3.left)               { change = new Vector3(1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    } 
  }

  void MoveCharacterVerticalInclineRightAway()
  {
    if(!fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0.7f,-1f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f);}
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
    else
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0.7f,1f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,0f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(0f,0f,0f); }
      if (change == Vector3.right)              { change = new Vector3(0.7f,1f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,0f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-0.7f,-1f,0f); }
      if (change == Vector3.down)               { change = new Vector3(0f,0f,0f); }
      if (change == Vector3.left)               { change = new Vector3(-0.7f,-1f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
  }
      void MoveCharacterVerticalInclineLeftToward()
  {
    if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.up)                 { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.right)              { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,0.2f,0f); }
    if (change == Vector3.down)               { change = new Vector3(1f,-0.2f,0f); }
    if (change == Vector3.left)               { change = new Vector3(-1f,0.2f,0f); }
    myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    Animate();
  }

      void MoveCharacterVerticalInclineRightToward()
  {
    if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.up)                 { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.right)              { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.2f,0f); }
    if (change == Vector3.down)               { change = new Vector3(1f,0.2f,0f); }
    if (change == Vector3.left)               { change = new Vector3(-1f,-0.2f,0f); }
    myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    Animate();
  } 
  void MoveCharacterUpDownLadder()
  { 
    if(!fixedDirectionLeft && !fixedDirectionRight)
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.right)              { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.down)               { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.left)               { change = new Vector3(0f,-1f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
    else if(fixedDirectionLeft)
    {
    if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,1f,0f); }
    if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,1f,0f); }
    if (change == Vector3.up)                 { change = new Vector3(0f,1f,0f); }
    if (change == Vector3.right)              { change = new Vector3(0f,1f,0f); }
    if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,-1,0f); }
    if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,-1f,0f); }
    if (change == Vector3.down)               { change = new Vector3(0f,-1f,0f); }
    if (change == Vector3.left)               { change = new Vector3(0f,-1f,0f); }
    myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    Animate();
  }
                
  else if (fixedDirectionRight)  
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.right)              { change = new Vector3(0f,1f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.down)               { change = new Vector3(0f,-1f,0f); }
      if (change == Vector3.left)               { change = new Vector3(0f,-1f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
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
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.right)              { change = new Vector3(-1f,0.5f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.down)               { change = new Vector3(1f,-0.5f,0f); }
      if (change == Vector3.left)               { change = new Vector3(1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }              
    else if (fixedDirectionRight)  
    {
      if (change == Vector3.right+Vector3.up)   { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.left+Vector3.up)    { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.up)                 { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.right)              { change = new Vector3(1f,0.5f,0f); }
      if (change == Vector3.right+Vector3.down) { change = new Vector3(-1f,-0.5f,0f); }
      if (change == Vector3.left+Vector3.down)  { change = new Vector3(-1f,-0.5f,0f); }
      if (change == Vector3.down)               { change = new Vector3(-1f,-0.5f,0f); }
      if (change == Vector3.left)               { change = new Vector3(-1f,-0.5f,0f); }
      myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
      Animate();
    }
  }

    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<Color> colorList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color col = sr.color;
                spriteList.Add(currentNode);
                colorList.Add(col);
            }

            foreach (Transform child in currentNode.transform)
            {
                    stack.Push(child.gameObject);
            }
        }
    }

    public void SetAlpha(GameObject treeNode, float alpha) 
    {
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }  

    // public void ModifyBlendTree(int n, float amountX, float amountY)
    // {
    //         // animator.baseLayer.walkingTree
    //         animator.baseLayer.walkingTree.children[n].position = new Vector2(blendTree.children[n].position.x, amountX);
    //         animator.baseLayer.walkingTree.children[n].position = new Vector2(blendTree.children[n].position.y, amountY);
    //         Debug.Log("Modified BlendTree motion position.");
    // }
}



