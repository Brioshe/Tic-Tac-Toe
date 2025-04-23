using System.Collections.Generic;
using UnityEngine;
public enum CellState {
    Empty,
    X,
    O
}
public class GraphClass : MonoBehaviour {
    // Translates 1's and 0's from MapData.cs to an array of nodes
    public Node[,] nodes; //Array of nodes
    public List<Node> walls = new List<Node>();

    TicTacToeGame game;

    int[,] m_mapData;
    int width = 3;
    int height = 3;

    public static readonly Vector2[] allDirections = {
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, 1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, 1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, -1f),
    };

    public int GetWidth() {
        return width;
    }
    public int GetHeight() {
        return height;
    }

    public void Init(TicTacToeGame game) {
        this.game = game;

        nodes = new Node[width, height];

        for (int r = 0; r < width; r++) {
            for (int c = 0; c < height; c++) {
                nodes[r, c] = new Node(r, c, CellState.Empty);
            }
        }
    }

    public bool IsWithinBounds(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public List<Node> GetNeighbors(int x, int y, Node[,] nodeArray) {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2 dir in allDirections) {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;
            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null) {
                neighbors.Add(nodeArray[newX, newY]);
            }
        }

        return neighbors;
    }

    public void ResetBoard() {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                nodes[x, y].cellState = CellState.Empty;
            }
        }
    }

    // Checks if the board is full (no empty cells)
    public bool IsBoardFull() {
        foreach (Node n in nodes) {
            if (n.cellState == CellState.Empty) {
                return false;
            }
        }
        return true;
    }
}