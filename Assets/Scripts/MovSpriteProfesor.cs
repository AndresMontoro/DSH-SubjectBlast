using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MovSpriteProfesor : MonoBehaviour
{
    [Header("GameObjects / Assets")]
    public GameObject imagenProfesor;
    public GameObject dialogueBox;
    private AudioSource audioSource;
    public GameObject posFinal;
    public GameObject posInicial;
    public AudioClip clip;


    [Header("Variables")]
    public float anim_duration = 1.0f;
    private float elapsedTime = 0.0f;
    public bool isMoving = false;
    public bool isTalking = false;

    


    void Start()
    {
        elapsedTime = 0.0f;
        isMoving = true;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isMoving) 
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / anim_duration);
            float easedT = t * (2 - t);

            Vector3 pointA = posInicial.transform.position;
            Vector3 pointB = posFinal.transform.position;

            // Lerp between pointA and pointB
            imagenProfesor.transform.position = Vector3.Lerp(pointA, pointB, easedT);

            // Check if the movement is complete
            if (t >= 1.0f)
            {
                isMoving = false;
                elapsedTime = 0.0f;
                isTalking = true;
            }

            if (isTalking) 
            {
                StartCoroutine(StartDialogue());
            }
        }
        else 
        {
            if (!isTalking)
            {
                elapsedTime += Time.deltaTime;

                float t2 = Mathf.Clamp01(elapsedTime / anim_duration);
                float easedT2 = t2 * (2 - t2);

                Vector3 pointA = posInicial.transform.position;
                Vector3 pointB = posFinal.transform.position;

                // Lerp between pointA and pointB
                imagenProfesor.transform.position = Vector3.Lerp(pointB, pointA, easedT2);
            }
        }
    }

    IEnumerator StartDialogue()
    {
        Debug.Log("Profesor empieza a hablar.");
        audioSource.clip = clip;
        audioSource.Play();
        dialogueBox.SetActive(true);
        yield return new WaitForSeconds(clip.length);
        dialogueBox.SetActive(false);
        isTalking = false;
    }
}
