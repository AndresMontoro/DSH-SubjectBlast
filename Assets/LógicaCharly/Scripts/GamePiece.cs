using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int x;
    private int z;
    private Grid.PieceType type;
    private Grid grid;
    private MovablePiece movableComponent;
    private SubjectPiece subjectComponent;
    private ClearablePiece clearableComponent;

    public int X { 
        get { return x; } 
        set { if (IsMovable()) { x = value; } }
    }
    public int Z { 
        get { return z; } 
        set { if (IsMovable()) { z = value; } }
    }
    public Grid.PieceType Type { get { return type; } }
    public Grid GridRef { get { return grid; } }
    public MovablePiece MovableComponent { get { return movableComponent; } }
    public SubjectPiece SubjectComponent { get { return subjectComponent; } }
    public ClearablePiece ClearableComponent { get { return clearableComponent; } }

    void Awake() 
    {
        movableComponent = GetComponent<MovablePiece>();
        subjectComponent = GetComponent<SubjectPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _x, int _z, Grid _grid, Grid.PieceType _type) 
    {
        x = _x;
        z = _z;
        grid = _grid;
        type = _type;
    }

    public void OnMouseEnter()
    {
        grid.EnterPiece(this);
    }

    public void OnMouseDown() 
    {
        grid.PressPiece(this);
    }

    public void OnMouseUp() 
    {
        grid.ReleasePiece();
    }

    public bool IsMovable() { return movableComponent != null; }
    public bool IsSubject() { return subjectComponent != null; }

    public bool IsClearable() { return clearableComponent != null; }


}
