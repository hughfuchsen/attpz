using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRaiseLower : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, -30, 0);
    public float speed = 300;
    private Vector3 loweredPosition = new Vector3();
    private Vector3 raisedPosition = new Vector3();
    private bool shouldBeUp = true;


    public void lower()
    {
        shouldBeUp = false;
    }

    public void raise()
    {
        shouldBeUp = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        loweredPosition = this.gameObject.transform.position + offset;
        raisedPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeUp && (this.gameObject.transform.position != raisedPosition))
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, raisedPosition, speed * Time.deltaTime);

        }
        else if (!shouldBeUp && (this.gameObject.transform.position != loweredPosition))
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, loweredPosition, speed * Time.deltaTime);
        }
    }
}
