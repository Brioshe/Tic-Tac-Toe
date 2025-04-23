using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TicTacToeGame : MonoBehaviour {
    public bool isPlayerXTurn = true; // Player X starts
    public GraphClass graph;
    GraphView graphView;
    LineRenderer lineRenderer;

    UIManager ui;

    public void Start() {
        if (graph != null) {
            graph.Init(this);
            graphView = graph.GetComponent<GraphView>();
            if (graphView != null) {
                graphView.Init(graph, this);
            } else {
                Debug.Log("No graph view is found");
            }
        }

        UpdateBoardUI();

        // LINE PARAMETERS
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit")); 
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
    }
    
    // Function for cell click behavior
    public void OnCellClicked(int x, int y) {
        if (graph.nodes[x, y].cellState == CellState.Empty && isPlayerXTurn) {
            graph.nodes[x, y].cellState = CellState.X;

            isPlayerXTurn = false; // Switch turns
            if (CheckForWinner()) {
                ui.victoryTxt.text = "PLAYER WINS";
            } else if (graph.IsBoardFull()) {
                ui.victoryTxt.text = "DRAW";
            } else {
                AITurn();
            }
        }
    }

    public void DrawWinLine(Vector2 n1, Vector2 n2) {
        lineRenderer.SetPosition(0, new Vector3(n1.x, 0, n1.y)); // Startpoint coords
        lineRenderer.SetPosition(1, new Vector3(n2.x, 0, n2.y)); // Endpoint coords
    }

    // UI update function
    public void UpdateBoardUI() {
        graphView.UpdateNodes(); // Update nodes via graphView
    }

    // AI's turn programming
    public void AITurn() {
        Dictionary<int, Node> valToNode = new Dictionary<int, Node>(); 
        foreach (Node n in graph.nodes) {
            if (n.cellState != CellState.Empty) {
                continue;
            }

            n.cellState = CellState.O;
            int key = Minimax(graph.nodes, true); // Determine move via Minimax algo 

            if (!valToNode.ContainsKey(key)) {
                valToNode.Add(Minimax(graph.nodes, true), n);
            }
            n.cellState = CellState.Empty;
        }

        valToNode[valToNode.Min(x => x.Key)].cellState = CellState.O; // Finds node with lowest value and selects it.

        UpdateBoardUI();
        if (CheckForWinner()) {
            ui.victoryTxt.text = "AI WINS";
        } else {
            isPlayerXTurn = true;
        }
    }
    public int Minimax(Node[,] nodes, bool isMaximizing) { // Minimax algorithm for determining all possible moves
        int utility = WinConditionCheck(false);
        if (utility != 0 || graph.IsBoardFull()) {
            return utility;
        }

        
        CellState playerCell = isMaximizing ? CellState.X : CellState.O; // X: Maximizing, O: minimizing       

        List<int> resultList = new List<int>();
        foreach (Node n in nodes) {
            if (n.cellState != CellState.Empty) {
                continue;
            }

            n.cellState = playerCell;

            resultList.Add(Minimax(nodes, !isMaximizing));

            n.cellState = CellState.Empty;
        }

        return isMaximizing ? Enumerable.Max(resultList) : Enumerable.Min(resultList);
    }

    // Checks if there is a winner
    // 1: X wins
    // 0: O Wins
    public int WinConditionCheck(bool winLine) {
        // Directional checks
        Vector2[] dir1 = { new Vector2(1f, 1f), new Vector2(-1f, -1f) };
        Vector2[] dir2 = { new Vector2(1f, 0f), new Vector2(-1f, 0f) };
        Vector2[] dir3 = { new Vector2(1f, -1f), new Vector2(-1f, 1f) };
        Vector2[] dir4 = { new Vector2(0f, 1f), new Vector2(0f, -1f) };

        Vector2[][] checkDirections = { dir1, dir2, dir3, dir4 }; // List of winning directions

        foreach (Node n in graph.nodes) {
            if (n.cellState == CellState.Empty) {
                continue;
            }

            foreach (Vector2[] dirs in checkDirections) {
                int won = n.cellState == CellState.X ? 1 : -1;
                foreach (Vector2 dir in dirs) {

                    int newX = (int)(n.position.x + dir.x);
                    int newY = (int)(n.position.z + dir.y);
                    if (!(graph.IsWithinBounds(newX, newY) && graph.nodes[newX, newY].cellState == n.cellState)) {
                        won = 0;
                        break;
                    }
                }
                if (won != 0) {
                    if (winLine) {
                        Vector2 n1 = new Vector2(n.position.x + 1.25f * dirs[0].x, n.position.z + 1.25f * dirs[0].y);
                        Vector2 n2 = new Vector2(n.position.x + 1.25f * dirs[1].x, n.position.z + 1.25f * dirs[1].y);
                        DrawWinLine(n1, n2);
                    }
                    return won;
                }
            }
        }
        return 0;
    }

    public bool CheckForWinner() {
        return WinConditionCheck(true) != 0;
    }

    // Resets the game for a new match
    public void ResetGame() {
        graph.ResetBoard();

        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));

        UpdateBoardUI();

        isPlayerXTurn = true; // Player X starts
    }

    public void SetUI(UIManager ui) {
        this.ui = ui;
    }
}