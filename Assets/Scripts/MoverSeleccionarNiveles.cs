using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSeleccionarNiveles : MonoBehaviour
{
    public float velocidadDesplazamiento = 0.5f;
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
            
            // Limitar el movimiento solo en el eje Z (hacia delante o hacia atrás)
            float desplazamientoZ = desplazamiento.y * velocidadDesplazamiento * Time.deltaTime;
            desplazamientoZ = Mathf.Clamp(desplazamientoZ, -desplazamientoMaximo, desplazamientoMaximo);

            // Obtener la dirección del movimiento en el espacio de la cámara
            Vector3 direccionMovimiento = Camera.main.transform.forward * desplazamientoZ;
            direccionMovimiento.y = 0; // Asegurar que el movimiento sea solo horizontal

            // Mover la cámara
            transform.position += direccionMovimiento;
        }
    }
}





}
