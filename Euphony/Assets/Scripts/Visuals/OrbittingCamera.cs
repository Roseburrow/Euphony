using UnityEngine;

public class OrbittingCamera : MonoBehaviour {

    public Transform origin;

    public float xOffset;
    public float zOffset;
    public float yOffset;

    public float orbitSpeed;
    public bool clockwise;
    public bool toggleMovement;

    float timer;

    // Use this for initialization
    void Start () 
	{
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
        timer += Time.deltaTime * orbitSpeed;

        if(toggleMovement)
            Orbit();

        transform.LookAt(origin);
	}

    void Orbit()
    {
        if (clockwise)
        {
            float x = -Mathf.Cos(timer) * xOffset;
            float z = Mathf.Sin(timer) * zOffset;
            transform.position = new Vector3(x, yOffset, z) + origin.position;
        }
        else
        {
            float x = Mathf.Cos(timer) * xOffset;
            float z = Mathf.Sin(timer) * zOffset;
            transform.position = new Vector3(x, yOffset, z) + origin.position;
        }
    }
}
