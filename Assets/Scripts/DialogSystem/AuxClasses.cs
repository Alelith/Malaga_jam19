using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Static class that converts custom tags into information
    /// </summary>
    public static class CustomTagsManager
    {
        #region Dictionaries
        /// <summary>
		/// Dictionary that maps custom tag regex patterns to their corresponding ActionType.
		/// </summary>
        static readonly Dictionary<Regex, ActionType> customTags = new ()
        {
            { new Regex(@"<s:(?<attribute>(\w*|\d*(?:\,\d+)?))>"), ActionType.SPEED },
            { new Regex(@"<p:(?<attribute>(\w*|\d*(?:\,\d+)?))>"), ActionType.PAUSE },
            { new Regex(@"<a:(?<attribute>\w*)( k=(?<k>(\d*(?:\,\d+)?)))?( q=(?<q>(\d*(?:\,\d+)?)))?( c=(?<c>(\d*(?:\,\d+)?)))?( origin=(?<origin>(#([0-9A-Fa-f][0-9A-Fa-f]){3})) target=(?<target>(#([0-9A-Fa-f][0-9A-Fa-f]){3})))?>"), ActionType.DIALOGANIMATION },
            { new Regex("</a>"), ActionType.ENDANIMATION }
        };

        /// <summary>
        /// Dictionary that relates predefined values that it can be used on speed tag and its value in float
        /// </summary>
        static readonly Dictionary<string, float> speedMultiplier = new ()
        {
            { "superslow", 0.25f },
            { "half", 0.5f },
            { "slow", 0.75f },
            { "somefaster", 1.25f },
            { "faster", 1.5f },
            { "flash", 1.75f },
            { "double", 2f },
        };

        /// <summary>
        /// Dictionary that relates predefined values that it can be used on pause tag and its value in float
        /// </summary>
        static readonly Dictionary<string, float> pauseTime = new ()
        {
            { "tiny", 0.25f },
            { "short", 0.5f },
            { "normal", 0.75f },
            { "long", 1f },
            { "extralong", 1.5f }
        };
        #endregion

        /// <summary>
        /// Iterates all the custom tags to translate them into code info
        /// </summary>
        /// <param name="message">Original dialog line</param>
        /// <param name="clearMessage">Output with the dialog line without the custom tags</param>
        /// <returns>All the custom properties info</returns>
        public static List<DialogueAction> ProccessMessage(string message, out string clearMessage)
        {
            // Temp list that stores all the custom tags actions info 
            List<DialogueAction> foundActions = new();

            clearMessage = new string(message);
            
            // Iterates all the allowed custom tags
            foreach (KeyValuePair<Regex, ActionType> customTag in customTags)
            {
                // Find all tags of a type in the current line
                foreach (Match match in customTag.Key.Matches(message))
                {
                    // Initializes temp variables to parse the attribute
                    float attributeFloat = 0;
                    float attributeK = 1;
                    float attributeQ = 1;
                    float attributeC = 1;
                    Color origin = Color.white;
                    Color target = Color.black;
                    AnimationType attributeAction = AnimationType.NULL;
                    string attribute = "";
                    Vector3 position = new();

                    // If the type of the current tag is not an end tag parse the attributes
                    if (customTag.Value != ActionType.ENDANIMATION)
                    {
                        // Gets the attribute of the tag
                        attribute = match.Groups["attribute"].Value;

                        switch (customTag.Value)
                        {
                            // If the current tag type is animation parse the attribute to AnimationType
                            case ActionType.DIALOGANIMATION:
                                attributeAction = (AnimationType)Enum.Parse(typeof(AnimationType), attribute.ToUpper());
                                if (match.Groups["k"].Success)
                                    attributeK = float.Parse(match.Groups["k"].Value);
                                if (match.Groups["q"].Success)
                                    attributeQ = float.Parse(match.Groups["q"].Value);
                                if (match.Groups["c"].Success)
                                    attributeC = float.Parse(match.Groups["c"].Value);
                                if (match.Groups["origin"].Success)
                                    if (ColorUtility.TryParseHtmlString(match.Groups["origin"].Value, out Color tempColor))
                                        origin = tempColor;
                                if (match.Groups["target"].Success)
                                    if (ColorUtility.TryParseHtmlString(match.Groups["target"].Value, out Color tempColor))
                                        target = tempColor;
                                break;
                            case ActionType.SPEED or ActionType.PAUSE when float.TryParse(attribute, out float tempAttributeFloat):
                                attributeFloat = tempAttributeFloat;
                                break;
                            case ActionType.SPEED:
                                attributeFloat = speedMultiplier[attribute.ToLower()];
                                break;
                            case ActionType.PAUSE:
                                attributeFloat = pauseTime[attribute.ToLower()];
                                break;
                        }
                    }
                    // Add the found custom tag to the temp list
                    foundActions.Add(new DialogueAction
                    {
                        Position = CharactersUpToIndex(message, match.Index),
                        DialogueType = customTag.Value,
                        Value = attributeFloat,
                        AnimType = attributeAction,
                        K = attributeK,
                        Q = attributeQ,
                        C = attributeC,
                        Origin = origin,
                        Target = target,
                        TransPosition = position 
                    });

                    
                    clearMessage = clearMessage.Replace(match.Value, "");
                }
            }
            // Return the temp list with all the custom tags info
            return new List<DialogueAction>(from value in foundActions orderby value.Position select value);
        }
        
        /// <summary>
		/// Returns the number of characters before the tag position, ignoring characters inside brackets.
		/// </summary>
        /// <param name="message">Line of dialog where the tag is in</param>
        /// <param name="index">Index where the tag is placed on the string</param>
        /// <returns>The amount of characters before the tagged one</returns>
        static int CharactersUpToIndex(string message, int index)
        {
            int result = index;
            bool insideBrackets = false;
            for (int i = index - 1; 0 <= i; i--)
            {
                if (message[i] == '>')
                {
                    insideBrackets = true;
                    result--;
                }
                else if (message[i] == '<')
                {
                    insideBrackets = false;
                    result--;
                }
                else if (insideBrackets)
                    result--;
            }
            
            return result;
        }
    }

    #region Structs
    /// <summary>
    /// Struct that contains the info of a custom tag placed in a string
    /// </summary>
    public struct DialogueAction
    {
        int position;
        ActionType type;
        float value;
        string animationName;
        string animationActor;
        AnimationType animationType;
        float k, q, c;
        Color origin, target;
        Vector3 transform;

        public int Position { get => position; set => position = value; }

        public ActionType DialogueType { get => type; set => type = value; }

        public float Value { get => value; set => this.value = value; }
        
        public AnimationType AnimType { get => animationType; set => animationType = value; }
        
        public float K { get => k; set => k = value; }

        public float Q { get => q; set => q = value; }

        public float C { get => c; set => c = value; }

        public Color Origin { get => origin; set => origin = value; }

        public Color Target { get => target; set => target = value; }

        public Vector3 TransPosition { get => transform; set => transform = value; }

        public override bool Equals(object obj)
        {
            return obj is DialogueAction other && Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(position);
            hashCode.Add((int)type);
            hashCode.Add(value);
            hashCode.Add(animationName);
            hashCode.Add(animationActor);
            hashCode.Add((int)animationType);
            hashCode.Add(k);
            hashCode.Add(q);
            hashCode.Add(c);
            hashCode.Add(origin);
            hashCode.Add(target);
            hashCode.Add(transform);
            return hashCode.ToHashCode();
        }
    }

    /// <summary>
    /// Struct that contains the info of an animated text between animation tags
    /// </summary>
    public struct AnimationInfo
    {
        int start;
        int end;
        AnimationType type;
        float k, q, c;
        Color origin, target;

        public int Start { get => start; set => start = value; }

        public int End { get => end; set => end = value; }

        public AnimationType ThisType { get => type; set => type = value; }

        public float K { get => k; set => k = value; }

        public float Q { get => q; set => q = value; }

        public float C { get => c; set => c = value; }

        public Color Origin { get => origin; set => origin = value; }

        public Color Target { get => target; set => target = value; }
    }
    #endregion

    #region Enums
    /// <summary>
    /// Type of custom tag
    /// </summary>
    public enum ActionType
    {
        SPEED,
        DIALOGANIMATION,
        ENDANIMATION,
        PAUSE
    }

    /// <summary>
    /// Type of animation
    /// </summary>
    public enum AnimationType
    {
        SHAKE,
        WAVE,
        INCR,
        SWING,
        DANGLE,
        JUMP,
        DEFORM,
        SLIDE,
        COLOR,
        NULL
    }
    #endregion
}