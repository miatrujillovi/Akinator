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
        currentNode.no = new Node("Empty No Question", 0);
        Debug.Log("New node NO created");

        //Check current FE Count
        FeManager(currentNode);

        //Changing currentNode to the new Node
        currentNode = currentNode.no;
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
        currentNode.yes = new Node("Empty Yes Question", 0);
        Debug.Log("New node YES created");

        //Check current FE Count
        FeManager(currentNode);

        //Changing currentNode to the new Node
        currentNode = currentNode.yes;
    }

    //Function that checks the balance of the Nodes
    public void FeManager(Node _node)
    {
        if (_node == null) return;

        //Gets the heights of the two sides
        int leftHeight = GetHeight(_node.no);
        int rightHeight = GetHeight(_node.yes);

        //Calculates the Fe of the Node
        _node.Fe = leftHeight - rightHeight;

        //Verifies if the number of the Fe calls for a Rotation to make the tree balanced
        
    }

    //Function that calculates the height of a Node
    public int GetHeight(Node _node)
    {
        if (_node == null) return 0;

        int leftHeight = GetHeight(_node.no);
        int rightHeight = GetHeight(_node.yes);

        return 1 + Mathf.Max(leftHeight, rightHeight);
    }

    //Function that rotates the Nodes if the FE is unbalanced
    public Node NodeRotation(Node _unbalancedNode, string rotationType)
    {
        if (rotationType == "Right")
        {
            return RotateRight(_unbalancedNode);
        }
        else if (rotationType == "Left")
        {
            return RotateLeft(_unbalancedNode);
        }
        return null;
    }

    //Rotation of the Nodes to the Right side of the Tree
    private Node RotateRight(Node _unbalancedNode)
    {
        Node leftChild = _unbalancedNode.no;
        Node leftSubTreeRightChild = leftChild.yes;

        //Rotation...
        leftChild.yes = _unbalancedNode;
        _unbalancedNode.no = leftSubTreeRightChild;

        //Update FE of the Rotated Nodes
        FeManager(_unbalancedNode);
        FeManager(leftChild);

        return leftChild;
    }

    //Rotation of the Nodes to the Left Side of the Tree
    private Node RotateLeft(Node _unbalancedNode)
    {
        Node rightChild = _unbalancedNode.yes;
        Node rightSubTreeLeftChild = rightChild.no;

        //Rotation...
        rightChild.no = _unbalancedNode;
        _unbalancedNode.yes = rightSubTreeLeftChild;

        //Update FE of the Rotated Nodes
        FeManager(_unbalancedNode);
        FeManager(rightChild);

        return rightChild;
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

    //Function that Updates the JSON with new Nodes
    public void JSONUpdate()
    {

    }
}
