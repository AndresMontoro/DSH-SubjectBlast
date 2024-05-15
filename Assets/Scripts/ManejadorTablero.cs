using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase que inicializa las fichas y maneja el comportamiento
public class ManejadorTablero : MonoBehaviour
{
    public int maximoFilas;
    public int maximoColumn;

    public float distanciaAlturaCentro = 5.0f;
    public float distanciaAnchuraCentro = 5.0f;

    int filai = -100, coli = -100, filaj = -100, colj = -100;
    bool setedFich;

    [Tooltip("Las fichas deben añadirse en este orden: ")]
    public GameObject[] arrayFichas;

    bool actualizandoTablero;

    GameObject[,] fichas;

    float _ladoCasilla;

    enum _ficha
    {
        VACIO,
        FICHA_1,
        FICHA_2,
        FICHA_3,
        FICHA_4,
        FICHA_5,
        FICHA_6
    }

    //Matriz de NxN fichas.
    _ficha[,] matrizLogica;

    void checkMovimiento()
    {
        if (Mathf.Abs(filai - filaj) == 1 ^ Mathf.Abs(coli - colj) == 1)
        {
            MoverFicha(filai, filaj, coli, colj);
            filai = -100;
            filaj = -100;
            coli = -100;
            colj = -100;
        }
        else
        {
            filai = -100;
            filaj = -100;
            coli = -100;
            colj = -100;
            setedFich = false;
        }
    }

    public void Movimiento (int fila, int col)
    {
        if (setedFich)
        {
            filaj = fila;
            colj = col;
            checkMovimiento();
        }
        else
        {
            filai = fila;
            coli = col;
        }
    }

    void MoverFicha(int fila1, int col1, int fila2, int col2)
    {
        (matrizLogica[fila1, col1], matrizLogica[fila2, col2]) = (matrizLogica[fila2, col2], matrizLogica[fila1, col1]);
        //IENUMERATOR que mueve visiblemente las fichas
        (fichas[fila1, col1], fichas[fila2, col2]) = (fichas[fila2, col2], fichas[fila1, col1]);
        fichas[fila1, col1].GetComponent<Ficha>().setPos(fila1, col1);
        fichas[fila2, col2].GetComponent<Ficha>().setPos(fila2, col2);
    }

    void IniciarMatriz()
    {
        //TODO
    }

    void PintarTablero()
    {
        //TODO
    }

    void Awake()
    {
        IniciarMatriz();
    }

    // Start is called before the first frame update
    void Start()
    {
        PintarTablero();
    }

    // Update is called once per frame
    void Update()
    {
        //COMPORTAMIENTO DE LA JUGADA

    }
}
