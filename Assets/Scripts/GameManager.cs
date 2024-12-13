using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

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
    public InputField inputField;
    public GameObject agregarBTN, yesbtn, nobtn;
    public Button exitBTN;
    private string answer;

    //Variables for the Nodes and Tree
    //public Node currentNode;
    public QuestionsManager questionsManager;

    //Private Variables
    private bool win;

    public void Start()
    {
        //Initialize the Tree and start on Root
        if (questionsManager.AVL.Root != null)
        {
            QuestionsManager.currentNode = questionsManager.AVL.Root;
        }
        TXTAssigments();
        win = false; //bool to control if the player has won yet
        inputField.onValueChanged.AddListener(FilterInput); //Listener to not let the player write ¿? on the inputField
        agregarBTN.SetActive(true);
        yesbtn.SetActive(false);
        nobtn.SetActive(false);
        exitBTN.interactable = false;
    }

    //Function to change the questions on the interface
    private void TXTAssigments()
    {
        dialogueTXT.text = QuestionsManager.currentNode.question;
    }

    //Function to detect if the node has a question on it
    private bool CheckQuestion(string _text)
    {
        return _text.StartsWith("¿") && _text.EndsWith("?");
    }

    //Function to not let the user write ¿? on the input field
    public void FilterInput(string _input)
    {
        inputField.text = _input.Replace("¿", "").Replace("?", "");
    }

    //Function to help restart the Game
    public void StartOver()
    {
        QuestionsManager.currentNode = questionsManager.AVL.Root;
        TXTAssigments();
        win = false;
        yesBTN.interactable = true;
        noBTN.interactable = true;
        startOverBTN.SetActive(false);
        agregarBTN.SetActive(true);
        yesbtn.SetActive(false);
        nobtn.SetActive(false);
        exitBTN.interactable = false;
        akinatorSprite.sprite = akinatorNormal;
    }

    //Function for the Yes Button
    public void ClickYes()
    {
        if (win == false)
        {
            //First verifies if the next nodes are null, if they are, tell the user they reached the end of the Tree and if they would like to add more Nodes
            if (QuestionsManager.currentNode.question == "Empty Yes Question" || QuestionsManager.currentNode.yes == null && QuestionsManager.currentNode.no == null)
            {
                DeveloperTools();
            }
            else if (CheckQuestion(QuestionsManager.currentNode.question) == true) //If it detects that the question has ¿?
            {
                //Verifies if the next node is null, if not, moves through the tree
                if (QuestionsManager.currentNode.yes != null)
                {
                    QuestionsManager.currentNode = QuestionsManager.currentNode.yes;
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
            if (QuestionsManager.currentNode.question == "Empty No Question" || QuestionsManager.currentNode.yes == null && QuestionsManager.currentNode.no == null)
            {
                DeveloperTools();
            }
            else if (CheckQuestion(QuestionsManager.currentNode.question) == true) //If it detects that the question has ¿?
            {
                //Verifies if the next node is null, if not, moves through the tree
                if (QuestionsManager.currentNode.no != null)
                {
                    QuestionsManager.currentNode = QuestionsManager.currentNode.no;
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
            QuestionsManager.currentNode = QuestionsManager.currentNode.no;
            win = false;
            akinatorSprite.sprite = akinatorNormal;
            TXTAssigments();
        }
    }

    //Function for what will happen on screen once it founds an answer on the Tree
    private void AnswerFound()
    {
        dialogueTXT.text = "¿Esta pensando en " + QuestionsManager.currentNode.question + "?";
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
        textTXT.text = "Ha llegado al final de las preguntas disponibles. Escriba el nombre de lo que estabas pensando.";
    }

    //Function for the AcceptBTN
    public void AgregarBTN()
    {
        exitBTN.interactable = true;
        agregarBTN.SetActive(false);
        answer = inputTXT.text;
        yesbtn.SetActive(true);
        nobtn.SetActive(true);
        textTXT.text = "Escriba las preguntas que llevaran a su respuesta.";
    }

    //Function to exit the Developer Tools Panel
    public void ExitDeveloperTools()
    {
        QuestionsManager.currentNode.question = answer;
        developersToolPanel.SetActive(false);
        StartOver();
    }
}
