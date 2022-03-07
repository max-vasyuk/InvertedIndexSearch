namespace InvertedIndexSearch;

public class Node
{
    public Node leftNode { get; set; }
    public Node rightNode { get; set; }
    public string data { get; set; }

    public Node(string data)
    {
        this.data = data;
        this.leftNode = this.rightNode = null;
    }

    public Node(string data, Node leftNode, Node rightNode)
    {
        this.data = data;
        this.leftNode = leftNode;
        this.rightNode = rightNode;
    }

    public void ViewNode()
    {
        Console.WriteLine(data);
        Console.WriteLine("L:{0}\tR:{1}", leftNode.data, rightNode.data);
    }
}