using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedAudioBands : MonoBehaviour
{
    private Sampler sampler;

    public static float[] m_freqBounds = new float[8];
    public static float[] m_freqBoundsBuffer = new float[8];
    private float[] m_freqBoundsBufferDecrease = new float[8];

    public enum channels { Stereo, Right, Left };
    public channels channel = new channels();

    private void Start()
    {
        sampler = GetComponent<Sampler>();
        sampler.m_SamplesTaken = 512;
    }
    void Update ()
    {
        CreateFrequencyBoundaries();
        CreateBoundaryBuffer();
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

    void CreateBoundaryBuffer()
    {
        /* The purpose of this is to created different raise and lower amounts 
         * for each bar individually so they can all adjust as they need to. 
         * It should be noted that the bars get drawn using the new buffers
         * which is why these values are modified. */
        for (int i = 0; i < 8; i++)
        {
            if (m_freqBounds[i] > m_freqBoundsBuffer[i])
            {
                /*If the current frequency of the bar is higher that the buffer
                 *then that is ok as the bar needs to go to the correct height,
                 * so we make the buffer equal the current frequency value. */
                m_freqBoundsBuffer[i] = m_freqBounds[i];

                /*We then have to set a suitable decrease amount for when the
                 * bar falls back below the highest frequency as the bar will
                 * have to be lowered down. */
                m_freqBoundsBufferDecrease[i] = 0.010f;
            }

            if (m_freqBounds[i] < m_freqBoundsBuffer[i])
            {
                /*If the buffer is higher than the current frequency value then
                 *the new frequency reading is lower than the previous meaning
                 *the bar has to be lowered. In this case we lower the buffer by
                 *the decrease amount. We then change the buffer decrease by a
                 * multiplier so it will lower faster and faster. */
                m_freqBoundsBuffer[i] -= m_freqBoundsBufferDecrease[i];
                m_freqBoundsBufferDecrease[i] *= 1.2f;
            }
        }
    }
}
