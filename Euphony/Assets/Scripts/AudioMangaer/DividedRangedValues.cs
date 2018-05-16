using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedRangedValues : MonoBehaviour
{
    public static float[] m_rangedBounds = new float[8];
    public static float[] m_rangedBoundsBuffer = new float[8];
    private float[] m_highestFreqValues = new float[8];
    public float startingHighest;

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            m_highestFreqValues[i] = startingHighest;
        }
    }

    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            if (DividedAudioBands.m_freqBounds[i] > m_highestFreqValues[i])
            {
                m_highestFreqValues[i] = DividedAudioBands.m_freqBounds[i];
            }
            m_rangedBounds[i] = (DividedAudioBands.m_freqBounds[i] / m_highestFreqValues[i]);
            m_rangedBoundsBuffer[i] = (DividedAudioBands.m_freqBoundsBuffer[i] / m_highestFreqValues[i]);
        }
    }
}
