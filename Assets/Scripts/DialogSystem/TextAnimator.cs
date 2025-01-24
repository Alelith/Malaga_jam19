using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public class TextAnimator
    {
        [SerializeField]
        bool isSizeGrowAnim;
        
        [SerializeField]
        bool isGoDownAnim;

        [SerializeField] 
        float goDownHeight;
        
        bool stopAnimating;

        TextMeshProUGUI textBox;

        public bool textAnimating;
        
        public TextMeshProUGUI TextBox { set => textBox = value; }

        public IEnumerator AnimateTextIn(List<DialogueAction> commands, string processedMessage, Action onFinish)
        {
            textAnimating = true;
            float secondsPerCharacter = 0.5f / 10f;
            float timeOfLastCharacter = 0;

            AnimationInfo[] textAnimInfo = SeparateOutTextAnimInfo(commands);
            TMP_TextInfo textInfo = textBox.textInfo;
            foreach (TMP_MeshInfo meshInfer in textInfo.meshInfo)
            {
                if (meshInfer.vertices == null) continue;
                for (int j = 0; j < meshInfer.vertices.Length; j++)
                    meshInfer.vertices[j] = Vector3.zero;
            }
            
            textBox.text = processedMessage;
            textBox.ForceMeshUpdate();

            TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
            Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
            for (int i = 0; i < originalColors.Length; i++)
            {
                Color32[] theColors = textInfo.meshInfo[i].colors32;
                originalColors[i] = new Color32[theColors.Length];
                Array.Copy(theColors, originalColors[i], theColors.Length);
            }

            int charCount = textInfo.characterCount;
            float[] charAnimStartTimes = new float[charCount];
            for (int i = 0; i < charCount; i++)
                charAnimStartTimes[i] = -1; //indicate the character as not yet started animating.
            
            int visibleCharacterIndex = 0;

            bool isAllcharsVisible = false;

            while (true)
            {
                if (stopAnimating)
                {
                    for (int i = visibleCharacterIndex; i < charCount; i++)
                        charAnimStartTimes[i] = Time.time;

                    visibleCharacterIndex = charCount;
                    FinishAnimating(onFinish);
                }

                if (ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter))
                {
                    if (visibleCharacterIndex <= charCount)
                    {
                        ExecuteCommandsForCurrentIndex(commands, visibleCharacterIndex, ref secondsPerCharacter,
                            ref timeOfLastCharacter);
                        if (visibleCharacterIndex < charCount &&
                            ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter))
                        {
                            charAnimStartTimes[visibleCharacterIndex] = Time.time;
                            visibleCharacterIndex++;
                            timeOfLastCharacter = Time.unscaledTime;
                        }
                    }
                }
                
                //Debug.Log($"Visible character #{visibleCharacterIndex} is {visibleCharacterIndex}. Is the first character visible? {isAllcharsVisible}");
                    
                if (visibleCharacterIndex == charCount && isAllcharsVisible)
                    FinishAnimating(onFinish);

                Vector3[] animPosAdjustment;

                int showedChars = 0;

                for (int j = 0; j < charCount; j++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
                    if (charInfo.isVisible) //Invisible characters have a vertexIndex of 0 because they have no vertices and so they should be ignored to avoid messing up the first character in the string whic also has a vertexIndex of 0
                    {
                        int vertexIndex = charInfo.vertexIndex;
                        int materialIndex = charInfo.materialReferenceIndex;
                        Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
                        Color32[] theColor = { 
                            j < visibleCharacterIndex
                                ? originalColors[materialIndex][vertexIndex]
                                : new(0, 0, 0, 0),
                            j < visibleCharacterIndex
                                ? originalColors[materialIndex][vertexIndex]
                                : new(0, 0, 0, 0),
                            j < visibleCharacterIndex
                                ? originalColors[materialIndex][vertexIndex]
                                : new(0, 0, 0, 0),
                            j < visibleCharacterIndex
                                ? originalColors[materialIndex][vertexIndex]
                                : new(0, 0, 0, 0)
                        };
                        
                        Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                        Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                        float charSize = 0;
                        float charAnimStartTime = charAnimStartTimes[j];
                        float statOffset = isGoDownAnim ? goDownHeight : 0;
                        if (charAnimStartTime >= 0)
                        {
                            float timeSinceAnimStart = Time.time - charAnimStartTime;
                            if (isSizeGrowAnim)
                                charSize = Mathf.Min(1, timeSinceAnimStart / 0.35f);
                            else
                                charSize = 1;
                            statOffset = Mathf.Max(0, statOffset - (timeSinceAnimStart / 0.007f));
                        }

                        animPosAdjustment = GetAnimPosAdjustment(textAnimInfo, j, textBox.fontSize, Time.time, ref theColor);
                        
                        destinationColors[vertexIndex + 0] = theColor[0];
                        destinationColors[vertexIndex + 1] = theColor[1];
                        destinationColors[vertexIndex + 2] = theColor[2];
                        destinationColors[vertexIndex + 3] = theColor[3];
                        
                        Vector3 offset = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
                        destinationVertices[vertexIndex + 0] = (sourceVertices[vertexIndex + 0] - offset) * charSize +
                                                               offset + animPosAdjustment[0] * charSize + new Vector3(statOffset / 2, statOffset, 0);
                        destinationVertices[vertexIndex + 1] = (sourceVertices[vertexIndex + 1] - offset) * charSize +
                                                               offset + animPosAdjustment[1] * charSize + new Vector3(statOffset / 2, statOffset, 0);
                        destinationVertices[vertexIndex + 2] = (sourceVertices[vertexIndex + 2] - offset) * charSize +
                                                               offset + animPosAdjustment[2] * charSize + new Vector3(statOffset / 2, statOffset, 0);
                        destinationVertices[vertexIndex + 3] = (sourceVertices[vertexIndex + 3] - offset) * charSize +
                                                               offset + animPosAdjustment[3] * charSize + new Vector3(statOffset / 2, statOffset, 0);
                        if (charSize >= 1)
                            showedChars++;
                    }
                }
                
                if (showedChars == textBox.GetParsedText().Replace(" ", "").Length)
                    isAllcharsVisible = true;

                textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    TMP_MeshInfo theInfo = textInfo.meshInfo[i];
                    theInfo.mesh.vertices = theInfo.vertices;
                    textBox.UpdateGeometry(theInfo.mesh, i);
                }

                yield return null;
            }
            
        }

        void ExecuteCommandsForCurrentIndex(List<DialogueAction> commands, int visibleCharacterIndex, ref float secondsPerCharacter, ref float timeOfLastCharacter)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                DialogueAction command = commands[i];
                if (command.Position != visibleCharacterIndex) continue;
                switch (command.DialogueType)
                {
                    case ActionType.PAUSE:
                        timeOfLastCharacter = Time.unscaledTime + command.Value;
                        break;
                    case ActionType.SPEED:
                        secondsPerCharacter /= command.Value;
                        break;
                }

                commands.RemoveAt(i);
                i--;
            }
        }

        void FinishAnimating(Action onFinish)
        {
            textAnimating = false;
            stopAnimating = false;
            onFinish?.Invoke();
        }

        Vector3[] GetAnimPosAdjustment(AnimationInfo[] textAnimInfo, int charIndex, float fontSize, float time, ref Color32[] color)
        {
            Vector3[] verticesOffsets = new Vector3[4];
            foreach (AnimationInfo info in textAnimInfo)
            {
                if (charIndex < info.Start || charIndex >= info.End) continue;
                switch (info.ThisType)
                {
                    case AnimationType.SHAKE:
                        float scaleAdjust = fontSize * 0.06f * info.C;
                        for (int j = 0; j < 4; j++)
                        {
                            verticesOffsets[j].x +=
                                (Mathf.PerlinNoise((charIndex * info.K + time) * 15 * info.Q, 0) - 0.5f) *
                                scaleAdjust;
                            verticesOffsets[j].y +=
                                (Mathf.PerlinNoise((charIndex * info.K + time) * 15 * info.Q, 1000) - 0.5f) *
                                scaleAdjust;
                        }
                        break;
                    case AnimationType.WAVE:
                        for (int j = 0; j < 4; j++)
                            verticesOffsets[j] += new Vector3(0,
                                Mathf.Sin(charIndex * 0.25f * info.K + time * 6 * info.Q) * fontSize * 0.06f *
                                info.C);
                        break;
                    case AnimationType.DANGLE:
                        verticesOffsets[0] += new Vector3(
                            Mathf.Sin(charIndex * 0.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                            0);
                        verticesOffsets[1] += Vector3.zero;
                        verticesOffsets[2] += Vector3.zero;
                        verticesOffsets[3] += new Vector3(
                            Mathf.Sin(charIndex * 0.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                            0);
                        break;
                    case AnimationType.SLIDE:
                        verticesOffsets[0] +=
                            new Vector3(
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                0);
                        verticesOffsets[1] +=
                            new Vector3(
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                0);
                        verticesOffsets[2] +=
                            new Vector3(
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                0);
                        verticesOffsets[3] +=
                            new Vector3(
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                0);
                        break;
                    case AnimationType.JUMP:
                        for (int j = 0; j < 4; j++)
                            verticesOffsets[j] += new Vector3(0, JumpFunction(time, charIndex, fontSize, info));
                        break;
                    case AnimationType.INCR:
                        float baseOffset = Mathf.Sin(charIndex * 0.25f * info.K + time * 6 * info.Q) * fontSize *
                                         0.06f * info.C;
                        
                        verticesOffsets[0] +=
                            new Vector3(
                                -baseOffset,
                                -baseOffset);
                        verticesOffsets[1] +=
                            new Vector3(
                                -baseOffset,
                                baseOffset);
                        verticesOffsets[2] +=
                            new Vector3(
                                baseOffset,
                                baseOffset);
                        verticesOffsets[3] +=
                            new Vector3(
                                baseOffset,
                                -baseOffset);
                        break;
                    case AnimationType.SWING:
                        verticesOffsets[0] +=
                            new Vector3(
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[1] +=
                            new Vector3(
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[2] +=
                            new Vector3(
                                Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[3] +=
                            new Vector3(
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                -Mathf.Sin(time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        break;
                    case AnimationType.DEFORM:
                        verticesOffsets[0] +=
                            new Vector3(
                                Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[1] +=
                            new Vector3(
                                -Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[2] +=
                            new Vector3(
                                -Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                -Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        verticesOffsets[3] +=
                            new Vector3(
                                Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C,
                                -Mathf.Sin(charIndex * 1.5f * info.K + time * 6 * info.Q) * fontSize * 0.06f * info.C);
                        break;
                    case AnimationType.COLOR:
                        color[0] = Color.Lerp(info.Target, info.Origin,
                            Mathf.Sin(time * info.Q + charIndex * 0.1f * info.K) / 2 + 0.5f);
                        color[1] = Color.Lerp(info.Target, info.Origin,
                            (Mathf.Sin(time * info.Q + charIndex * 0.2f * info.K) / 2 + 0.5f));
                        color[2] = Color.Lerp(info.Target, info.Origin,
                            Mathf.Sin(time * info.Q + charIndex * 0.1f * info.K) / 2 + 0.5f);
                        color[3] = Color.Lerp(info.Target, info.Origin,
                            (Mathf.Sin(time * info.Q + charIndex * 0.2f * info.K) / 2 + 0.5f));
                        break;
                }
            }

            return verticesOffsets;
        }
        
        static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) => Time.unscaledTime - timeOfLastCharacter > secondsPerCharacter;

        public void SkipToEndOfCurrentMessage()
        {
            if (textAnimating)
                stopAnimating = true;
        }
        
        AnimationInfo[] SeparateOutTextAnimInfo(List<DialogueAction> commands)
        {
            Dictionary<DialogueAction, DialogueAction> startEndPairs = new();
            
            Stack<DialogueAction> startCommands = new();
            
            for (int i = 0; i < commands.Count; i++)
            {
                DialogueAction command = commands[i];
                if (command.DialogueType == ActionType.DIALOGANIMATION)
                {
                    startCommands.Push(command);
                    commands.RemoveAt(i);
                    i--;
                }
                else if (command.DialogueType == ActionType.ENDANIMATION)
                {
                    DialogueAction startCommand = startCommands.Pop();
                    startEndPairs.Add(startCommand, command);
                    commands.RemoveAt(i);
                    i--;
                }
            }

            List<AnimationInfo> tempResult = new();
            
            foreach (var startEndPair in startEndPairs)
            {
                DialogueAction startCommand = startEndPair.Key;
                DialogueAction endCommand = startEndPair.Value;
                tempResult.Add(new AnimationInfo
                {
                    Start = startCommand.Position,
                    End = endCommand.Position,
                    ThisType = startCommand.AnimType,
                    K = startCommand.K,
                    C = startCommand.C,
                    Q = startCommand.Q,
                    Origin = startCommand.Origin,
                    Target = startCommand.Target
                });
            }
            
            return tempResult.ToArray();
        }

        public static float JumpFunction(float time, int charIndex, float fontSize, AnimationInfo info)
        {
            float ajustment = 2f * info.K;

            float scaledTime = Mathf.Abs((time - charIndex/(24f * info.Q)) % ajustment);
            
            if (scaledTime < (0.5f * ajustment) && scaledTime >= 0)
                return fontSize/1.5f * Mathf.Sin((2 * Mathf.PI / ajustment) * scaledTime - 2 * Mathf.PI) * info.C;
            
            if (scaledTime < (0.75f * ajustment) && scaledTime >= (0.5f * ajustment))
                return fontSize / 4 * Mathf.Sin(((4 * Mathf.PI) / ajustment) * scaledTime - 4 * Mathf.PI) * info.C;
            
            if (scaledTime < (0.875f * ajustment) && scaledTime >= (0.75f * ajustment))
                return fontSize / 16 * Mathf.Sin(((8 * Mathf.PI) / ajustment) * scaledTime - 8 * Mathf.PI) * info.C;
            
            if (scaledTime < (0.9375f * ajustment) && scaledTime >= (0.875f * ajustment))
                return fontSize / 64 * Mathf.Sin(((16 * Mathf.PI) / ajustment) * scaledTime - 16 * Mathf.PI) * info.C;
            
            return 0;
        }
    }
}