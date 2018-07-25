using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amplitude : MonoBehaviour
{

    public static float amplitude;
    public static float ampBuffer;
    private float ampHigh;

    // Update is called once per frame
    void Update()
    {
        float avgAmp = 0f;
        float avgBuff = 0f;

        for (int i = 0; i < 8; i++)
        {
            avgAmp += RangedBandBuffer.m_rangedBounds[i];
            avgBuff += RangedBandBuffer.m_rangedBoundsBuffer[i];
        }

        if (avgAmp > ampHigh)
            ampHigh = avgAmp;

        amplitude = avgAmp / ampHigh;
        ampBuffer = avgBuff / ampHigh;
    }
}
