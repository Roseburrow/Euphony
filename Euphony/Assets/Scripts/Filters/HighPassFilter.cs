using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighPassFilter : MonoBehaviour
{
    private AudioSource source;
    //public InputField cutoffInput;
    //public InputField resonanceInput;

    private int sampleRate;

    [Range(0.1f, 1.4f)]
    public float resonance;

    [Range(10, 20000)]
    public int cutoffFrequency;

    private float c, a1, a2, a3, b1, b2;
    private float output;
    private string input;

    private float[] iHistory;
    private float[] oHistory;

    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
        source = GetComponent<AudioSource>();

        iHistory = new float[2];
        oHistory = new float[2];

        cutoffFrequency = 100;
        resonance = 1f;
    }

    void GetCutoffText(string text)
    {
        input = text;
        ChangeCutoff(int.Parse(text));
    }

    void GetResonanceText(string text)
    {
        input = text;
        ChangeResonance(float.Parse(text));
    }

    public void ChangeCutoff(int newfrequency)
    {
        cutoffFrequency = newfrequency;
    }

    public void ChangeResonance(float newResonance)
    {
        resonance = newResonance;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        c = Mathf.Tan(Mathf.PI * cutoffFrequency / sampleRate);
        a1 = 1.0f / (1.0f + resonance * c + c * c);
        a2 = -2.0f * a1;
        a3 = a1;
        b1 = 2.0f * (c * c - 1.0f) * a1;
        b2 = (1.0f - resonance * c + c * c) * a1;

        for (int i = 0; i < data.Length; i++)
        {
            output = a1 * data[i] + a2 * iHistory[0] + a3 * iHistory[1] - b1 * oHistory[0] - b2 * oHistory[1];

            iHistory[1] = iHistory[0];
            iHistory[0] = data[i];
            data[i] = output;

            oHistory[1] = oHistory[0];
            oHistory[0] = output;
        }
    }
}
