using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using PatternLibrary;

public class SentenceUIManager : Singleton<SentenceUIManager>
{
    [Header("Attributes")]
    [SerializeField] private int maxNodes = 3;
    [SerializeField] private int maxValueCharacters = 2;
    private int nodeCount = 0;

    [Header("References")]
    [SerializeField] private SentenceParser sentenceParser; // Confirms if the sentence is valid
    [SerializeField] private SentenceTranslator sentenceTranslator; // Translates the sentence into a programmatic question to ask the computer
    [SerializeField] private GameObject sentencePanel;
    [SerializeField] private GameObject nodePanel;
    [SerializeField] private TMP_Dropdown nodeDropdown;
    [SerializeField] private GameObject sentenceHolder;
    [SerializeField] private TMP_Text sentencePreviewText;

    [Header("Sentence Prefabs")]
    [SerializeField] private GameObject sentencePrefixValue;
    [SerializeField] private GameObject sentencePrefixDescriptor;
    [SerializeField] private GameObject sentencePrefixValueDescriptor;

    [Header("Sentence Parts")]
    [SerializeField] private GameObject sentencePrefix;
    [SerializeField] private GameObject sentenceValue;
    [SerializeField] private GameObject sentenceDescriptor;
    private GameObject lastSentence;

    [Header("Node Stack")]
    private Stack<GameObject> nodeStack = new Stack<GameObject>();

    // Sentence Creation
    public void OpenSentenceMenu()
    {
        sentencePanel.SetActive(true);
    }

    public void CloseSentenceMenu()
    {
        sentencePanel.SetActive(false);
    }

    public void UpdateSentence()
    {
        string text = "";

        GameObject[] nodesArray = nodeStack.ToArray();
        for (int i = nodesArray.Length - 1; i >= 0; i--)
        {
            GameObject node = nodesArray[i];
            if (node.GetComponent<TMP_InputField>())
            {
                TMP_InputField inputField = node.GetComponent<TMP_InputField>();
                if (inputField.text == "")
                {
                    continue;
                }
                if (int.TryParse(inputField.text, out int value))
                {
                    value = Mathf.Clamp(value, 0, 99);
                    text += " " + value;
                }
                else
                {
                    text += " " + inputField.text;
                }
            }
            else if (node.GetComponent<TMP_Dropdown>())
            {
                TMP_Dropdown dropdown = node.GetComponent<TMP_Dropdown>();
                text += " " + dropdown.options[dropdown.value].text;
            }
            else
            {
                Debug.LogError("Node is wrong type");
            }
        }

        if (sentencePreviewText.text.Contains("It is"))
        {
            sentencePreviewText.text = text;
        }
        else
        {
            sentencePreviewText.text = text + "?";
        }
    }

    public void AskQuestion()
    {
        (bool, SentenceType) valid = sentenceParser.CheckSentence(nodeStack, lastSentence);
        if (valid.Item1)
        {
            Debug.Log("Valid Sentence");
            bool answer = sentenceTranslator.TranslateSentence(nodeStack, valid.Item2);

            Debug.Log($"Answer: {answer}");
        }
        else
        {
            Debug.Log("Invalid Sentence");
        }
    }

    // Node Creation
    public void OpenNodeMenu()
    {
        nodePanel.SetActive(true);
    }

    public void CloseNodeMenu()
    {
        nodePanel.SetActive(false);
    }

    public void CreateNode()
    {
        GameObject typeOfSentence = null;
        switch (nodeDropdown.value)
        {
            case 0:
                typeOfSentence = sentencePrefixValue;
                break;
            case 1:
                typeOfSentence = sentencePrefixDescriptor;
                break;
            case 2:
                typeOfSentence = sentencePrefixValueDescriptor;
                break;
        }

        GameObject[] nodes = typeOfSentence.GetComponentsInChildren<Node>().Select(t => t.gameObject).ToArray();

        if (nodeCount >= maxNodes)
        {
            Debug.Log($"Max nodes reached: {nodeCount}/{maxNodes}");
            return;
        }
        else if (nodes.Length < 2 || nodeCount + nodes.Length > maxNodes)
        {
            Debug.Log($"Nodes array less than 2 ({nodes.Length}) OR {nodeCount + nodes.Length} is higher than {maxNodes}");
            return;
        }
        else
        {
            nodeCount += nodes.Length;
        }

        foreach (GameObject node in nodes)
        {
            GameObject typeOfNode = null;

            if (node.name.Contains("Prefix"))
            {
                typeOfNode = sentencePrefix;
            }
            else if (node.name.Contains("Value"))
            {
                typeOfNode = sentenceValue;
            }
            else if (node.name.Contains("Descriptor"))
            {
                typeOfNode = sentenceDescriptor;
            }

            GameObject newNode = Instantiate(typeOfNode, sentenceHolder.transform);

            if (newNode.TryGetComponent<TMP_InputField>(out var inputField))
            {
                inputField.characterLimit = maxValueCharacters;
                inputField.onValueChanged.AddListener(delegate { UpdateSentence(); }); // I get what this does but I don't understand the magic syntax
            }
            else if (newNode.TryGetComponent<TMP_Dropdown>(out var dropDown))
            {
                dropDown.onValueChanged.AddListener(delegate { UpdateSentence(); });
            }
            else
            {
                Debug.LogError("Incorrect node type");
                return;
            }

            nodeStack.Push(newNode);
        }
        lastSentence = typeOfSentence;
    }

    public void RemoveLastNode()
    {
        if (nodeCount <= 0)
        {
            return;
        }
        else
        {
            nodeCount--;
        }
        GameObject nodeToRemove = nodeStack.Pop();
        Destroy(nodeToRemove);

        UpdateSentence();
    }

    public void RemoveLastSentence()
    {
        while (nodeCount > 0)
        {
            RemoveLastNode();
        }
    }
}