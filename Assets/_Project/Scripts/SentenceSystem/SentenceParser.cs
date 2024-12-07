using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SentenceParser : MonoBehaviour
{
    private SentenceType sentenceType;
    public (bool, SentenceType) CheckSentence(Stack<GameObject> stack, GameObject sentencePrefab)
    {
        sentenceType = SentenceType.Invalid;
        GameObject[] sentenceParts = stack.Reverse().ToArray();
        
        if (sentenceParts.Length < 3)
        {
            if (sentenceParts[1].name.Contains("Value"))
            {
                sentenceType = SentenceType.PrefixValue;
            }
            else if (sentenceParts[1].name.Contains("Descriptor"))
            {
                sentenceType = SentenceType.PrefixDescriptor;
            }
            else
            {
                sentenceType = SentenceType.Invalid;
            }
        }
        else
        {
            if (sentenceParts[2].name.Contains("Descriptor"))
            {
                sentenceType = SentenceType.PrefixValueDescriptor;
            }
            else
            {
                sentenceType = SentenceType.Invalid;
            }
        }

        if (sentenceType == SentenceType.Invalid)
        {
            Debug.LogWarning("Invalid Sentence Type");
            return (false, SentenceType.Invalid);
        }
        
        
        // Verify Each Rule
        if (sentenceType == SentenceType.PrefixValue)
        {
            return (VerifyPrefixValue(sentenceParts), sentenceType);
        }

        if (sentenceType == SentenceType.PrefixDescriptor)
        {
            return (VerifyPrefixDescriptor(sentenceParts), sentenceType);
        }

        if (sentenceType == SentenceType.PrefixValueDescriptor)
        {
            return (VerifyPrefixValueDescriptor(sentenceParts), sentenceType);
        }

        return (false, SentenceType.Invalid); // Something went wrong so we return false
    }

    private bool VerifyPrefixValue(GameObject[] sentenceParts)
    {
        TMP_Dropdown dropdown = sentenceParts[0].GetComponent<TMP_Dropdown>();
        string dropdownText = dropdown.options[dropdown.value].text;

        if (dropdownText == "Is it")
        {
            return false; // PrefixValue requires "It has"
        }
        else if (dropdownText == "It has")
        {
            TMP_InputField valueField = sentenceParts[1].GetComponent<TMP_InputField>();

            if (int.TryParse(valueField.text, out int value))
            {
                return false; // "It has number" doesn't make sense in this case
            }
            else
            {
                Regex rg = new Regex(@"^[a-zA-Z\s,]*$");
                if (rg.IsMatch(valueField.text))
                {
                    return true;
                }
                else
                {
                    return false; // "It has invalid string" doesn't make sense in this case
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid Prefix");
            return false;
        }
    }

    private bool VerifyPrefixDescriptor(GameObject[] sentenceParts)
    {
        TMP_Dropdown dropdown = sentenceParts[0].GetComponent<TMP_Dropdown>();
        string dropdownText = dropdown.options[dropdown.value].text;

        if (dropdownText == "Is it")
        {
            TMP_Dropdown dropdown1 = sentenceParts[1].GetComponent<TMP_Dropdown>();
            string dropdownText1 = dropdown1.options[dropdown1.value].text;

            if (dropdownText1 == "Noun" || dropdownText1 == "Adjective" || dropdownText1 == "Verb" || dropdownText1 == "Adverb")
            {
                return true;
            }
            else
            {
                return false; // PrefixDescriptor requires a type of word
            }
        }
        else if (dropdownText == "It has")
        {
            return false; // PrefixDescriptor requires "It is"
        }
        else
        {
            Debug.LogWarning("Invalid Prefix");
            return false;
        }
    }

    private bool VerifyPrefixValueDescriptor(GameObject[] sentenceParts)
    {
        TMP_Dropdown dropdown = sentenceParts[0].GetComponent<TMP_Dropdown>();
        string dropdownText = dropdown.options[dropdown.value].text;

        if (dropdownText == "Is it")
        {
            TMP_InputField valueField = sentenceParts[1].GetComponent<TMP_InputField>();

            if (int.TryParse(valueField.text, out int value))
            {
                TMP_Dropdown dropdown1 = sentenceParts[2].GetComponent<TMP_Dropdown>();
                string dropdownText1 = dropdown1.options[dropdown1.value].text;

                if (dropdownText1 == "Letters Long")
                {
                    return true;
                }
                else
                {
                    return false; // We don't want to check if it is a noun or what not as it is preceded by a number
                }
            }
            else
            {
                return false; // PrefixValueDescriptor requires a number value
            }
        }
        else if (dropdownText == "It has")
        {
            return false; // PrefixValueDescriptor requires "It is"
        }
        else
        {
            Debug.LogWarning("Invalid Prefix");
            return false;
        }
    }
}
