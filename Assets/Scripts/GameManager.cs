using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Public Variables for Unity Editor
    public Text dialogueTXT;
    public Image akinatorSprite;
    public Sprite akinatorNormal, akinatorHappy;
    public Button yesBTN, noBTN;
    public GameObject startOverBTN;

    //Variables for developersToolPanel
    public GameObject developersToolPanel;
    public Text textTXT;
    public Text inputTXT;
    public GameObject agregarBTN, yesbtn, nobtn;
    public Button exitBTN;
    private string answer;

    //Variables for the Nodes and Tree
    public Node currentNode;
    public QuestionsManager questionsManager;

    //Private Variables
    private bool win;

    public void Start()
    {
        //Initialize the Tree and start on Root
        currentNode = questionsManager.AVL.Root;
        TXTAssigments();
        win = false;
        agregarBTN.SetActive(true);
        yesbtn.SetActive(false);
        nobtn.SetActive(false);
        exitBTN.interactable = false;
    }

    //Function to change the questions on the interface
    private void TXTAssigments()
    {
        dialogueTXT.text = currentNode.question;
    }

    //Function to detect if the node has a question on it
    private bool CheckQuestion(string _text)
    {
        return _text.StartsWith("¿") && _text.EndsWith("?");
    }

    //Function to help restart the Game
    public void StartOver()
    {
        currentNode = questionsManager.AVL.Root;
        TXTAssigments();
        win = false;
        yesBTN.interactable = true;
        noBTN.interactable = true;
        startOverBTN.SetActive(false);
        agregarBTN.SetActive(true);
        yesbtn.SetActive(false);
        nobtn.SetActive(false);
        exitBTN.interactable = false;
    }

    //Function for the Yes Button
    public void ClickYes()
    {
        if (win == false)
        {
            //First verifies if the next nodes are null, if they are, tell the user they reached the end of the Tree and if they would like to add more Nodes
            if (currentNode.yes == null && currentNode.no == null)
            {
                DeveloperTools();
            }
            else if (CheckQuestion(currentNode.question) == true) //If it detects that the question has ¿?
            {
                //Verifies if the next node is null, if not, moves through the tree
                if (currentNode.yes != null)
                {
                    currentNode = currentNode.yes;
                    TXTAssigments();
                }
            }
            else //If the questions doesn't have ¿? then its an answer
            {
                AnswerFound();
            }
        } 
        else
        {
            dialogueTXT.text = "¡Akinator ha adivinado!. ¿Desea jugar de nuevo?";
            yesBTN.interactable = false;
            noBTN.interactable = false;
            startOverBTN.SetActive(true);
        }
    }

    //Function for the No Button
    public void ClickNo()
    {
        if (win == false)
        {
            //First verifies if the next nodes are null, if they are, tell the user they reached the end of the Tree and if they would like to add more Nodes
            if (currentNode.yes == null && currentNode.no == null)
            {
                DeveloperTools();
            }
            else if (CheckQuestion(currentNode.question) == true) //If it detects that the question has ¿?
            {
                //Verifies if the next node is null, if not, moves through the tree
                if (currentNode.no != null)
                {
                    currentNode = currentNode.no;
                    TXTAssigments();
                }
            }
            else //If the questions doesn't have ¿? then its an answer
            {
                AnswerFound();
            }
        }
        else
        {
            dialogueTXT.text = "¡Akinator ha adivinado!. ¿Desea jugar de nuevo?";
            yesBTN.interactable = false;
            noBTN.interactable = false;
            startOverBTN.SetActive(true);
        }
    }

    //Function for what will happen on screen once it founds an answer on the Tree
    private void AnswerFound()
    {
        dialogueTXT.text = "¿Esta pensando en " + currentNode.question + "?";
        if (akinatorSprite != null && akinatorHappy != null)
        {
            akinatorSprite.sprite = akinatorHappy;
            win = true;
        }
    }

    //Function that works once the user reached the end of the tree
    private void DeveloperTools()
    {
        developersToolPanel.SetActive(true);
        textTXT.text = "Ha llegado al final de las preguntas disponibles. Escriba el nombre de lo que estaba pensando.";
    }

    //Function for the AcceptBTN
    public void AgregarBTN()
    {
        exitBTN.interactable = false;
        agregarBTN.SetActive(false);
        answer = inputTXT.text;
        yesbtn.SetActive(true);
        nobtn.SetActive(true);
        textTXT.text = "Escriba las preguntas que llevaran a su respuesta.";
    }

    //Function to exit the Developer Tools Panel
    public void ExitDeveloperTools()
    {
        developersToolPanel.SetActive(false);
        StartOver();
    }
}
