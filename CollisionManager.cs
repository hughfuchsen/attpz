using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Runtime Lists (auto-filled)")]
    public List<GameObject> allNpcs = new List<GameObject>();
    public List<Collider2D> inclineColliders = new List<Collider2D>();

    public GameObject player;

    void Awake()
    {
        CollectNPCs();
        CollectInclineColliders();
        IgnoreNpcInclineCollisions();
        IgnorePlayerInclineCollisions();
    }

    // --------------------------------------------
    // 1. Collect all NPCs in the scene
    // --------------------------------------------
    void CollectNPCs()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        allNpcs.AddRange(npcs);
    }

    // --------------------------------------------
    // 2. Collect all incline threshold colliders 
    // --------------------------------------------
    void CollectInclineColliders()
    {
        var scripts = FindObjectsOfType<InclineThresholdColliderScript>();

        foreach (var s in scripts)
        {
            Collider2D col = s.GetComponent<Collider2D>();
            if (col != null)
                inclineColliders.Add(col);
        }
    }

    // --------------------------------------------
    // 3. Ignore collisions NPC <-> incline colliders
    // --------------------------------------------
    void IgnoreNpcInclineCollisions()
    {
        foreach (GameObject npc in allNpcs)
        {
            // Find the child collider tagged NPCCollider
            BoxCollider2D npcCol = npc.GetComponentInChildren<BoxCollider2D>(true);

            if (npcCol == null || !npcCol.CompareTag("NPCCollider"))
                continue;

            // Ignore collision between this NPC and every incline collider
            foreach (var inclineCol in inclineColliders)
            {
                Physics2D.IgnoreCollision(npcCol, inclineCol, true);
            }
        }
    }
    void IgnorePlayerInclineCollisions()
    {
            player = GameObject.FindGameObjectWithTag("Player");

            // Find the child collider tagged NPCCollider
            BoxCollider2D playerCol = player.GetComponentInChildren<BoxCollider2D>(true);

            if (playerCol == null || !playerCol.CompareTag("PlayerCollider"))
                return;

            // Ignore collision between this NPC and every incline collider
            foreach (var inclineCol in inclineColliders)
            {
                if(inclineCol.GetComponent<InclineThresholdColliderScript>().building != null)
                Physics2D.IgnoreCollision(playerCol, inclineCol, true);
            }
    }
}
