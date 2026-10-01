using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance { get; private set; }

    [SerializeField] private Grid grid; // uma grid na unity

        //dicionario com: chave = coordenada((x1,y1)) e valor = entity(player, inimigo etc)
    private readonly Dictionary<Vector2Int, GridEntity> occupants = new(); 

    void Awake() => Instance = this; //cria uma instacia da grid

    public Vector2Int WorldToCell(Vector3 world)
    {
        var c = grid.WorldToCell(world); // 
        return new Vector2Int(c.x, c.y); //normaliza a posição vector3 pra um int vector2
    }

    public Vector3 CellToWorld(Vector2Int cell) =>
        grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0)); //transforma o int vector2 em vector3 

        // se a cell/coordenada não estiver vazia retorna true
    public bool IsFree(Vector2Int cell) => !OutBounds(cell) && !occupants.ContainsKey(cell);

    //metodos
        
        //qual entity (letra e) está na cell? 
    public GridEntity GetAt(Vector2Int cell) => //adiquire o valor da cell. se preenchida, retorna e
        occupants.TryGetValue(cell, out var e) ? e : null;//senão, retorna null.

        //atribui uma entity a uma cell 
    public void Place(GridEntity e, Vector2Int cell) => occupants[cell] = e; 

    public void Move(GridEntity e, Vector2Int from, Vector2Int to)
    {
        occupants.Remove(from); //retira a cell (chave)
        occupants[to] = e;      //adiciona uma nova cel a entidade. a entidade moveu.
    }

        //player spawna na cell (-3,-2) na grid
    public bool OutBounds(Vector2Int cell) =>
        cell.x < -3 || cell.y < -2 || cell.x > 6 || cell.y > 7;


}