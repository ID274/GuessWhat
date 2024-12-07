using System.Collections.Generic;
using UnityEngine;

public enum WordType
{
    Invalid,
    Noun,
    Verb,
    Adverb,
    Adjective
}

public class WordPicker : MonoBehaviour
{
    [SerializeField] private string chosenWord;
    [SerializeField] private WordType wordType;

    public List<Word> words = new List<Word>();
    public List<Word> generatedWords = new List<Word>();

    [Header("Attributes")]
    [SerializeField] private int wordsToGenerate = 20;


    private void Start()
    {
        GenerateWords(wordsToGenerate);
        ChooseWord();
    }

    public void GenerateWords(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Word word = words[Random.Range(0, words.Count)];
            if (!generatedWords.Contains(word))
            {
                generatedWords.Add(word);
            }
            else if (generatedWords.Count < words.Count)
            {
                i--;
            }
            else
            {
                break;
            }
        }
    }

    private void ChooseWord()
    {
        Word word = generatedWords[Random.Range(0, generatedWords.Count)];
        chosenWord = word.name;
        wordType = word.wordType;
    }

    public (string, WordType) ReturnChosenWord()
    {
        return (chosenWord, wordType);
    }
}