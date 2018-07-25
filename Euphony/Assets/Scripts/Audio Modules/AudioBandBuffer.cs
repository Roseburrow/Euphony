using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBandBuffer : MonoBehaviour {

    public static float[] m_freqBoundsBuffer = new float[8];
    float[] m_freqBoundsBufferDecrease = new float[8];

    // Use this for initialization
    void Start ()
    {
	}

    // Update is called once per frame
    void Update ()
    {
        CreateBoundaryBuffer();
	}

    void CreateBoundaryBuffer()
    {
        /* The purpose of this is to created different raise and lower amounts 
         * for each bar individually so they can all adjust as they need to. 
         * It should be noted that the bars get drawn using the new buffers
         * which is why these values are modified. */
        for (int i = 0; i < 8; i++)
        {
            if (AudioBands.m_freqBounds[i] > m_freqBoundsBuffer[i])
            {
                /*If the current frequency of the bar is higher that the buffer
                 *then that is ok as the bar needs to go to the correct height,
                 * so we make the buffer equal the current frequency value. */
                m_freqBoundsBuffer[i] = AudioBands.m_freqBounds[i];

                /*We then have to set a suitable decrease amount for when the
                 * bar falls back below the highest frequency as the bar will
                 * have to be lowered down. */
                m_freqBoundsBufferDecrease[i] = 0.010f;
            }

            if (AudioBands.m_freqBounds[i] < m_freqBoundsBuffer[i])
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
