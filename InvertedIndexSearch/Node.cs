using System.Runtime.InteropServices;

namespace InvertedIndexSearch;

public class Node
{
    public Node parent { get; set; }
    public Node leftNode { get; set; }
    public Node rightNode { get; set; }
    public string data { get; set; }

    public Node(string data)
    {
        this.data = data;
        leftNode = rightNode = parent = null;
    }

    public Node(string data, Node leftNode, Node rightNode)
    {
        this.data = data;
        this.leftNode = leftNode;
        this.rightNode = rightNode;
        parent = null;
        leftNode.parent = this;
        rightNode.parent = this;
    }

    public void ViewNode()
    {
        Console.WriteLine(data);
        var tempParent = (parent is null) ? "null" : parent.data;
        var tempLeftNode = (leftNode is null) ? "null" : leftNode.data;
        var tempRightNode = (rightNode is null) ? "null" : rightNode.data;
        Console.WriteLine("P:{0}\tL:{1}\tR:{2}\n", tempParent, tempLeftNode, tempRightNode);
    }

}