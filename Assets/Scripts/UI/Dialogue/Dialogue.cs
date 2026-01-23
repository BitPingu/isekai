
[System.Serializable]
public class Dialogue
{
    public string sentence;
    public string[] options;

    public Dialogue(string s)
    {
        sentence = s;
        options = new string[0];
    }
    public Dialogue(string s, string[] o)
    {
        sentence = s;
        options = o;
    }

}
