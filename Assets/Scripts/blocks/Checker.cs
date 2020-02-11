using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checker : MonoBehaviour
{

    private Stack<Block> blockStack;
    public string currentName;

    // Start is called before the first frame update
    void Start()
    {
        blockStack = new Stack<Block>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBlock(Block b)
    {
        blockStack.Push(b);
    }

    public void removeLast()
    {
        blockStack.Pop();
    }

    public void removeAll()
    {
        while (blockStack.Count != 0)
        {
            removeLast();
        }
    }

    public void checkLastBlock()
    {
        string blockText = blockStack.Peek().textGuide.GetComponentInChildren<Text>().text;
        if (blockText != currentName)
        {
            print("EFE");
        }
        else
        {
            print("Todo ok");
        }
    }
}
