using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Beat_Detection
{
    [RequireComponent(typeof(AudioSource))]
    public class BeatDetector : MonoBehaviour
    {
        //The last average energy readings. Size will be 44032 / samplesTaken as this is the closest you can get to 1 second of audio.
        private float[] localHistory;

        public int m_SamplesTaken; //Must be the same as the audio manager.

        //Determines what change in amplitude dictates a beat. Variance is used in this calculation.
        public float m_Sensitivity;
        public float m_EnergyVariance;

        //Values required for detecting if there is a beat or not.
        //Don't need to be public but are for testing.
        public float m_InstantEnergy;
        public float m_AverageLocalEnergy;

        public int m_Seconds = 1;

        //True if a beat is detected, false otherwise.
        public static bool m_Beat;

        void Start()
        {
            int size = (m_Seconds * 44100) / m_SamplesTaken;
            localHistory = new float[size]; //Dictated by 44032 / 1024 for 1 second of audio.

            m_Beat = false;
        }

        void Update()
        {
            m_InstantEnergy = CalculateInstantEnergy(); //Calculate the instant energy based on sample arrays.
            m_AverageLocalEnergy = CalculateAverageLocalEnergy(); //Calculate the local average energy based on 43 instant energy readings.

            m_EnergyVariance = CalculateEnergyVariance();
            m_Sensitivity = CalculateSensitivity();

            localHistory = ShiftHistory(); //Shift the history buffer up one to make room for new values.
            localHistory[0] = m_InstantEnergy; //Add the instant energy average to the history.

            m_Beat = IsBeat();
        }

        float CalculateInstantEnergy()
        {
            float instantEnergy = 0f;
            for (int i = 0; i < m_SamplesTaken; i++)
            {
                instantEnergy += (float)Math.Pow(AudioManager.m_SamplesLeft[i], 2) + (float)Math.Pow(AudioManager.m_SamplesRight[i], 2);
            }
            return instantEnergy;
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
            float S = (0.0025714f * m_EnergyVariance) + 1.5142857f;
            return S;
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
