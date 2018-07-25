using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBandBuffer : MonoBehaviour {

    public static float[] m_rangedBounds = new float[8];
    public static float[] m_rangedBoundsBuffer = new float[8];
    float[] m_highestFreqValues = new float[8];
    public float startingHighest;

    // Use this for initialization
    void Start ()
    {
        SetStartingHighest();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CreateRangedBounds();
	}

    void CreateRangedBounds()
    {
        for (int i = 0; i < 8; i++)
        {
            if (AudioBands.m_freqBounds[i] > m_highestFreqValues[i])
            {
                m_highestFreqValues[i] = AudioBands.m_freqBounds[i];
            }
            m_rangedBounds[i] = (AudioBands.m_freqBounds[i] / m_highestFreqValues[i]);
            m_rangedBoundsBuffer[i] = (AudioBandBuffer.m_freqBoundsBuffer[i] / m_highestFreqValues[i]);
        }
    }

    void SetStartingHighest()
    {
        for (int i = 0; i < 8; i++)
        {
            m_highestFreqValues[i] += startingHighest;
        }
    }
}
