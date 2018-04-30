using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCubeCamera : MonoBehaviour {

    public Transform m_Target;

	// Use this for initialization
	void Start ()
    {
        transform.position = new Vector3(m_Target.position.x + 90, m_Target.position.y - 96, m_Target.position.z - 350);
	}
	
	// Update is called once per frame
	void Update ()
    {
    }
}
