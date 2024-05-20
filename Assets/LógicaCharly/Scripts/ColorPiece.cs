using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectPiece : MonoBehaviour
{
    public enum SubjectType
    {
        BASEDATOS,
        DISEÑO,
        CALCULO,
        PROGRAMACION,
        SOFTWARE,
        HARDWARE,
        COUNT
    }

    [System.Serializable]
    public struct SubjectGameObject
    {
        public SubjectType subject;
        public GameObject gameObject;
    }

    public SubjectGameObject[] subjectGameObjects;

    private SubjectType subject;

    public SubjectType Subject
    {
        get { return subject; }
        set { SetSubject(value); }
    }

    public int NumSubjects
    {
        get { return subjectGameObjects.Length; }
    }

    private Dictionary<SubjectType, GameObject> subjectGameObjectDict;

    void Awake()
    {
        subjectGameObjectDict = new Dictionary<SubjectType, GameObject>();

        for (int i = 0; i < subjectGameObjects.Length; i++)
        {
            if (!subjectGameObjectDict.ContainsKey(subjectGameObjects[i].subject))
            {
                subjectGameObjectDict.Add(subjectGameObjects[i].subject, subjectGameObjects[i].gameObject);
            }
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSubject(SubjectType newSubject)
    {
        subject = newSubject;

        // Find and destroy the existing child
        if (transform.childCount > 0)
        {
            Transform currentChild = transform.GetChild(0);
            Destroy(currentChild.gameObject);
        }

        // Instantiate the new child from the dictionary
        if (subjectGameObjectDict.ContainsKey(newSubject))
        {
            GameObject newChild = Instantiate(subjectGameObjectDict[newSubject], transform);
            newChild.transform.localPosition = Vector3.zero;
            newChild.transform.localRotation = Quaternion.identity;
        }
    }
}
