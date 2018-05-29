using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour 
{
    [SerializeField] private AudioClip mainThrusterClip;
    [SerializeField] private AudioClip airPumpClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip winClip;

    [SerializeField] private ParticleSystem mainThrusterParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem winParticles;

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float upThrust = 100f;

    enum State { Alive, Dead, Transcending }
    private State shipState;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

	// Use this for initialization
	void Start() 
	{
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        shipState = State.Alive;
	}
	
	// Update is called once per frame
	void Update() 
	{
        if (shipState == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    private void Thrust()
    {
        // Check if it's being thrusted upwards
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                audioSource.clip = mainThrusterClip;
                mainThrusterParticles.Play();
                rigidBody.AddRelativeForce(Vector3.right * mainThrust * Time.deltaTime);

                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(mainThrusterClip);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                audioSource.clip = airPumpClip;
                rigidBody.AddForce(Vector3.up * upThrust * Time.deltaTime);

                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(airPumpClip);
                }
            }
        }
        else
        {
            mainThrusterParticles.Stop();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (shipState != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OK");
                break;
            case "Finish":
                shipState = State.Transcending;

                audioSource.Stop();
                mainThrusterParticles.Stop();

                audioSource.PlayOneShot(winClip);
                winParticles.Play();

                Invoke("LoadNextScene", 2f);
                break;
            default:
                shipState = State.Dead;

                audioSource.Stop();
                mainThrusterParticles.Stop();

                audioSource.PlayOneShot(deathClip);
                deathParticles.Play();

                Invoke("LoadFirstLevel", 2f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
