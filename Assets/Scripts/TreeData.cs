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

        //Serialize recursive constructor to save data
        public NodeData(Node node)
        {
            if (node == null) return;

            this.question = node.question;
            this.Fe = node.Fe;
            this.yes = node.yes != null ? new NodeData(node.yes) : null;
            this.no = node.no != null ? new NodeData(node.no) : null;
        }
    }
}