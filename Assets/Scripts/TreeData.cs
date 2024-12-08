[System.Serializable]
public class TreeData
{
    public NodeData root;

    [System.Serializable]
    public class NodeData
    {
        public string question;
        public int Fe;
        public NodeData yes;
        public NodeData no;

        public NodeData(Node _node)
        {
            question = _node.question;
            Fe = _node.Fe;
            yes = _node.yes != null ? new NodeData(_node.yes) : null;
            no = _node.no != null ? new NodeData(_node.no) : null;
        }
    }
}
