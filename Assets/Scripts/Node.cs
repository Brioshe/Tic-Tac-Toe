using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node {
    // this is a single node in a graph
    public CellState cellState = CellState.Empty;
    public int xIndex = -1;
    public int yIndex = -1;
    public Vector3 position;

    public List<Node> neighbors = new List<Node>();
    public Node prev = null;
    public Node(int xIndex, int yIndex, CellState cellState) {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.cellState = cellState;
        position = new Vector3(xIndex, 0, yIndex);
    }

    public void Reset() {
        prev = null;
    }
}