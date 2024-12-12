[System.Serializable]
public class TreeData
{
    public string question;
    public TreeData yes;
    public TreeData no;

    public TreeData(string _question)
    {
        this.question = _question;
        this.yes = null;
        this.no = null;
    }
}