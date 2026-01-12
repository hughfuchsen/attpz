using UnityEngine;
using System.Collections;

public class ShowerScript : MonoBehaviour
{
    public GameObject dropletPrefab;         // assign in Inspector
    public Transform dropletSpawnPoint;      // where droplets spawn from
    public float groundY = 28f;              // height where droplets reset
    public float spawnInterval = 0.3f;       // time between droplet spawns

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

        // Launch 3 streams
        StartCoroutine(SpawnStream(0));
        StartCoroutine(SpawnStream(2));
        StartCoroutine(SpawnStream(4));

        yield return null; // You could optionally wait until all streams are done, if you limit them
    }

    private IEnumerator SpawnStream(int xOffset)
    {
        while (isShowering)
        {
            Vector3 spawnPos = dropletSpawnPoint.position + new Vector3(xOffset, 0f, 0f);
            GameObject droplet = Instantiate(
                dropletPrefab,
                spawnPos,
                Quaternion.identity,
                transform.parent   // same parent → sibling of this object
            );
            // Color tweak
            SpriteRenderer sr = droplet.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color original = sr.color;
                Color.RGBToHSV(original, out float h, out float s, out float v);
                float vOffset = Random.Range(-0.2f, 0.5f);
                float newV = Mathf.Clamp01(v + vOffset);
                sr.color = Color.HSVToRGB(h, s, newV);
            }

            StartCoroutine(MoveDroplet(droplet));
            yield return new WaitForSeconds(spawnInterval);
        }
    }
        
    private IEnumerator MoveDroplet(GameObject droplet)
    {
        while (droplet != null && droplet.transform.position.y > dropletSpawnPoint.position.y - groundY)
        {
            int step = Random.Range(2, 3);
            int longDrop = Random.Range(1, 4);

            Vector3 scale = droplet.transform.localScale;
            scale.y = longDrop;
            droplet.transform.localScale = scale;

            droplet.transform.position += Vector3.down * step;

            if(droplet.GetComponent<IsoSpriteSorting>() != null)
            {
                droplet.GetComponent<IsoSpriteSorting>().SorterPositionOffset.y += step;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (droplet != null)
        {
            Vector3 puddleScale = new Vector3(3,3,0);
            droplet.transform.localScale = puddleScale;
            if(droplet.GetComponent<IsoSpriteSorting>() != null)
            {
                droplet.GetComponent<IsoSpriteSorting>().SorterPositionOffset.y += 2;
            }
            yield return new WaitForSeconds(2f);
            Destroy(droplet);
        }
    }
}
