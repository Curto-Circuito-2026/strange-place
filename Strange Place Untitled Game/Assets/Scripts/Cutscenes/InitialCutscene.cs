using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitialCutscene : MonoBehaviour
{
    //COLOCAR B: para fala do boss e C: para fala do personagem
    // "C: burururubu"
    ///"B: berebebebe"  
    [SerializeField] List<string> dialogueLines;

    [SerializeField] Sprite bossImage;
    [SerializeField] Sprite characterImage;

    [SerializeField] List<MovinCharCutscene> charsInCutscene;
    void Start()
    {
        StartCoroutine(StartCutscene());
    }
    IEnumerator StartCutscene()
    {
        foreach(var character in charsInCutscene)
        {
            StartCoroutine(character.playAnimation());
        }
        yield return new WaitUntil(() => {
            foreach(var character in charsInCutscene)
            {
                if(character.inAnimation) return false;
            }
            return true;
    });

        StartCoroutine(PlayConversation());
    }
    IEnumerator PlayConversation()
    {
        Sprite dialogImage = null;
        foreach (string rawLine in dialogueLines)
        {
            string lineToSay = "";
            if (rawLine.StartsWith("B:"))
            {
                //lineToSay = "<color=red>Boss:</color> " + rawLine.Substring(2).Trim();
                lineToSay = rawLine.Substring(2).Trim();
                dialogImage = bossImage;
            }
            else if (rawLine.StartsWith("C:"))
            {
                //lineToSay = "<color=blue>You:</color> " + rawLine.Substring(2).Trim();
                lineToSay = rawLine.Substring(2).Trim();
                dialogImage = characterImage;
            }
            else
            {
                lineToSay = rawLine;
            }

            yield return StartCoroutine(DialogSystem.Instance.TypeDialog(lineToSay,dialogImage));
            yield return new WaitUntil(() => 
                (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) || 
                (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            );
        }
    }
}