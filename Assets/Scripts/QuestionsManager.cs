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
    }

    //Function that when ->noBTN<- is clicked, it creates a Node on the Left side of the Tree
    public void LeftNode()
    {
        //Saving the question on the input
        if (currentNode != null)
        {
            currentNode.question = questionTXT.text;
        }

        //Creating a new no Node
        currentNode.no = new Node("Empty No Question");
        Debug.Log("New node NO created");

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Changing currentNode to the new Node
        currentNode = currentNode.no;

        //Save currentNode in JSON
        JSONSave();
    }

    //Function that when ->yesBTN<- is clicked, it creates a Node on the Right Side of the Tree
    public void RightNode() 
    {
        //Saving the question on the input
        if (currentNode != null)
        {
            currentNode.question = questionTXT.text;
        }

        //Creating a new yes Node
        currentNode.yes = new Node("Empty Yes Question");
        Debug.Log("New node YES created");

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Changing currentNode to the new Node
        currentNode = currentNode.yes;

        //Save currentNode in JSON
        JSONSave();
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
            if (_father.no != null && _father.no.Fe >= 0)
            {
                return RotateRight(_father); //Simple Right Rotation
            } 
            else if (_father.no != null && _father.no.Fe < 0)
            {
                //Double Right Rotation
                _father.no = RotateLeft(_father.no);
                return RotateRight(_father);
            }
        } 
        else if (_father.Fe < -1) { //If th disbalance is to the right
            if (_father.yes != null && _father.yes.Fe <= 0)
            {
                return RotateLeft(_father); //Simple Left Rotation
            } 
            else if (_father.yes != null && _father.yes.Fe > 0)
            {
                //Double Left Rotation
                _father.yes = RotateRight(_father.yes);
                return RotateLeft(_father);
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

    //Rotation of the Nodes to the Left Side of the Tree
    private Node RotateLeft(Node _unbalancedNode)
    {
        if (_unbalancedNode == null || _unbalancedNode.yes == null)
        {
            return _unbalancedNode;
        }

        //Rotating...
        Node rightSubTree = _unbalancedNode.yes;
        _unbalancedNode.yes = rightSubTree.no;
        rightSubTree.no = _unbalancedNode;

        //Recalculating Fe...
        _unbalancedNode.Fe = GetHeight(_unbalancedNode.no) - GetHeight(_unbalancedNode.yes);
        rightSubTree.Fe = GetHeight(rightSubTree.no) - GetHeight(rightSubTree.yes);

        //Save currentNode in JSON
        JSONSave();

        return rightSubTree;
    }

    //Rotation of the Nodes to the Right side of the Tree
    private Node RotateRight(Node _unbalancedNode)
    {
        if (_unbalancedNode == null || _unbalancedNode.no == null)
        {
            return _unbalancedNode;
        }

        //Rotating...
        Node leftSubTree = _unbalancedNode.no;
        _unbalancedNode.no = leftSubTree.yes;
        leftSubTree.yes = _unbalancedNode;

        //Recalculating Fe...
        _unbalancedNode.Fe = GetHeight(_unbalancedNode.no) - GetHeight(_unbalancedNode.yes);
        leftSubTree.Fe = GetHeight(leftSubTree.no) - GetHeight(leftSubTree.yes);

        //Save currentNode in JSON
        JSONSave();

        return leftSubTree;
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
    public void JSONSave()
    {
        TreeData treeData = new TreeData();
        treeData.root = new TreeData.NodeData(AVL.Root);

        string json = JsonUtility.ToJson(treeData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("Árbol guardado en JSON.");
    }

    //Function to load the Data from the JSON
    public void JSONLoad()
    {
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            TreeData treeData = JsonUtility.FromJson<TreeData>(json);

            AVL.Root = DeserializeNode(treeData.root);
            Debug.Log("Árbol cargado desde JSON.");
        }
        else
        {
            Debug.Log("ERROR: El archivo JSON no existe.");
        }
    }

    //Function to help deserialize the Data to one Tree.cs can understand
    private Node DeserializeNode(TreeData.NodeData nodeData)
    {
        if (nodeData == null)
        {
            return null;
        }

        Node node = new Node(nodeData.question);
        node.Fe = nodeData.Fe;
        node.yes = DeserializeNode(nodeData.yes);
        node.no = DeserializeNode(nodeData.no);

        return node;
    }
}
