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
        if (!(leftNode is null))
        {
            this.leftNode = leftNode;
            leftNode.parent = this;
        }
        
        if (!(rightNode is null))
        {
            this.rightNode = rightNode;
            rightNode.parent = this;
        }
        parent = null;
    }

    public int ChildCount()
    {
        return ((leftNode is null) ? 0 : 1) + ((rightNode is null) ? 0 : 1);
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