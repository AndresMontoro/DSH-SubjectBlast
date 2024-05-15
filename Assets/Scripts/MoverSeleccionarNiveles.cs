using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSeleccionarNiveles : MonoBehaviour
{
    public Transform paredFinal;
    private float limiteFinal;

    public Transform paredInicio;
    private float limiteInicio;

    public float velocidadDesplazamiento = 0.8f;
    public float desplazamientoMaximo = 1.0f;
    private Vector2 posicionInicial;

    // Start is called before the first frame update
    void Start()
    {
        limiteFinal = paredFinal.position.z;
        limiteInicio = paredInicio.position.z;
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

                // Limitar el movimiento solo en el eje Z (hacia adelante o hacia atrás)
                float desplazamientoZ = desplazamiento.y * velocidadDesplazamiento * Time.deltaTime;
                desplazamientoZ = Mathf.Clamp(desplazamientoZ, -desplazamientoMaximo, desplazamientoMaximo);

                // Obtener la nueva posición de la cámara
                Vector3 nuevaPosicion = transform.position + Vector3.forward * desplazamientoZ;

                // Restringir la posición de la cámara dentro de los límites de las paredes invisibles
                nuevaPosicion.z = Mathf.Clamp(nuevaPosicion.z, limiteInicio, limiteFinal);

                // Mover la cámara solo si no excede los límites
                transform.position = nuevaPosicion;
            }
        }
    }
}
