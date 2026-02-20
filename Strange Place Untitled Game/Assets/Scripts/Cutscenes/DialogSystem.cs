using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogSystem : MonoBehaviour
{
    [SerializeField] GameObject dialogObject;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] int letterPerSecond;

    [SerializeField] AudioSource voiceSound;

    [SerializeField] Image speakerImage;

    public bool talking;

    bool isDialogActive;

    public static DialogSystem Instance{get;private set;}
    void Awake()
    {
        Instance=this;
       
    }
    void SetActive(bool state)
    {
        isDialogActive = state;
        dialogObject.SetActive(isDialogActive);
    }

    void SetImage(Sprite image)
    {
        speakerImage.sprite = image;
    }
    public IEnumerator TypeDialog(string dialog, Sprite image)
    {
        SetImage(image);
        if(!isDialogActive) SetActive(true);
        
        talking =true;
        //voiceSound.Play();
        text.text="";
        bool isSpecialChar=false;
        foreach(var letter in dialog.ToCharArray()){
            if(letter=='>') isSpecialChar=false;
            if(letter=='<' || isSpecialChar){
                text.text+=letter;
                isSpecialChar=true;
            }
            if(!isSpecialChar){
                text.text+=letter;
                yield return new WaitForSeconds(1f/letterPerSecond);
            }
        }
        //voiceSound.Pause();
        talking =false;
    } 
    
}
