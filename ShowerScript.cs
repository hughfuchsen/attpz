using UnityEngine;
using System.Collections;

public class ShowerScript : MonoBehaviour
{
    public GameObject dropletPrefab;         // assign in Inspector
    public Transform dropletSpawnPoint;      // where droplets spawn from
    public float groundY = -5f;              // height where droplets reset
    public float spawnInterval = 0.1f;       // time between droplet spawns

    private bool isShowering = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider") && !isShowering)
        {
            StartCoroutine(ShowerCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            isShowering = false;
            // StopAllCoroutines();
        }
    }

    private IEnumerator ShowerCoroutine()
    {
        isShowering = true;

        while (isShowering)
        {
            GameObject droplet = Instantiate(dropletPrefab, dropletSpawnPoint.position, Quaternion.identity);
            Debug.Log(droplet.transform.position);

            // Get the SpriteRenderer component
            SpriteRenderer sr = droplet.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color original = sr.color;
                Color.RGBToHSV(original, out float h, out float s, out float v);

                // Offset V by ±0.05, clamped between 0 and 1
                float vOffset = Random.Range(-0.2f, 0.2f);
                float newV = Mathf.Clamp01(v + vOffset);

                Color newColor = Color.HSVToRGB(h, s, newV);
                sr.color = newColor;
            }

            StartCoroutine(MoveDroplet(droplet));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveDroplet(GameObject droplet)
    {
        while (droplet != null && droplet.transform.localPosition.y > droplet.transform.localPosition.y - groundY)
        {
            int step = Random.Range(2, 3);
            droplet.transform.position += Vector3.down * step;
            yield return new WaitForSeconds(0.05f);
        }

        if (droplet != null)
            Destroy(droplet);
    }
}
