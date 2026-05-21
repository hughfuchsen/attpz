using System.Collections.Generic;
using UnityEngine;

public class NodeTransportManager : MonoBehaviour
{
    public List<DecisionNode> topNodes;
    public List<DecisionNode> bottomNodes;

    void Start()
    {
        Shuffle(bottomNodes);

        for (int i = 0; i < topNodes.Count; i++)
        {
            topNodes[i].linkedNode = bottomNodes[i];

            // ensure opposite vertical alignment rule is enforced
            if (topNodes[i].nodeColor == bottomNodes[i].nodeColor)
            {
                Debug.LogWarning("Same color vertical match avoided design rule!");
            }
        }
    }

    void Shuffle(List<DecisionNode> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);

            DecisionNode tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}