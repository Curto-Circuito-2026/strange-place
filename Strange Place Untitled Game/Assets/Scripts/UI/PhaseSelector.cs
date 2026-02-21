using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhaseSelector : MonoBehaviour
{
    [SerializeField] float timeUntilStart;
    [SerializeField] Sprite phasePrintScreen;
    Image phaseImageComponent;

    [SerializeField] Sprite loadingImage;
    [SerializeField] Image loadingImageComponent;
    [SerializeField] string phaseName;
    [SerializeField] float expandSpeed = 4f;
    [SerializeField] float timeUntilStatrt = 2f;

    [SerializeField] GameObject loadingScene;

    private void Awake() 
    {
        phaseImageComponent = GetComponent<Image>();
    }
    void Start()
    {

        phaseImageComponent.sprite = phasePrintScreen;
        
        loadingImageComponent.sprite = loadingImage;
        loadingImageComponent.rectTransform.sizeDelta = Vector2.zero;


        loadingImageComponent.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        loadingImageComponent.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        loadingImageComponent.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void OnPhaseClick()
    {
        Debug.Log("clicou");
        StopAllCoroutines();
        StartCoroutine(ExpandLoadingEffect());
        
    }

    IEnumerator ExpandLoadingEffect()
    {
        transform.SetAsLastSibling(); 
        RectTransform rt = loadingImageComponent.rectTransform;
        Canvas canvas = loadingImageComponent.canvas;
        RectTransform canvasRT = canvas.GetComponent<RectTransform>();
        
        Vector2 targetSize = new Vector2(canvasRT.rect.width, canvasRT.rect.height) * 0.98f;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 initialWorldPos = rt.position;


        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * expandSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            rt.sizeDelta = Vector2.Lerp(Vector2.zero, targetSize, smoothT);
            rt.position = Vector3.Lerp(initialWorldPos, screenCenter, smoothT);

            yield return null;
        }

        rt.sizeDelta = targetSize;
        rt.position = screenCenter;
        rt.SetAsLastSibling();
        loadingScene.transform.SetAsLastSibling(); 
        loadingScene.SetActive(true);
        
        yield return new WaitForSeconds(timeUntilStart);
        SceneManager.LoadScene(phaseName);
    }
}
