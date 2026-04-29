using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] GameObject smallComb;
    [SerializeField] GameObject largeComb;
    [SerializeField] GameObject wing = null;

    void Start()
    {
        int choice = Random.Range(0, 3); // 0, 1, or 2 (equal chance)

        var smallCombSR = smallComb.GetComponent<SpriteRenderer>();
        var largeCombSR = largeComb.GetComponent<SpriteRenderer>();

        if(wing != null)
        {
            var wingSR = wing.GetComponent<SpriteRenderer>();
            SetAlpha(wingSR, 0f);
        }

        switch (choice)
        {
            case 0: // small comb only
                SetAlpha(largeCombSR, 0f);
                SetAlpha(smallCombSR, 1f);
                break;

            case 1: // medium (both visible)
                SetAlpha(largeCombSR, 1f);
                SetAlpha(smallCombSR, 1f);
                break;

            case 2: // large comb (big + stretched small)
                SetAlpha(largeCombSR, 1f);
                SetAlpha(smallCombSR, 1f);

                Vector3 scale = smallComb.transform.localScale;
                scale.y = 2f;
                smallComb.transform.localScale = scale;
                break;
        }
    }

    void SetAlpha(SpriteRenderer sr, float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}