using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSeleccionarNiveles : MonoBehaviour
{
    public float velocidadDesplazamiento = 0.0001f;
    public float desplazamientoMaximo = 1.0f;
    private Vector2 posicionInicial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                posicionInicial = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 desplazamiento = touch.position - posicionInicial;
                // float desplazamientoZ = desplazamiento.y * velocidadDesplazamiento;
                float desplazamientoZ = Mathf.Clamp(-desplazamiento.y * velocidadDesplazamiento, -desplazamientoMaximo, desplazamientoMaximo);
                transform.Translate(0, desplazamientoZ, desplazamientoZ);
            }
        }
    }
}
