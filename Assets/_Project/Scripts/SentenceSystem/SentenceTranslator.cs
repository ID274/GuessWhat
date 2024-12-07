using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SentenceTranslator : MonoBehaviour
{
    public WordPicker wordPicker;

    private SentenceType sentenceType;

    public bool TranslateSentence(Stack<GameObject> stack, SentenceType sentenceType)
    {
        GameObject[] sentenceParts = stack.Reverse().ToArray();

        // Translate Each Rule
        if (sentenceType == SentenceType.PrefixValue)
        {
            return TranslatePrefixValue(sentenceParts);
        }

        if (sentenceType == SentenceType.PrefixDescriptor)
        {
            return TranslatePrefixDescriptor(sentenceParts);
        }

        if (sentenceType == SentenceType.PrefixValueDescriptor)
        {
            return TranslatePrefixValueDescriptor(sentenceParts);
        }

        Debug.LogError("Something went wrong");
        return false;
    }



    private bool TranslatePrefixValue(GameObject[] sentenceParts) // Question Example: Does the word contain this string of letters?
    {
        string value = sentenceParts[1].GetComponent<TMP_InputField>().text.ToUpper();

        if (wordPicker.ReturnChosenWord().Item1.ToUpper().Contains(value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TranslatePrefixDescriptor(GameObject[] sentenceParts) // Question Example: Is the word a Noun?
    {
        string value = sentenceParts[1].GetComponent<TMP_Dropdown>().options[sentenceParts[1].GetComponent<TMP_Dropdown>().value].text;
        WordType wordType = wordPicker.ReturnChosenWord().Item2;

        if (wordType == WordType.Noun && value == "Noun")
        {
            return true;
        }
        if (wordType == WordType.Verb && value == "Verb")
        {
            return true;
        }
        if (wordType == WordType.Adverb && value == "Adverb")
        {
            return true;
        }
        if (wordType == WordType.Adjective && value == "Adjective")
        {
            return true;
        }
        return false;
    }

    private bool TranslatePrefixValueDescriptor(GameObject[] sentenceParts) // Question Example: Is the word 5 letters long?
    {
        int value = int.Parse(sentenceParts[1].GetComponent<TMP_InputField>().text);
        int wordLength = wordPicker.ReturnChosenWord().Item1.Length;

        if (value == wordLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
