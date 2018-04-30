using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCubes : MonoBehaviour
{
    public GameObject m_cubePrefab;

    GameObject[] m_sampleCubes = new GameObject[512];
    public enum channels { Stereo, Right, Left };
    public channels channel = new channels();

    public float m_maxScale;
    public float clampVal;
    float newScale;

    public int circleSize;

    public bool m_bufferActive;
    public bool clamp;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < m_sampleCubes.Length; i++)
        {
            //Instantiate cube and add it to the sampleCube Array, each cubes parent will be this gameobject. 
            m_sampleCubes[i] = Instantiate(m_cubePrefab, new Vector3(transform.position.x + i, transform.position.y, transform.position.z), transform.rotation, transform);
        }
        #region OLDSPAWNING
        /*Done for every cube...
        for (int i = 0; i < m_sampleCubes.Length; i++)
        {
            //Creates a cube in the game based off of the prefab passed in to it.
            GameObject cubeInstance = Instantiate(m_cubePrefab); //Instantiate makes a clone of a given object.

            //Makes the cube position the same as the invisible init object in the game.
            cubeInstance.transform.position = this.transform.position; 

            //Makes the cube a child of this object which is initialising the cubes.
            cubeInstance.transform.parent = this.transform;
            cubeInstance.name = "SampleCube_" + i;

            //Work  out circle spacing by doing 360 / sample number so in this instance:
            //360 / 512 = 0.703125...

            //DRAW AS A CIRCLE
            //this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);

            //DRAW AS A LINE! 
            this.transform.position = this.transform.position + new Vector3(1, 0, 0);

            //Moves each cube forward the given amount (depends on cube size).
            cubeInstance.transform.position = Vector3.forward * circleSize;

            //Adds instance of the created cube to the list.
            m_sampleCubes[i] = cubeInstance;
        } */
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //For every cube...
        for (int i = 0; i < m_sampleCubes.Length; i++)
        {
            //If the cube isn't null it has been setup correctly...
            if (m_sampleCubes != null)
            {
                //Scales the object every frame based on the sample data from the audio peer.
                //The +2 is the starting scale for each cube.
                if (m_bufferActive)
                {
                    newScale = (AudioManager.m_SampleBuffer[i] * m_maxScale) + 2;
                }
                else
                {
                    if (channel == channels.Left)
                    {
                        newScale = (AudioManager.m_SamplesLeft[i] * m_maxScale) + 2;
                    }
                    else if (channel == channels.Right)
                    {
                        newScale = (AudioManager.m_SamplesRight[i] * m_maxScale) + 2;
                    }
                    else if (channel == channels.Stereo)
                    {
                        newScale = (AudioManager.m_SamplesLeft[i] + AudioManager.m_SamplesRight[i] * m_maxScale) + 2;
                    }
                }

                if (clamp)
                {
                    newScale = Mathf.Clamp(newScale, 0, clampVal);
                }

                m_sampleCubes[i].transform.localScale = new Vector3(1, newScale, 1);
            }
        }
    }
}