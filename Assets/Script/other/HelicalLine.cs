using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicalLine : MonoBehaviour
{
    public Transform target;

    public float a = -1.0f;
    public float b = 5.0f;
    public float c = 0.0f;

    public float aAmplitude = 1.0f; // Amplitude of the sine wave
    public float aFrequency = 1.0f; // Frequency of the sine wave

    private float tt = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tt += Time.deltaTime;
        a = aAmplitude * Mathf.Sin(aFrequency * Time.time);
        b = aAmplitude * Mathf.Sin(aFrequency * Time.time*0.5f);
        b += 5;
        if (target)
        {
            transform.position = target.position + new Vector3(GetX(tt), GetY(tt), 0);
        }
        else
            transform.position = new Vector3(GetX(tt), GetY(tt), 0);
    }

    private float GetX(float t)
    {
        return a * Mathf.Cos(b * t);
    }
    private float GetY(float t)
    {
        return a * Mathf.Sin(b * t);
    }
    private float GetZ(float t)
    {
        return c * t;
    }
}
