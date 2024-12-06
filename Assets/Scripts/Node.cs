using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //Variables for the Base Node
    public string question {get; set;}
    public Node yes {get; set;}
    public Node no {get; set;}
    public int Fe {get; set;}

    public Node (string _question, int _Fe)
    {
        question = _question;
        Fe = _Fe;
    }
}
