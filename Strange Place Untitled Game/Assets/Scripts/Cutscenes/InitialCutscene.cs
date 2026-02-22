using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InitialCutscene : MonoBehaviour
{
    //COLOCAR B: para fala do boss e C: para fala do personagem
    // "C: burururubu"
    ///"B: berebebebe"  
    [SerializeField] List<string> dialogueLines;

    [SerializeField] GameObject phaseSelector;
    [SerializeField] GameObject imageBackground;
    [SerializeField] Sprite bossImage;
    [SerializeField] Sprite characterImage;
    [SerializeField] Sprite openDoorSprite;
    [SerializeField] Sprite closedDoorSprite;
    [SerializeField] float timeUntilStart = 0.2f;


    [SerializeField] List<MovinCharCutscene> charsInCutscene;
    void Start()
    {
        StartCoroutine(StartCutscene());
    }
    IEnumerator StartCutscene()
    {

        yield return new WaitForSeconds(timeUntilStart);
        imageBackground.GetComponent<Image>().sprite = openDoorSprite;
        yield return new WaitForSeconds(timeUntilStart);
        yield return MoveCharacters();

        yield return PlayConversation();
        InvertCharacters();
        yield return MoveCharacters();
        imageBackground.GetComponent<Image>().sprite = closedDoorSprite;
        yield return new WaitForSeconds(0.2f);
        phaseSelector.SetActive(true);
    }

    private void InvertCharacters()
    {
        foreach(var character in charsInCutscene)
        {
            character.ChangeSprite(false);
            character.InvertDirection();
        }
    }

    IEnumerator MoveCharacters()
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

        DialogSystem.Instance.SetActive(false);
    }
}