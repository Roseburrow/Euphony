using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{

    public static AudioSource m_AudioSource; //Unity class for storing audio files.
    public int m_SamplesTaken = 512;

    //Sample arrays for data, buffer and buffer decrease. Is 512 but can be 1024 or 2048.
    public static float[] m_SamplesLeft;
    public static float[] m_SamplesRight;
    public static float[] m_SampleBuffer;
    float[] m_SampleBufferDecrease;

    //Stores average of samples in 8 frequency boundaries.
    public static float[] m_freqBounds = new float[8];
    public static float[] m_freqBoundsBuffer = new float[8];
    float[] m_freqBoundsBufferDecrease = new float[8];
    /*Every time the freqB is higher than the boundB:
     *boundB = freqB.
     * 
     * If freqB is lower than boundB then it is decreased a set amount. */

    //Arrays for ranged data.
    float[] m_highestFreqValues = new float[8];
    public float startingHighest;
    public static float[] m_rangedBounds = new float[8];
    public static float[] m_rangedBoundsBuffer = new float[8];

    public enum channels { Stereo, Right, Left };
    public channels channel = new channels();

    // Use this for initialization
    void Start()
    {
        //Takes the audio source from the unity project and sets it as the source to use.
        m_AudioSource = GetComponent<AudioSource>();
        m_SamplesLeft = new float[m_SamplesTaken];
        m_SamplesRight = new float[m_SamplesTaken];
        m_SampleBuffer = new float[m_SamplesTaken];
        m_SampleBufferDecrease = new float[m_SamplesTaken];
        SetStartingHighest();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumSource(); //Populates sample array every frame.

        CreateFrequencyBoundaries(); //Splits samples into 8 catagories.
        CreateBoundaryBuffer(); //Creates buffer for divided boudnaries.
        CreateSampleBuffer(); //Create buffer for sample data. 
        CreateRangedBounds(); //Creates ranged buffer and values.

    }

    void GetSpectrumSource()
    {
        //Reads samples from the given source in real time into the array only 512 big.
        //This is essentially our audio stream...
        m_AudioSource.GetSpectrumData(m_SamplesLeft, 0, FFTWindow.BlackmanHarris);
        m_AudioSource.GetSpectrumData(m_SamplesRight, 1, FFTWindow.BlackmanHarris);
    }

    void SetStartingHighest()
    {
        for (int i = 0; i < 8; i++)
        {
            m_highestFreqValues[i] += startingHighest;
        }
    }

    void CreateRangedBounds()
    {
        for (int i = 0; i < 8; i++)
        {
            if (m_freqBounds[i] > m_highestFreqValues[i])
            {
                m_highestFreqValues[i] = m_freqBounds[i];
            }
            m_rangedBounds[i] = (m_freqBounds[i] / m_highestFreqValues[i]);
            m_rangedBoundsBuffer[i] = (m_freqBoundsBuffer[i] / m_highestFreqValues[i]);
        }
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
                    avg += (m_SamplesLeft[counter] + m_SamplesRight[counter]) * (counter + 1);
                }
                if (channel == channels.Right)
                {
                    avg += m_SamplesRight[counter] * (counter + 1);
                }
                if (channel == channels.Left)
                {
                    avg += m_SamplesLeft[counter] * (counter + 1);
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

    void CreateSampleBuffer()
    {
        for (int i = 0; i < m_SamplesLeft.Length; i++)
        {
            if (m_SamplesLeft[i] > m_SampleBuffer[i])
            {
                m_SampleBuffer[i] = m_SamplesLeft[i];
                m_SampleBufferDecrease[i] = 0.0001f;
            }

            if (m_SamplesLeft[i] < m_SampleBuffer[i])
            {
                m_SampleBuffer[i] -= m_SampleBufferDecrease[i];
                m_SampleBufferDecrease[i] *= 1.3f;
            }
        }
    }
}