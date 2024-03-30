using UnityEngine;
using System.Collections;

public class SunCycle : MonoBehaviour

{
    public Material material;
    public float cycleDuration = 10f;
    private float timer = 0f;
    private bool isDay = true;

    void Update()
    {
        timer += Time.deltaTime;

        if (isDay)
        {
            float brightness = Mathf.Lerp(1f, 0.4f, timer / cycleDuration);
            material.SetFloat("_Brightness", brightness);
            if (timer >= cycleDuration)
            {
                timer = 0f;
                isDay = false;
            }
        }
        else
        {
            float brightness = Mathf.Lerp(0.4f, 1f, timer / cycleDuration);
            material.SetFloat("_Brightness", brightness);
            if (timer >= cycleDuration)
            {
                timer = 0f;
                isDay = true;
            }
        }
    }
}
