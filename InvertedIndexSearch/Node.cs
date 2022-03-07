namespace InvertedIndexSearch;

public class Node
{
    public Node leftNode { get; set; }
    public Node rightNode { get; set; }
    public string data { get; set; }

    public void ViewNode()
    {
        Console.WriteLine(data);
        Console.WriteLine("L:{0}\tR:{1}", leftNode.data, rightNode.data);
    }
}