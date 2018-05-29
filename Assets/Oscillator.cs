using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour 
{

    [SerializeField] private Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] private float period = 2f;

    private float movementFactor;
    private Vector3 startingPos;

    private const float tau = Mathf.PI * 2f;

    // Use this for initialization
    void Start() 
	{
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update() 
	{
        float cycles = Time.time / period;

        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
