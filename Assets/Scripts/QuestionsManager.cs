using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    //Public Variables to Unity Interface
    public Text questionTXT;
    public Button yesBTN, noBTN;

    //Variables to navigate Tree
    public Node currentNode;
    public Tree AVL;

    //Private Variables for JSON
    private string filePath = "tree.json";

    private void Start()
    {
        //Initialization of the Tree
        AVL = new Tree("Empty Root Question");
        currentNode = AVL.Root;
        TreeData treeData = new TreeData();
        Debug.Log("Nodo actual establecido en la raíz: " + currentNode.question);
    }

    //Function that when ->noBTN<- is clicked, it creates a Node on the Left side of the Tree
    public void LeftNode()
    {
        //Verifying if currentNode points at Root
        if (currentNode == null)
        {
            Debug.LogWarning("currentNode es nulo. Restableciendo a la Raiz.");
            currentNode = AVL.Root;
        }

        //Saving the question on the input
        if (currentNode != null)
        {
            currentNode.question = questionTXT.text;
        }

        //Creating a new no Node
        if (currentNode.no == null)
        {
            currentNode.no = new Node("Empty No Question");
            Debug.Log("New node NO created");
        }

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Changing currentNode to the new Node
        currentNode = currentNode.no;

        //Save currentNode in JSON
        //JSONSave();
    }

    //Function that when ->yesBTN<- is clicked, it creates a Node on the Right Side of the Tree
    public void RightNode() 
    {
        //Verifying if currentNode points at Root
        if (currentNode == null)
        {
            Debug.LogWarning("currentNode es nulo. Restableciendo a la Raiz.");
            currentNode = AVL.Root;
        }

        //Saving the question on the input
        if (currentNode != null)
        {
            currentNode.question = questionTXT.text;
        }

        //Creating a new yes Node
        if (currentNode.yes == null)
        {
            currentNode.yes = new Node("Empty Yes Question");
            Debug.Log("New node YES created");
        }

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Changing currentNode to the new Node
        currentNode = currentNode.yes;

        //Save currentNode in JSON
        //JSONSave();
    }

    //Function that checks the balance of the Nodes
    public Node FeManager(Node _father)
    {
        if (_father == null) return null;

        //Moves through every subtree
        _father.no = FeManager(_father.no);
        _father.yes = FeManager(_father.yes);

        //Gets the heights of the left and right subtrees
        int leftHeight = GetHeight(_father.no);
        int rightHeight = GetHeight(_father.yes);

        //Calculates the Fe of the Node
        _father.Fe = leftHeight - rightHeight;

        //Verifies if the number of the Fe calls for a Rotation to make the tree balanced
        if (_father.Fe > 1) //If the disbalance is to the left
        {
            if (_father.no.Fe > 0)
            {
                _father = RotateLL(_father); //Left Left Rotation
            } 
            else
            {
                _father = RotateLR(_father); //Left Right Rotation
            }
        } else if (_father.Fe < -1) { //If the disbalance is to the right
            if (_father.yes.Fe > 0)
            {
                _father = RotateRL(_father); //Right Left Rotation
            } 
            else
            {
                _father = RotateRR(_father); //Right Right Rotation
            }
        }

        return _father;
    }

    //Function that calculates the height of a Node
    public int GetHeight(Node _node)
    {
        if (_node == null) return 0;

        int leftHeight = GetHeight(_node.no);
        int rightHeight = GetHeight(_node.yes);

        return 1 + Mathf.Max(leftHeight, rightHeight);
    }

    //Function for Left Left Rotation
    private Node RotateLL(Node _unbalancedNode)
    {
        //Rotating...
        Node pivot = _unbalancedNode.no;
        _unbalancedNode.no = pivot.yes;
        pivot.yes = _unbalancedNode;

        //Recalculating Fe...
        _unbalancedNode.Fe = GetHeight(_unbalancedNode.no) - GetHeight(_unbalancedNode.yes);
        pivot.Fe = GetHeight(pivot.no) - GetHeight(pivot.yes);

        return pivot;
    }

    //Function for Right Right Rotation
    private Node RotateRR(Node _unbalancedNode)
    {
        //Rotating...
        Node pivot = _unbalancedNode.yes;
        _unbalancedNode.yes = pivot.no;
        pivot.no = _unbalancedNode;

        //Recalculating Fe...
        _unbalancedNode.Fe = GetHeight(_unbalancedNode.no) - GetHeight(_unbalancedNode.yes);
        pivot.Fe = GetHeight(pivot.no) - GetHeight(pivot.yes);

        return pivot;
    }

    //Function for Left Right Rotation
    private Node RotateLR(Node _unbalancedNode)
    {
        //Rotating
        Node pivot = _unbalancedNode.no;
        _unbalancedNode.no = RotateRR(pivot);

        return RotateLL(_unbalancedNode);
    }

    //Function for Right Left Rotation
    private Node RotateRL(Node _unbalancedNode)
    {
        //Rotating
        Node pivot = _unbalancedNode.yes;
        _unbalancedNode.yes = RotateLL(pivot);

        return RotateRR(_unbalancedNode);
    }

    //Function to print the current tree
    public void CheckTree()
    {
        if (AVL.Root == null)
        {
            Debug.Log("El Arbol esta Vacio.");
            return;
        }

        Debug.Log("Estructura del Arbol:");
        TraverseTree(AVL.Root, 0);
    }

    //Recursive function to get the information of every node
    private void TraverseTree(Node currentNode, int level)
    {
        if (currentNode == null)
        {
            return;
        }

        Debug.Log($"Nivel {level}: Pregunta = \"{currentNode.question}\", Fe = {currentNode.Fe}, " +
                  $"Yes -> {(currentNode.yes != null ? currentNode.yes.question : "null")}, " +
                  $"No -> {(currentNode.no != null ? currentNode.no.question : "null")}");

        TraverseTree(currentNode.no, level++);
        TraverseTree(currentNode.yes, level++);
    }

    //Function that saves the Nodes in a JSON
    public void JSONSave(Node _data)
    {
        //Save the string of TreeData inside the JSON
        string json = JsonUtility.ToJson(treeData);
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log("Árbol guardado en JSON:");
        Debug.Log(json);
    }

    private Node SerializeTree(Node _node)
    {
        return _node;
    }

    //Function to load the Data from the JSON
    public void JSONLoad()
    {
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            TreeData treeData = JsonUtility.FromJson<TreeData>(json);

            // Reconstruye el árbol a partir del JSON deserializado
            //AVL.Root = DeserializeNode(treeData);

            // Actualiza el nodo actual para que apunte a la raíz
            currentNode = AVL.Root;

            Debug.Log("Árbol cargado desde JSON.");
        }
        else
        {
            Debug.Log("ERROR: El archivo JSON no existe.");
        }
    }

    //Function to help deserialize the Data to one Tree.cs can understand
    private Node DeserializeTree(TreeData.NodeData nodeData)
    {
        if (nodeData == null)
        {
            return null;
        }

        Node node = new Node(nodeData.question);
        node.Fe = nodeData.Fe;
        node.yes = DeserializeTree(nodeData.yes);
        node.no = DeserializeTree(nodeData.no);

        return node;
    }
}
