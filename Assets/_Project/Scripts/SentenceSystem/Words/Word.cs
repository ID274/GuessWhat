using UnityEngine;

[CreateAssetMenu(fileName = "New Word", menuName = "Word")]
public class Word : ScriptableObject
{
    // string not needed as we will use the name of the scriptable object
    public WordType wordType;
}
