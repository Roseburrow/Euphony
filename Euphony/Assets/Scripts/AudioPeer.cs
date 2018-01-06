using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioPeer : MonoBehaviour {

    AudioSource m_audioSource;

    public static float[] m_sampleArray = new float[512]; //512 samples every frame.
    public static float[] m_sampleBuffer = new float[512];
    float[] m_sampleDecrease = new float[512];

    public static float[] m_freqBoundaries = new float[8];
    public static float[] m_boundaryBuffer = new float[8];

    /*Every time the freqB is higher than the boundB:
     *boundB = freqB.
     * 
     * If freqB is lower than boundB then it is decreased a set amount. */
    float[] m_bufferDecrease = new float[8];

	// Use this for initialization
	void Start ()
    {
        //Takes the audio source from the unity project and sets it as the source to use.
        m_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Populates sample array every frame.
        GetSpectrumSource();

        CreateFrequencyBoundaries(); //Splites the 512 samples into 8 sections for each bar.
        CreateBoundaryBuffer(); //Manages the buffers used to steady the bars during playback.
        CreateSampleBuffer();
	}

    void GetSpectrumSource()
    {
        //Reads samples from the given source in real time into the array only 512 big.
        //This is essentially our audio stream...
        m_audioSource.GetSpectrumData(m_sampleArray, 0, FFTWindow.Rectangular);
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

            for (int j =0; j < sampleCounter; j++)
            {
                avg += m_sampleArray[counter] * (counter + 1);
                counter++;
            }

            avg /= counter;
            m_freqBoundaries[i] = avg * 10;
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
            if (m_freqBoundaries[i] > m_boundaryBuffer[i])
            {
                /*If the current frequency of the bar is higher that the buffer
                 *then that is ok as the bar needs to go to the correct height,
                 * so we make the buffer equal the current frequency value. */
                m_boundaryBuffer[i] = m_freqBoundaries[i];

                /*We then have to set a suitable decrease amount for when the
                 * bar falls back below the highest frequency as the bar will
                 * have to be lowered down. */
                m_bufferDecrease[i] = 0.010f;
            }

            if (m_freqBoundaries[i] < m_boundaryBuffer[i])
            {
                /*If the buffer is higher than the current frequency value then
                 *the new frequency reading is lower than the previous meaning
                 *the bar has to be lowered. In this case we lower the buffer by
                 *the decrease amount. We then change the buffer decrease by a
                 * multiplier so it will lower faster and faster. */
                m_boundaryBuffer[i] -= m_bufferDecrease [i];
                m_bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void CreateSampleBuffer()
    {
        for (int i = 0; i < 512; i++)
        {
            if (m_sampleArray[i] > m_sampleBuffer[i])
            {
                m_sampleBuffer[i] = m_sampleArray[i];
                m_sampleDecrease[i] = 0.0001f;
            }

            if (m_sampleArray[i] < m_sampleBuffer[i])
            {
                m_sampleBuffer[i] -= m_sampleDecrease[i];
                m_sampleDecrease[i] *= 1.3f;
            }
        }
    }
}
