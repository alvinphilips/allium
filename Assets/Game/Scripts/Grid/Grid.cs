using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Grid<T> : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    public int CellCount => rows * columns;

    public int Index(int x, int y) => y * columns + x;

    private readonly Dictionary<int, T> _data = new();


    void Insert(T value, int x, int y) {
        var c = Maths.Clamp(x, 0, columns);
        var r = Maths.Clamp(y, 0, rows);

        var index = Index(c, r);

        _data.TryAdd(index, value);
    }
}
