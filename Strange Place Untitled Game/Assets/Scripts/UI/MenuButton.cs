using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject toDisable;
    [SerializeField] GameObject toEnable;

    Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();
    }
    void Start()
    {
        myButton.onClick.AddListener(()=>
        {
           toDisable.SetActive(false);
           toEnable.SetActive(true); 
        });
    }
}
