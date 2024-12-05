using System.Collections;
using System.Collections.Generic;
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
    public Node checkedNode;
    private int level;

    private void Start()
    {
        //Initialization of the Tree
        AVL = new Tree("Empty Root Question");
        currentNode = AVL.Root;
        checkedNode = currentNode;
        level = 0;
    }

    public void Update()
    {
        FeManager();
    }

    //Function that when ->noBTN<- is clicked, it creates a Node on the Left side of the Tree
    public void LeftNode()
    {
        //Saving the question on the input
        questionTXT.text = currentNode.question;

        //Creating a new no Node
        level++;
        currentNode.no = new Node("Empty No Question", level, 0);
        currentNode.Fe = -1;

        //Changing currentNode to the new Node
        currentNode = currentNode.no;
        
    }

    //Function that when ->yesBTN<- is clicked, it creates a Node on the Right Side of the Tree
    public void RightNode() 
    {
        //Saving the question on the input
        questionTXT.text += currentNode.question;

        //Creating a new yes Node
        level++;
        currentNode.yes = new Node("Empty Yes Question", level, 0);
        currentNode.Fe = 1;

        //Changing currentNode to the new Node
        currentNode = currentNode.yes;
    }

    //Function that checks the balance of the Nodes
    public void FeManager()
    {
        Node temp = AVL.Root;
        int feCounter = 0;

        while (temp != null)
        {
            temp.Fe += feCounter;
            if (feCounter == -2 || feCounter == 2)
            {

            }
        }
    }

    //Function that rotates the Nodes if the FE is unbalanced
    public void NodeRotation()
    {

    }

    //Function that Updates the JSON with new Nodes
    public void JSONUpdate()
    {

    }
}
