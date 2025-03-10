using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float areaWidth = 10.0f;
    [SerializeField] private float areaHeight = 5.0f;
    [SerializeField] private float nodeOffsetX = 0.5f;
    [SerializeField] private float nodeOffsetY = 0.5f;
    [SerializeField] private GameObject nodeObject;

    public List<List<Node>> nodes;

    void Start()
    {
        CreateNodes();
    }

    void CreateNodes()
    {
        nodes = new List<List<Node>>();
        if (nodeOffsetX <= 0 || nodeOffsetY <= 0)
        {
            Debug.LogError("Node offsets must be positive");
            return;
        }
        for (int x = 0; x * nodeOffsetX < areaWidth; ++x)
        {
            List<Node> row = new List<Node>();
            nodes.Add(row);
            for (int y = 0; y * nodeOffsetY < areaHeight; ++y)
            {
                Vector3 offset = new Vector3(x * nodeOffsetX, y * nodeOffsetY);
                GameObject newNode = Instantiate(nodeObject, transform.position + offset, transform.rotation, transform);
                Node nodeComponent = newNode.GetComponent<Node>();
                SetAdjacentNodes(nodeComponent, x, y);
                row.Add(nodeComponent);
            }
        }
    }

    // Adds connections to Nodes adjacent to the given Node in the grid
    void SetAdjacentNodes(Node node, int x, int y)
    {
        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                if (x + i >= 0 &&
                    x + i < nodes.Count &&
                    y + j >= 0 &&
                    y + j < nodes[x + i].Count)
                {
                    node.connections.Add(nodes[x + i][y + j]);
                    nodes[x + i][y + j].connections.Add(node);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()

    {
        
    }
}
