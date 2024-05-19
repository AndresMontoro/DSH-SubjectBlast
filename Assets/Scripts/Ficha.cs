using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    int fila, col;
    public Camera mainCamera;
    public ManejadorTablero manejador;

    public void setPos(int f, int c)
    {
        fila = f;
        col = c;
    }
    public int getFila() { return fila; }
    public int getCol() { return col; }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detectar si hay toques en la pantalla
        if (Input.touchCount > 0)
        {
            // Obtener el primer toque
            Touch touch = Input.GetTouch(0);

            // Comprobar si el toque ha comenzado (puedes cambiar esto a TouchPhase.Ended si prefieres detectar cuando se suelta el toque)
            if (touch.phase == TouchPhase.Began)
            {
                // Convertir la posición del toque en una posición de ray en el mundo
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Verificar si el raycast ha golpeado un collider
                if (Physics.Raycast(ray, out hit))
                {
                    // Comprobar si el GameObject golpeado es este GameObject
                    if (hit.transform == transform)
                    {
                        manejador.Movimiento(fila, col);
                        Debug.Log("Me han tocado");
                    }
                }
            }
        }
    }
}
