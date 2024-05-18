using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    int fila, col;

    public void setPos(int f, int c)
    {
        fila = f;
        col = c;
    }
    public int getFila() { return fila; }
    public int getCol() { return col; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
