using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    public Node Root {get; set;}

    public Tree(string _initalRoot)
    {
        Root = new Node(_initalRoot);
    }
}
