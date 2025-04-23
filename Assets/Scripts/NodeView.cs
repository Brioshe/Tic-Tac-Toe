using UnityEngine;

public class NodeView : MonoBehaviour {
    public GameObject tile;
    public float borderSize = 0.01f;
    TicTacToeGame game;

    TextMesh text;
    Node node;
    public void Init(Node node, TicTacToeGame game) {
        if (tile != null) {
            this.node = node;
            this.game = game;
            tile.name = "Node (" + node.position.x + ", " + node.position.z + ")";
            tile.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);

            tile.AddComponent<BoxCollider>();

            GameObject t = new GameObject();
            text = t.AddComponent<TextMesh>();
            text.text = "";
            text.fontSize = 80;
            text.characterSize = 0.1f;
            text.color = Color.black;

            text.transform.position = node.position;

            text.transform.eulerAngles = new Vector3(90, 0, 0);
            text.anchor = TextAnchor.MiddleCenter;

        } else {
            Debug.LogWarning("Tile does not exist!");
        }
    }

    void ColorNode(Color color, GameObject gameObject) {
        if (gameObject != null) {
            Renderer gameObjectRenderer = gameObject.GetComponent<Renderer>();
            gameObjectRenderer.material.color = color;
        }
    }

    public void ColorNode(Color color) {
        ColorNode(color, tile);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject == tile) {
                    game.OnCellClicked(node.xIndex, node.yIndex);
                }
            }
        }
    }
    public void DrawText(string s) {
        text.text = s;
    }
}