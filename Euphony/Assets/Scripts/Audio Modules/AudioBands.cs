using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBands : MonoBehaviour {

    //Stores average of samples in 8 frequency boundaries.
    public static float[] m_freqBounds;

    public enum channels { Stereo, Right, Left };
    public channels channel = new channels();

    // Use this for initialization
    void Start ()
    {
        m_freqBounds = new float[8];
    }
	
	// Update is called once per frame
	void Update ()
    {
        CreateFrequencyBoundaries();
	}

    void CreateFrequencyBoundaries()
    {
        /* See notebook for frequency boundaries,
         It's all about splitting the amount of samples you have between the boundaries. */

        int counter = 0;
        float avg = 0;

        //For each new bar we are creating...
        for (int i = 0; i < 8; i++)
        {
            int sampleCounter = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
                sampleCounter += 2;

            for (int j = 0; j < sampleCounter; j++)
            {
                if (channel == channels.Stereo)
                {
                    avg += (Sampler.m_SamplesLeft[counter] + Sampler.m_SamplesRight[counter]) * (counter + 1);
                }
                if (channel == channels.Right)
                {
                    avg += Sampler.m_SamplesRight[counter] * (counter + 1);
                }
                if (channel == channels.Left)
                {
                    avg += Sampler.m_SamplesLeft[counter] * (counter + 1);
                }
                counter++;
            }
            avg /= counter;
            m_freqBounds[i] = avg * 10;
        }
    }
}
