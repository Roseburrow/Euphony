using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBandBeatDetection : MonoBehaviour
{

    //The audisource playing
    AudioSource m_AudioSource;

    public FFTWindow m_FFTWindow;
    public static AudioBand[] Bands;

    public enum channels { Stereo, Right, Left };
    public channels channel = new channels();

    private Sampler s;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        s = GetComponent<Sampler>();

        Bands = new AudioBand[8];

        for (int j = 0; j < Bands.Length; j++)
        {
            Bands[j] = new AudioBand(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetBandsInstantEnergy();

        foreach (AudioBand ab in Bands)
        {
            ab.Update();
        }
    }

    void GetBandsInstantEnergy()
    {
        float avg = 0f;
        int counter = 0;

        for (int i = 0; i < 8; i++)
        {
            int sampleCounter = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
                sampleCounter += 2;

            for (int j = 0; j < sampleCounter; j++)
            {
                avg += (float)Math.Pow(Sampler.m_SamplesLeft[counter], 2) + (float)Math.Pow(Sampler.m_SamplesRight[counter], 2);

                if (j == sampleCounter)
                {
                    Bands[i].m_InstantEnergy = avg;
                    avg = 0f;
                }
            }
        }
    }

    public class AudioBand
    {
        //The last average energy readings. Size will be 44032 / samplesTaken as this is the closest you can get to 1 second of audio.
        private float[] localHistory;

        //Determines what change in amplitude dictates a beat. Variance is used in this calculation.
        public float m_Sensitivity;
        public float m_EnergyVariance;

        //Values required for detecting if there is a beat or not.
        //Don't need to be public but are for testing.
        public float m_InstantEnergy;
        private float m_AverageLocalEnergy;

        public static bool m_Beat;

        public AudioBand(Sampler s)
        {
            int size = 44100 / s.m_SamplesTaken;
            localHistory = new float[size];
        }

        public void Update()
        {
            m_AverageLocalEnergy = CalculateAverageLocalEnergy(); //Calculate the local average energy based on 43 instant energy readings.

            m_EnergyVariance = CalculateEnergyVariance();
            m_Sensitivity = CalculateSensitivity();

            localHistory = ShiftHistory(); //Shift the history buffer up one to make room for new values.
            localHistory[0] = m_InstantEnergy; //Add the instant energy average to the history.

            m_Beat = IsBeat();
        }

        float CalculateAverageLocalEnergy()
        {
            float averageLocalEnergy = 0f;
            for (int i = 0; i < localHistory.Length; i++)
            {
                averageLocalEnergy += localHistory[i];
            }
            return averageLocalEnergy / localHistory.Length;
        }

        float CalculateEnergyVariance()
        {
            float variance = 0f;
            for (int i = 0; i < localHistory.Length; i++)
            {
                variance += (float)Math.Pow(localHistory[i] - m_AverageLocalEnergy, 2);
            }
            return variance / localHistory.Length;
        }

        float CalculateSensitivity()
        {
            return (0.0025714f * m_EnergyVariance) + 1.5142857f;
            //return (-0.0000015f * m_EnergyVariance) + 1.5142857f;
        }

        /// <summary>
        /// Shifts the history data to the right by one place to make room for new values.
        /// </summary>
        /// <returns>'result' which is the new array with the values shifted.</returns>
        float[] ShiftHistory()
        {
            float[] result = new float[localHistory.Length];

            for (int i = 1; i < localHistory.Length - 1; i++)
            {
                result[i] = localHistory[i - 1];
            }
            return result;
        }

        /// <summary>
        /// Not currently used but could be useful in the future.
        /// </summary>
        /// <returns>true if a beat is detected, false otherwise</returns>
        public bool IsBeat()
        {
            if (m_InstantEnergy > m_AverageLocalEnergy * m_Sensitivity)
            {
                return true;
            }
            else { return false; }
        }
    }
}
