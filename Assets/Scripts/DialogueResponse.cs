using UnityEngine;

[CreateAssetMenu]
public class DialogueResponse : ScriptableObject
{
    public string text;
    public AudioClip audioClip;
    public bool rude;
    public bool excuse;
    public Code CodeType;
}