using System;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Localization;

namespace DialogSystem
{
    /// <summary>
    /// Class that manages the text typewrite and animation
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Typewriter : MonoBehaviour
    {
        /// <summary>
        /// The dialog that contains the dialog line
        /// </summary>
        TextMeshProUGUI dialog;

        [SerializeField]
        [Tooltip("The text animator that will typewrite the text")]
        TextAnimator textAnim;
        
        [SerializeField]
        LocalizedString textToTypewrite;
        
        /// <summary>
        /// The coroutine that will animate the text
        /// </summary>
        Coroutine typeRoutine;

        /// <summary>
        /// Event that will be called when the line is shown
        /// </summary>
        public event Action OnLineShown;

        /// <summary>
        /// If the sentence is being shown
        /// </summary>
        public bool IsSentenceShown => !textAnim.textAnimating;
        
        void Awake()
        {
            // Initializes the dialog
            dialog = GetComponent<TextMeshProUGUI>();
            // Clear its text
            dialog.text = "";
            // Create a text animator for this typewritter
            //textAnim = new TextAnimator(dialog);
            textAnim.TextBox = dialog;
        }

        void Start()
        {
            if (textToTypewrite.IsEmpty) return;
            List<DialogueAction> commands =
                CustomTagsManager.ProccessMessage(textToTypewrite.GetLocalizedString(), out string totalTextMessage);
            Write(commands, totalTextMessage);
        }

        /// <summary>
        /// Writes a line of dialog letter by letter
        /// </summary>
        /// <param name="sentence">Line of dialog that will be shown</param>
        public void Write(List<DialogueAction> commands, string clearMessage) 
        {
            // If the coroutine is not null, stop it
            if (typeRoutine != null)
            {
                StopCoroutine(typeRoutine);
                typeRoutine = null;
            }
            // Set the sentence to the current sentence
            textAnim.textAnimating = false;

            // Start the coroutine to animate the text
            typeRoutine = StartCoroutine(textAnim.AnimateTextIn(commands, clearMessage, OnLineShown));
        }

        /// <summary>
        /// Skips the current message
        /// </summary>
        public void Skip()
        {
            textAnim.SkipToEndOfCurrentMessage();
        }

        /// <summary>
        /// Clears the dialog
        /// </summary>
        public void Clear() => dialog.text = "";
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Typewriter))]
    public class TypewriterEditor : Editor
    {
        string text;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Typewriter typewriter = (Typewriter)target;

            text = EditorGUILayout.TextArea(text, GUILayout.Height(50));

            if (GUILayout.Button("Write"))
            {
                List<DialogueAction> commands =
                        CustomTagsManager.ProccessMessage(text, out string totalTextMessage);

                typewriter.Write(commands, totalTextMessage);
            }
            if (GUILayout.Button("Clear"))
                typewriter.Clear();
        }
    }
#endif
}
