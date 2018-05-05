using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBufferer : MonoBehaviour
{
    private Sampler sampler;
    public static float[] m_SampleBuffer;
    private float[] m_SampleBufferDecrease;

    // Use this for initialization
    void Start ()
    {
        sampler = GetComponent<Sampler>();
        m_SampleBuffer = new float[sampler.m_SamplesTaken];
        m_SampleBufferDecrease = new float[sampler.m_SamplesTaken];
    }
	
	// Update is called once per frame
	void Update ()
    {
		for (int i = 0; i < Sampler.m_SamplesLeft.Length; i++)
        {
            if (Sampler.m_SamplesLeft[i] > m_SampleBuffer[i])
            {
                m_SampleBuffer[i] = Sampler.m_SamplesLeft[i];
                m_SampleBufferDecrease[i] = 0.0001f;
            }

            if (Sampler.m_SamplesLeft[i] < m_SampleBuffer[i])
            {
                m_SampleBuffer[i] -= m_SampleBufferDecrease[i];
                m_SampleBufferDecrease[i] *= 1.3f;
            }
        }
	}
}
