using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnScript : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        BoxCollider2D col = GetComponentInChildren<BoxCollider2D>();
        IsoSpriteSorting iss = GetComponent<IsoSpriteSorting>();

        Bounds bounds = sprites[0].bounds;
        foreach (var sr in sprites)
        {
            bounds.Encapsulate(sr.bounds);
        }

        float boundsBottomY = bounds.min.y;
        
        if(col != null) 
        {
            float colBottomY = col.bounds.min.y;

            // --- COLLIDER FIX
            float delta = boundsBottomY - colBottomY;
            float localDelta = delta / col.transform.lossyScale.y;
            col.offset += new Vector2(0f, localDelta);
        }

        // --- ISS FIX (world → local)
        float localBottomY = transform.InverseTransformPoint(
            new Vector3(0f, boundsBottomY, 0f)
        ).y;

        iss.SorterPositionOffset = new Vector3(iss.SorterPositionOffset.x, localBottomY, 0f);
    }
}
