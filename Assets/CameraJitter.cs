using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraJitter : MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool enablePosition = false;
    public bool enableRotation = false;
    public bool enableFrustum = false;
    public bool gaussian = false;
    public bool pixelScaleAmplitude = true;
    public bool debug = false;
    public float positionAmplitude;
    public float rotationAmplitude;

    private Vector3[] cachedRandomVectors;
    private const int numCachedVectors = 4096;
    private int currentRandomVectorIndex = 0;

    private float horizontalObliquenessPerPixel = 0f;
    private float verticalObliqunessPerPixel = 0f;
    private int frustumPositionIndex = 0;
    private Camera camera;

    public float firstRandomValue;

    public enum FrustumSamplingMode
    {
        Equirectangular,
        Random,
        Gaussian,
        Radial
    }
    public FrustumSamplingMode frustumMode = FrustumSamplingMode.Equirectangular;

    
    public void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Start ()
    {

        cachedRandomVectors = new Vector3[numCachedVectors];
        for (int i = 0; i < numCachedVectors; i++)
        {
            cachedRandomVectors[i].x = NextRandom();
            cachedRandomVectors[i].y = NextRandom();
            cachedRandomVectors[i].z = 0f;
        }

        firstRandomValue = cachedRandomVectors[0].x;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (pixelScaleAmplitude)
        {
            // Should be degrees per pixel divided by two.
            float degreesPerPixel = camera.fieldOfView / camera.pixelHeight;
            rotationAmplitude = degreesPerPixel / 2f;
        }

        SetDimensions(camera.pixelWidth, camera.pixelHeight);

        //for (int i = 0; i < 20; i++)
        //    Debug.Log(i + " " + NextGaussian());
    }


    public void SetDimensions(int width, int height)
    {
        horizontalObliquenessPerPixel = 1f / (width / 2f);
        verticalObliqunessPerPixel = 1f / (height / 2f);
        if (debug)
            Debug.Log("horizontalObliquenessPerPixel: " + horizontalObliquenessPerPixel.ToString("R") + " verticalObliqunessPerPixel " + verticalObliqunessPerPixel.ToString("R"));
    }


    // Update is called once per frame
    Vector3 newEulers;
    void Update () {
        if (enablePosition)
        {
            Vector3 newPosition;
            newPosition.x = initialPosition.x + NextRandom() * positionAmplitude;
            newPosition.y = initialPosition.y + NextRandom() * positionAmplitude;
            newPosition.z = initialPosition.z + NextRandom() * positionAmplitude;
            transform.position = newPosition;
        }
        if (enableRotation)
        {
            currentRandomVectorIndex++;
            if (currentRandomVectorIndex >= numCachedVectors)
                currentRandomVectorIndex = 0;

            newEulers.x = initialRotation.eulerAngles.x + cachedRandomVectors[currentRandomVectorIndex].x * rotationAmplitude;
            newEulers.y = initialRotation.eulerAngles.y + cachedRandomVectors[currentRandomVectorIndex].y * rotationAmplitude;
            newEulers.z = 0f;// initialRotation.eulerAngles.z + NextRandom() * rotationAmplitude;
            //transform.rotation = Quaternion.Euler(newEulers);
            transform.localEulerAngles = newEulers;
        }
        if (enableFrustum)
        {
            /*
                One represents a flat side view frustum. So,
                it basically represents moving the view frustum
                to the side (or up) by width/2 (or height/2) pixels.

            */
            float horizontalObliqueness = 0f, verticalObliquness = 0f;

            switch (frustumMode)
            {
                case FrustumSamplingMode.Equirectangular:
                    //frustumPositionIndex++;
                    //frustumPositionIndex %= 4;
                    frustumPositionIndex = Random.Range(0, 4);
                    if (frustumPositionIndex == 0)
                    {
                        horizontalObliqueness = -horizontalObliquenessPerPixel * 0.25f;
                        verticalObliquness = -verticalObliqunessPerPixel * 0.25f;
                    }
                    else if (frustumPositionIndex == 1)
                    {
                        horizontalObliqueness = horizontalObliquenessPerPixel * 0.25f;
                        verticalObliquness = -verticalObliqunessPerPixel * 0.25f;
                    }
                    else if (frustumPositionIndex == 2)
                    {
                        horizontalObliqueness = horizontalObliquenessPerPixel * 0.25f;
                        verticalObliquness = verticalObliqunessPerPixel * 0.25f;
                    }
                    else
                    {
                        horizontalObliqueness = -horizontalObliquenessPerPixel * 0.25f;
                        verticalObliquness = verticalObliqunessPerPixel * 0.25f;
                    }
                    break;
                case FrustumSamplingMode.Random:
                    //horizontalObliqueness = (Random.value * 2f - 1f) * horizontalObliquenessPerPixel * 0.49f;
                    //verticalObliquness = (Random.value * 2f - 1f) * verticalObliqunessPerPixel * 0.49f;
                    horizontalObliqueness = ((float)rnd.NextDouble() * 2f - 1f) * horizontalObliquenessPerPixel * 0.5f;
                    verticalObliquness = ((float)rnd.NextDouble() * 2f - 1f) * verticalObliqunessPerPixel * 0.5f;
                    break;
                case FrustumSamplingMode.Gaussian:
                    horizontalObliqueness = NextGaussian() / 2f * horizontalObliquenessPerPixel * 0.5f;
                    verticalObliquness = NextGaussian() / 2f * verticalObliqunessPerPixel * 0.5f;
                    break;
                case FrustumSamplingMode.Radial:
                    SimpleRadialSampler(out horizontalObliqueness, out verticalObliquness);
                    break;
                default:
                    throw new System.Exception("Unexpected FrustumMode.");
            }

            SetObliqueness(horizontalObliqueness, verticalObliquness);
        }
    }
    static private System.Random rnd = new System.Random();

    private int radial_sample_index = 0;
    private int radial_sample_count = 5;
    void SimpleRadialSampler(out float horizontalObliqueness, out float verticalObliqueness)
    {
        radial_sample_index += radial_sample_count / 2;
        radial_sample_index %= radial_sample_count;

        float period_position = (float)(radial_sample_index + 1) / (float)radial_sample_count;
        if(debug)
            Debug.Log("period_position: " + period_position);

        horizontalObliqueness = Mathf.Cos(period_position * 360f * Mathf.Deg2Rad) * horizontalObliquenessPerPixel * 0.5f;
        verticalObliqueness = Mathf.Sin(period_position * 360f * Mathf.Deg2Rad) * verticalObliqunessPerPixel * 0.5f;
        if(debug)
            Debug.Log("h/v: " + horizontalObliqueness + ", " + verticalObliqueness);
    }


    void SetObliqueness(float horizObl, float vertObl)
    {
        Matrix4x4 mat = camera.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        camera.projectionMatrix = mat;
    }


    /*

     def NormalizedRandom(minValue, maxValue):
     mean = (minValue + maxValue) / 2
     sigma = (maxValue - mean) / 3
     return nextRandom(mean, sigma)
     */

    //static System.Random randomGenerator = new System.Random();
    //float Random(float min, float max)
    //{
    //    float mean = (min + max) / 2;
    //    float sigma = (max - min) / 3;
    //    return (float)randomGenerator.NextDouble
    //}

    public float NextRandom()
    {
        float retval;
        if (gaussian)
            retval = NextGaussian() / 2f;
        else
            retval = Random.value * 2f - 1f;
        if (debug)
            Debug.Log("random: " + retval);
        return retval;
    }


    public static float NextGaussian()
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
        return u * fac;
    }
}
