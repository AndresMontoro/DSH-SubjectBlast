using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase que inicializa las fichas y maneja el comportamiento
public class ManejadorTablero : MonoBehaviour
{
    public int maximoFilas;
    public int maximoColumn;

    [Tooltip("Las fichas deben añadirse en este orden: ")]
    public GameObject[] arrayFichas;

    bool actualizandoTablero;

    Transform[,] posicionesFichas;

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

    struct Movimiento
    {
        public int filaOrigen, filaDestino, columnaOrigen, columnaDestino;
        public Movimiento(int fO, int fD, int cO, int cD) { filaOrigen = fO; filaDestino = fD; columnaOrigen = cO; columnaDestino = cD; }
    }

    //Matriz de NxN fichas.
    _ficha[,] matrizLogica;

    void IniciarMatriz()
    {
        //TODO
    }

    void PintarTablero()
    {
        //TODO
    }

    void ObtenerPosicionesFichas()
    {
        //TODO
    }

    void Awake()
    {
        IniciarMatriz();
        ObtenerPosicionesFichas();
    }

    // Start is called before the first frame update
    void Start()
    {
        PintarTablero();
    }

    IEnumerator MoverFichas (Movimiento movimiento)
    {
        yield return null;
    }

    _ficha GenerarFichas ()
    {
        //TODO
        return _ficha.VACIO;
    }

    IEnumerator ActualizarTablero(Movimiento movimiento)
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //COMPORTAMIENTO DE LA JUGADA

    }
}
