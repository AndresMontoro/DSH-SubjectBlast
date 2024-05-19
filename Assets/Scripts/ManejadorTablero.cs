using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//Clase que inicializa las fichas y maneja el comportamiento
public class ManejadorTablero : MonoBehaviour
{
    public int maximoFilas;
    public int maximoColumn;

    public int puntosParaDar = 0;
    public int puntosTotales = 0;
    public int multiplicador = 1;

    public float distanciaAlturaCentro = 5.0f;
    public float distanciaAnchuraCentro = 5.0f;

    int filai = -100, coli = -100, filaj = -100, colj = -100;
    bool setedFich;

    [Tooltip("Las fichas deben a�adirse en este orden: ")]
    public GameObject[] arrayFichas;

    bool actualizandoTablero;
    bool fichasCayendo;

    GameObject[,] fichas;

    float _ladoCasilla;

    List<Vector2Int> combos;

    enum _ficha
    {
        FICHA_1 = 0,
        FICHA_2 = 1,
        FICHA_3 = 2,
        FICHA_4 = 3,
        FICHA_5 = 4,
        FICHA_6 = 5,
        VACIO = 6
    }

    //Matriz de NxN fichas.
    _ficha[,] matrizLogica;

    IEnumerator movimientoVisual(GameObject[] param)
    {
        Vector3 pos1 = param[0].GetComponent<Transform>().position;
        Vector3 pos2 = param[1].GetComponent<Transform>().position;
        Vector3 posOrg1 = new Vector3(-param[0].GetComponent<Ficha>().getFila() * distanciaAnchuraCentro, param[0].GetComponent<Ficha>().getCol() * distanciaAlturaCentro, pos1.z);
        Vector3 posOrg2 = new Vector3(-param[1].GetComponent<Ficha>().getFila() * distanciaAnchuraCentro, param[1].GetComponent<Ficha>().getCol() * distanciaAlturaCentro, pos2.z);
        Vector3 dir1 = (posOrg2 - pos1) / 10;
        Vector3 dir2 = (posOrg1 - pos2) / 10;
        while (pos1 != posOrg2 && pos2 != posOrg1)
        {
            pos1 += dir1;
            pos2 += dir2;
            yield return new WaitForSeconds(.1f);
        }
        actualizandoTablero = false;
    }

    //Funcion que mueve la ficha en la matriz logica y llama al movimiento visual
    void MoverFicha(int fila1, int col1, int fila2, int col2)
    {
        (matrizLogica[fila1, col1], matrizLogica[fila2, col2]) = (matrizLogica[fila2, col2], matrizLogica[fila1, col1]);

        (fichas[fila1, col1], fichas[fila2, col2]) = (fichas[fila2, col2], fichas[fila1, col1]);
        fichas[fila1, col1].GetComponent<Ficha>().setPos(fila1, col1);
        fichas[fila2, col2].GetComponent<Ficha>().setPos(fila2, col2);

        //IENUMERATOR que mueve visiblemente las fichas
        actualizandoTablero = true;
        GameObject[] parms = new GameObject[2] { fichas[fila1, col1], fichas[fila2, col2] };
        StartCoroutine("movimientoVisual", parms);
        //movimientoVisual(fichas[fila1,col1], fichas[fila2,col2]);

        combos = comprobarJugada();
    }

    //Funcion que checkea si el movimiento es correcto
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
    
    //Funcion Publica que llaman las fichas
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

    List<Vector2Int> comprobarJugada()
    {
        List<Vector2Int> fichasCombo = new List<Vector2Int>();

        // Detectar combos horizontales
        for (int i = 0; i < maximoFilas; i++)
        {
            for (int j = 0; j < maximoColumn - 2; j++)
            {
                if (matrizLogica[i, j] != _ficha.VACIO &&
                    matrizLogica[i, j] == matrizLogica[i, j + 1] &&
                    matrizLogica[i, j] == matrizLogica[i, j + 2])
                {
                    fichasCombo.Add(new Vector2Int(i, j));
                    fichasCombo.Add(new Vector2Int(i, j + 1));
                    fichasCombo.Add(new Vector2Int(i, j + 2));
                }
            }
        }

        // Detectar combos verticales
        for (int j = 0; j < maximoColumn; j++)
        {
            for (int i = 0; i < maximoFilas - 2; i++)
            {
                if (matrizLogica[i, j] != _ficha.VACIO &&
                    matrizLogica[i, j] == matrizLogica[i + 1, j] &&
                    matrizLogica[i, j] == matrizLogica[i + 2, j])
                {
                    fichasCombo.Add(new Vector2Int(i, j));
                    fichasCombo.Add(new Vector2Int(i + 1, j));
                    fichasCombo.Add(new Vector2Int(i + 2, j));
                }
            }
        }

        // Eliminar duplicados
        fichasCombo = fichasCombo.Distinct().ToList();

        return fichasCombo;
    }


    void generarNuevasFichas()
    {

    }

    void caidaLogica ()
    {
        // Recorremos cada columna de la matriz
        for (int col = 0; col < maximoColumn; col++)
        {
            // Inicializamos emptyRow a la �ltima fila
            int emptyRow = maximoFilas - 1;

            // Recorremos cada fila de abajo hacia arriba
            for (int row = maximoFilas - 1; row >= 0; row--)
            {
                // Si la celda actual no est� vac�a (es decir, est� ocupada)
                if (matrizLogica[row, col] != _ficha.VACIO)
                {
                    // Movemos la celda ocupada a la posici�n de emptyRow si no est� ya all�
                    if (emptyRow != row)
                    {
                        // Colocamos la celda ocupada en la posici�n de emptyRow
                        matrizLogica[emptyRow, col] = matrizLogica[row, col];
                        // Vaciamos la celda original
                        matrizLogica[row, col] = _ficha.VACIO;
                        fichas[row, col].GetComponent<Ficha>().setPos(row, col);
                    }
                    // Actualizamos emptyRow para la siguiente celda ocupada
                    emptyRow--;
                }
            }
        }

    }


    void IniciarMatriz()
    {
        matrizLogica = new _ficha[maximoFilas, maximoColumn];
        for (int i = 0; i < maximoFilas; i++)
        {
            for (int j = 0; j < maximoColumn; j++)
            {
                matrizLogica[i, j] = (_ficha)Random.Range((int)_ficha.FICHA_1, 2);
            }
        }
    }

    void PintarTablero()
    {
        Transform transformOriginal = this.GetComponent<Transform>();
        fichas = new GameObject[maximoFilas, maximoColumn];
        for (int i = 0; i <maximoFilas; i++)
        {
            for (int j = 0; j < maximoColumn; j++)
            {
                fichas[i, j] = Instantiate(arrayFichas[(int)matrizLogica[i,j]], new Vector3 (i * distanciaAnchuraCentro, transformOriginal.position.z, j * distanciaAlturaCentro), Quaternion.identity);
                fichas[i, j].GetComponent<Ficha>().setPos(i, j);
            }
        }
    }

    void Awake ()
    {
        IniciarMatriz();
        combos = new List<Vector2Int>();
    }

    // Start is called before the first frame update
    void Start ()
    {
        PintarTablero();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (combos.Count() != 0 && !actualizandoTablero)
        {
            for (int i = 0; i < combos.Count() - 1; i++)
            {
                int offset = 0;
                if (combos[i].x == combos[i+1].x)
                {
                    for (int j = 0; combos[i].x == combos[i + j].x; j++)
                    {
                        puntosTotales += puntosParaDar * multiplicador;
                        offset = j;
                        Destroy(fichas[combos[i + j].x, combos[i + j].y]);
                        matrizLogica[combos[i + j].x, combos[i + j].y] = _ficha.VACIO;
                    }
                    i += offset;
                    multiplicador += 1;
                }
                else
                {
                    for (int j = 0; combos[i].y == combos[i + j].y; j++)
                    {
                        puntosTotales += puntosParaDar * multiplicador;
                        offset = j;
                        Destroy(fichas[combos[i + j].x, combos[i + j].y]);
                        matrizLogica[combos[i + j].x, combos[i + j].y] = _ficha.VACIO;
                    }
                    i += offset;
                    multiplicador += 1;
                }
            }

            caidaLogica();

            for (int i = 0; i < maximoFilas; i++)
            {
                for (int j = 0; j < maximoColumn; j++)
                {

                }
            }

            //

            //Generar nuevas fichas con la lista COMBOS
        }
    }
}
