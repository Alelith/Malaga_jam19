using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] string[] dialog;
    
    public string this[int i] => dialog[i]; 
    
    public int Count => dialog.Length;
}
