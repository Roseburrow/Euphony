using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhllotaxisReactive : MonoBehaviour
{
    public AudioManager audioManager;
    private Material trailMat;
    public Color colour;

    //Phyllotacis variables
    private TrailRenderer trailRenderer;
    private Vector2 pos;
    public float angle;
    public float c;         //constant scaler
    private int n;          //quad index, 'sunflower seed'

    //Lerp Values
    public int startPoint;
    public int endPoint;
    public int stepSize;
    public int movevementAudioBand;

    //Lerping variables
    public bool lerpValues;
    public bool repeat;
    public bool invert;
    private bool fwrd;

    private bool isLerp;
    public AnimationCurve frequencyCurve;
    public Vector2 min_maxSpeed;

    private float lerpTimer;
    private float lerpSpeed;
    private int currentPoint; //The index of the seed being lerped to.
    private Vector3 startPos, endPos;

    //Scaling with audio
    public bool audioScaling;
    public bool audioScalingCurve;
    public Vector2 min_maxScale;
    public AnimationCurve scaleCurve;
    public float curveScaleSpeed;
    public int scaleAudioBand;
    private float scaleTimer, currentScale;

    // Use this for initialization
    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailMat = new Material(trailRenderer.material);
        trailMat.SetColor("_TintColor", colour);
        trailRenderer.material = trailMat;

        currentScale = c;
        n = startPoint;
        fwrd = true;
        transform.localPosition = PhyllotaxisCalculator(angle, currentScale, n);

        if (lerpValues)
        {
            isLerp = true;
            InitialiseLerpValues();
        }
    }

    private void InitialiseLerpValues()
    {
        pos = PhyllotaxisCalculator(angle, currentScale, n);
        startPos = transform.localPosition;
        endPos = new Vector3(pos.x, pos.y, 0);
    }

    private void Update()
    {
        if (audioScaling)
        {
            if (audioScalingCurve)
            {
                scaleTimer += (curveScaleSpeed * AudioManager.m_rangedBounds[scaleAudioBand]) * Time.deltaTime;
                if (scaleTimer >= 1)
                {
                    scaleTimer -= 1;
                }
                currentScale = Mathf.Lerp(min_maxScale.x, min_maxScale.y, scaleCurve.Evaluate(scaleTimer));
            }
            else
            {
                currentScale = Mathf.Lerp(min_maxScale.x, min_maxScale.y, AudioManager.m_rangedBounds[scaleAudioBand]);
            }
        }

        if (lerpValues)
        {
            if (isLerp)
            {
                lerpSpeed = Mathf.Lerp(min_maxSpeed.x, min_maxSpeed.y, frequencyCurve.Evaluate(AudioManager.m_rangedBounds[movevementAudioBand]));
                lerpTimer += Time.deltaTime * lerpSpeed;
                transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(lerpTimer));

                if (lerpTimer >= 1)
                {
                    lerpTimer -= 1;

                    if (fwrd)
                    {
                        n += stepSize;
                        currentPoint++;
                    }
                    else
                    {
                        n -= stepSize;
                        currentPoint--;
                    }

                    if (currentPoint < endPoint && currentPoint > 0)
                    {
                        InitialiseLerpValues();
                    }
                    else
                    {
                        if (repeat)
                        {
                            if (invert)
                            {
                                fwrd = !fwrd;
                                InitialiseLerpValues();
                            }
                            else
                            {
                                n = startPoint;
                                currentPoint = 0;
                                InitialiseLerpValues();
                            }
                        }
                        else
                        {
                            isLerp = false;
                        }
                    }
                }
            }
        }
        else
        {
            pos = PhyllotaxisCalculator(angle, currentScale, n);
            transform.localPosition = new Vector3(pos.x, pos.y, 0);
            n += stepSize;
            currentPoint++;
        }
    }

    private Vector2 FermatCalculator()
    {
        float phi = startPoint + (n * Time.deltaTime);
        float radius = Mathf.Sqrt(phi);

        //Polar to Cartesean co-ords conversion
        float x = radius * Mathf.Cos(phi);
        float y = radius * Mathf.Sin(phi);
        return new Vector2(x, y);
    }

    private Vector2 PhyllotaxisCalculator(float angle, float scalar, int seedNo)
    {
        float phi = seedNo * (angle * Mathf.Deg2Rad);
        float radius = scalar * Mathf.Sqrt(seedNo);

        //Polar to Cartesean co-ords conversion
        float x = radius * Mathf.Cos(phi);
        float y = radius * Mathf.Sin(phi);
        return new Vector2(x, y);
    }
}
