using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private GamePiece piece;
    private IEnumerator moveCoroutine;

    void Awake() {
        piece = GetComponent<GamePiece>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int newX, int newZ, float time) 
    {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        
        moveCoroutine = MoveCoroutine(newX, newZ, time);
        StartCoroutine(moveCoroutine);
    }

    public IEnumerator MoveCoroutine(int newX, int newZ, float time) {
        piece.X = newX;
        piece.Z = newZ;

        Vector3 startPos = transform.position;
        Vector3 endPos = piece.GridRef.GetWorldPosition(newX, newZ);

        for (float t = 0; t <= 1 * time; t += Time.deltaTime) {
            piece.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        piece.transform.position = endPos;
    }
}
