using System.Collections.Generic;
using UnityEngine;

public class InfiniteIsoScroller : MonoBehaviour
{
    public List<Transform> chunks;

    public float scrollSpeed = 1f;

    public Vector3 moveDirection =
        new Vector3(1f, -0.5f, 0f);

    Vector3 startPos;

    float recycleX;

    void Start()
    {
        // rightmost chunk start position
        recycleX =
            chunks[chunks.Count - 1].position.x +
            Mathf.Abs(chunks[chunks.Count - 1].position.x);
        startPos =
            GetLeftmostChunk().position;
    }

    void Update()
    {
        Vector3 movement =
            moveDirection.normalized *
            scrollSpeed *
            Time.deltaTime;

        foreach (Transform chunk in chunks)
        {
            chunk.position += movement;

            // recycle after FULL extra chunk distance
            if (chunk.position.x > recycleX)
            {
                chunk.position =
                    startPos +
                    new Vector3(-Mathf.Abs(2*(startPos.x)), Mathf.Abs(startPos.y), 0);
                    // GetLeftmostChunk().position;
            }
        }
    }

    Transform GetLeftmostChunk()
    {
        Transform best = chunks[0];

        foreach (Transform chunk in chunks)
        {
            if (chunk.position.x < best.position.x)
            {
                best = chunk;
            }
        }

        return best;
    }
}