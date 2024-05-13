using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionarNiveles : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    if (hit.transform == transform)
                    {
                        LoadScene();
                    }
                }
            }
        }
    }

    void LoadScene()
    {
        Debug.Log("Cargando nivel: " + gameObject.name);
    }
}