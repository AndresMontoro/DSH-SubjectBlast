using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearTablero : MonoBehaviour
{

    public GameObject tableroPrefab;
    public Vector3 posicionCentralTablero;
    GameObject _tablero;

    void Awake()
    {
        //vacio
    }

    void Start ()
    {
        _tablero = Instantiate(tableroPrefab);
        _tablero.transform.position = posicionCentralTablero;

        // Ejecutar esta linea provoca la creación del tablero en pendiente
        /*_tablero.transform.LookAt(Camera.main.transform.position);*/
    }


}
