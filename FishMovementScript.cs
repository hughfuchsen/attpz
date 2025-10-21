
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FishNPCMovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public PolygonCollider2D boundary;
    public float moveInterval = 0.2f;
    public float directionChangeChance = 0.1f;

    [Header("Sprite Settings")]
    public Sprite cardinalSprite;       // up/down/left/right
    public Sprite diagonalSprite;       // diagonal

    [Header("Allowed Movement Directions")]
    public bool allowCardinal = true;
    public bool allowDiagonalLeft = true;
    public bool allowDiagonalRight = true;

    [Header("Cardinal Sprite Flip")]
    public bool flipCardinalX = true;
    public bool flipCardinalY = true;

    [Header("Diagonal Sprite Flip")]
    public bool flipDiagonalX = true;
    public bool flipDiagonalY = true;

    [Header("Sprite Behavior")]
    public bool forceCardinalSprite = false; // always show cardinal sprite even on diagonal movement

    [Header("Organic Movement")]
    public float stopChance = 0.1f;
    public float minStopTime = 0.2f;
    public float maxStopTime = 1f;

    private Vector2Int[] allDirections = new Vector2Int[]
    {
        new Vector2Int(1, 0),   // right
        new Vector2Int(-1, 0),  // left
        new Vector2Int(0, 1),   // up
        new Vector2Int(0, -1),  // down
        new Vector2Int(1, 1),   // up-right
        new Vector2Int(1, -1),  // down-right
        new Vector2Int(-1, 1),  // up-left
        new Vector2Int(-1, -1)  // down-left
    };

    private Vector2Int currentDir;
    private float moveTimer;
    private float idleTimer = 0f;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        float h = Random.value;
        float s = Random.Range(0.5f, 1f);
        float v = Random.Range(0.7f, 1f);
        sr.color = Color.HSVToRGB(h, s, v);
        PickRandomDirection();
    }

    private void Update()
    {
        // Handle idle / stop
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            moveTimer = moveInterval;

            // random stop
            if (Random.value < stopChance)
            {
                idleTimer = Random.Range(minStopTime, maxStopTime);
                PickRandomDirection(); // optionally pick new dir while idle
                return;
            }

            StepMove();
        }
    }

    private void StepMove()
    {
        Vector2 newPos = (Vector2)transform.position + (Vector2)currentDir;

        if (boundary != null && !IsSpriteFullyInside(newPos))
        {
            currentDir *= -1;
            newPos = (Vector2)transform.position + (Vector2)currentDir;

            if (!IsSpriteFullyInside(newPos))
                newPos = transform.position;
        }

        // Snap to pixel grid (1 unit = 1 pixel)
        transform.position = new Vector2(
            Mathf.Round(newPos.x),
            Mathf.Round(newPos.y)
        );

        UpdateSpriteAndFlip();
    }

    private bool IsSpriteFullyInside(Vector2 position)
    {
        if (boundary == null || sr == null) return true;

        Vector2 half = sr.bounds.extents;
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(-half.x, -half.y),
            new Vector2(-half.x,  half.y),
            new Vector2( half.x, -half.y),
            new Vector2( half.x,  half.y)
        };

        foreach (var offset in offsets)
        {
            if (!boundary.OverlapPoint(position + offset))
                return false;
        }

        return true;
    }

    private void PickRandomDirection()
    {
        List<Vector2Int> validDirs = new List<Vector2Int>();

        foreach (var dir in allDirections)
        {
            bool isDiagonal = dir.x != 0 && dir.y != 0;
            bool isCardinal = dir.x == 0 || dir.y == 0;

            if (isDiagonal)
            {
                if (dir.x > 0 && allowDiagonalRight) validDirs.Add(dir);
                if (dir.x < 0 && allowDiagonalLeft) validDirs.Add(dir);
            }
            else if (isCardinal)
            {
                if (allowCardinal) validDirs.Add(dir);
            }
        }

        if (validDirs.Count == 0)
        {
            Debug.LogWarning($"{name} has no allowed movement directions!");
            currentDir = Vector2Int.zero;
            return;
        }

        currentDir = validDirs[Random.Range(0, validDirs.Count)];
    }

    private void UpdateSpriteAndFlip()
    {
        bool isDiagonal = currentDir.x != 0 && currentDir.y != 0;

        // Choose sprite
        if (forceCardinalSprite)
        {
            sr.sprite = cardinalSprite;
        }
        else
        {
            sr.sprite = isDiagonal ? diagonalSprite : cardinalSprite;
        }

        // Apply flips
        if (isDiagonal)
        {
            if (forceCardinalSprite)
            {
                if (flipCardinalX) sr.flipX = currentDir.x > 0;
                if (flipCardinalY) sr.flipY = currentDir.y < 0;
            }
            else
            {
                if (flipDiagonalX) sr.flipX = currentDir.x > 0;
                if (flipDiagonalY) sr.flipY = currentDir.y < 0;
            }
        }
        else
        {
            if (flipCardinalX) sr.flipX = currentDir.x > 0;
            if (flipCardinalY) sr.flipY = currentDir.y < 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.yellow;
        Vector2 center = sr.bounds.center;
        Vector2 size = sr.bounds.size;
        Gizmos.DrawWireCube(center, size);
    }
}
