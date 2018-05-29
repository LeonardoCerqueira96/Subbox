using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour 
{
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float upThrust = 100f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

	// Use this for initialization
	void Start() 
	{
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update() 
	{
        Thrust();
        Rotate();
	}

    private void Thrust()
    {
        // Check if it's being thrusted upwards
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.right * mainThrust);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.AddForce(Vector3.up * upThrust);
            }
                
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void Rotate()
    {
        // Take manual control of rotation
        rigidBody.freezeRotation = true;
        
        // Check if it's being rotated
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rcsThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rcsThrust);
        }

        // Resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

}
