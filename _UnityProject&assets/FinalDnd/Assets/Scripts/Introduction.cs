using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour {

    AudioSource audioSource;

    AudioClip controls;
    AudioClip introduction;
    
    // Use this for initialization
	void Start () {
        audioSource = this.gameObject.GetComponent<AudioSource>();

        controls = Resources.Load<AudioClip>("controls");
        introduction = Resources.Load<AudioClip>("introduction");
    }
	
	// Update is called once per frame
	void LateUpdate () {
		if (!audioSource.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                audioSource.clip = introduction;
                audioSource.Play();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                audioSource.clip = controls;
                audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //load the game
            SceneManager.LoadScene("House");
        }
    }

    
}
