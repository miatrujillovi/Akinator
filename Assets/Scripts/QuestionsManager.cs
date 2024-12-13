using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class QuestionsManager : MonoBehaviour
{
    //Public Variables to Unity Interface
    public Text questionTXT;

    //Variables to navigate Tree
    public Node currentNode;
    public Tree AVL;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        JSONLoad();
        //Initialization of the Tree
        currentNode = AVL.Root;
        Debug.Log("Nodo actual establecido en la raíz: " + currentNode.question);
        if (AVL == null || AVL.Root == null)
        {
            Debug.LogError("El árbol AVL o su raíz están vacíos. No se puede guardar en JSON.");
            return;
        }

    }

    //Function to add question marks to the questions
    public string AddQuestionMarks(string _text)
    {
        if (!_text.StartsWith("¿"))
        {
            _text = "¿" + _text;
        }
        if (!_text.EndsWith("?"))
        {
            _text += "?";
        }
        return _text;
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
            string result = AddQuestionMarks(questionTXT.text);
            currentNode.question = result;
        }

        //Creating a new no Node
        if (currentNode.no == null)
        {
            currentNode.no = new Node("Empty No Question");
            Debug.Log("New node NO created");
        }

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Save currentNode in JSON
        JSONSave();

        //Changing currentNode to the new Node
        currentNode = currentNode.no;

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
            string result = AddQuestionMarks(questionTXT.text);
            currentNode.question = result;
        }

        //Creating a new yes Node
        if (currentNode.yes == null)
        {
            currentNode.yes = new Node("Empty Yes Question");
            Debug.Log("New node YES created");
        }

        //Check current FE Count
        AVL.Root = FeManager(AVL.Root);

        //Save currentNode in JSON
        JSONSave();

        //Changing currentNode to the new Node
        currentNode = currentNode.yes;

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

    //Function to save the Tree/Nodes onto the JSON
    public void JSONSave()
    {
        if (AVL == null)
        {
            Debug.Log("El arbol esta vacio");
            return;
        }

        string json = JsonConvert.SerializeObject(AVL, Formatting.Indented);
        string filePath = Application.persistentDataPath + "/tree.json";
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("Arbol guardado Exitosamente");
    }

    //Function to load the Tree/Nodes from the JSON
    public void JSONLoad()
    {
        string filePath = Application.persistentDataPath + "/tree.json";

        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            AVL = JsonConvert.DeserializeObject<Tree>(json);
            currentNode = AVL.Root;
            Debug.Log("Árbol cargado exitosamente.");
        } 
        else
        {
            Debug.LogWarning("No se encontró el archivo JSON.");
        }
    }
}
