using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DialogSystem;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DressGameController : IInitialSettings
{
    [SerializeField] List<GameObject> shirts;
    [SerializeField] List<GameObject> jeans;
    [SerializeField] List<GameObject> foots;
    [SerializeField] RectTransform openCloset;
    [SerializeField] RectTransform info;
    [SerializeField] Dialog dialog;
    [SerializeField] CanvasGroup generalDialog;
    [SerializeField] CanvasGroup exitButton;

    [SerializeField]
    List<GameObject> correctDresses = new List<GameObject>();
    List<GameObject> selectedDresses = new List<GameObject>();
    Dictionary<RectTransform, bool> anchors = new Dictionary<RectTransform, bool>();
    RectTransform tempDress;
    readonly Dictionary<string, Vector2> tempPosition = new Dictionary<string, Vector2>();

    Dictionary<RectTransform, RectTransform> anchored = new(); 
    
    bool isShowingText = false;

    void Start()
    {
        foreach (var shirt in shirts)
            tempPosition.Add(shirt.name, ((RectTransform)shirt.transform).anchoredPosition);
        foreach (var jean in jeans)
            tempPosition.Add(jean.name, ((RectTransform)jean.transform).anchoredPosition);
        foreach (var foot in foots)
            tempPosition.Add(foot.name, ((RectTransform)foot.transform).anchoredPosition);
        
        /*correctDresses.Add(shirts[Random.Range(0, shirts.Count)]);
        correctDresses.Add(jeans[Random.Range(0, shirts.Count)]);
        correctDresses.Add(foots[Random.Range(0, shirts.Count)]);*/

        foreach (var i in GameObject.FindGameObjectsWithTag("Anchor"))
            anchors.Add((RectTransform)i.transform, false);
    }

    void FixedUpdate()
    {
        foreach (var ancor in anchored)
            ancor.Key.position = ancor.Value.position;
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(((RectTransform)transform.parent.parent),Input.mousePosition,Camera.main,out Vector2 vector );
        if (tempDress)
            tempDress.anchoredPosition = vector + (Vector2.left * 500);
        info.anchoredPosition = vector + (Vector2.left * 500);
    }

    public void OnDragDress(RectTransform dress) => tempDress = dress;
    
    public void OnDropDress() 
    {
        if (tempDress.CompareTag("Shirt"))
        {
            foreach (var anchor in anchors)
            {
                if (anchor.Key.name.Contains("Shirt") && (anchor.Value || 
                                                          Vector2.Distance(tempDress.position, anchor.Key.position) >= 1.5f))
                {
                    if (selectedDresses.Contains(tempDress.gameObject))
                        selectedDresses.Remove(tempDress.gameObject);
                    tempDress.anchoredPosition = tempPosition[tempDress.name];
                    anchors[anchor.Key] = false;
                    if (anchored.ContainsKey(tempDress))
                        anchored.Remove(tempDress);
                    break;
                }
                if (anchor.Key.name.Contains("Shirt") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
                    
                    anchored.Add(tempDress, anchor.Key);
                    break;
                }
            }
        }
        if (tempDress.CompareTag("Jean"))
        {
            foreach (var anchor in anchors)
            {
                if (anchor.Key.name.Contains("Jean") && (anchor.Value || 
                                                         Vector2.Distance(tempDress.position, anchor.Key.position) >= 1.5f))
                {
                    if (selectedDresses.Contains(tempDress.gameObject))
                        selectedDresses.Remove(tempDress.gameObject);
                    tempDress.anchoredPosition = tempPosition[tempDress.name];
                    anchors[anchor.Key] = false;
                    if (anchored.ContainsKey(tempDress))
                        anchored.Remove(tempDress);
                    break;
                }
                if (anchor.Key.name.Contains("Jean") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
                    
                    anchored.Add(tempDress, anchor.Key);
                    break;
                }
            }
        }
        if (tempDress.CompareTag("Foot"))
        {
            foreach (var anchor in anchors)
            {
                if (anchor.Key.name.Contains("Foot") && (anchor.Value || 
                                                         Vector2.Distance(tempDress.position, anchor.Key.position) >= 1.5f))
                {
                    if (selectedDresses.Contains(tempDress.gameObject))
                        selectedDresses.Remove(tempDress.gameObject);
                    tempDress.anchoredPosition = tempPosition[tempDress.name];
                    anchors[anchor.Key] = false;
                    if (anchored.ContainsKey(tempDress))
                        anchored.Remove(tempDress);
                    break;
                }
                if (anchor.Key.name.Contains("Foot") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
                    
                    anchored.Add(tempDress, anchor.Key);
                    break;
                }
            }
        }
        
        tempDress = null;
    }
    
    public void OnCheckDresses()
    {
        int correctDressesCount = 0;
        foreach (var dress in correctDresses)
            if (selectedDresses.Contains(dress))
                correctDressesCount++;
        if (selectedDresses.Count == 3)
        {
            Typewriter typewriter = generalDialog.GetComponentInChildren<Typewriter>();
            generalDialog.DOFade(1, 0.5f);
            
            openCloset.DOAnchorPosY(openCloset.sizeDelta.y, 0.5f).OnComplete(() =>
            {
                openCloset = null;
            });
            
            exitButton.DOFade(1, 0.5f);
            exitButton.interactable = true;
            exitButton.blocksRaycasts = true;
            
            if (correctDressesCount == 3)
                typewriter.Write(CustomTagsManager.ProccessMessage(dialog[dialog.Count - 1], out string clearMessage), clearMessage);
            else
                typewriter.Write(CustomTagsManager.ProccessMessage(dialog[Random.Range(15, dialog.Count - 1)], out string clearMessage), clearMessage);
        }
        else
            Debug.Log("No has seleccionado todas las prendas");
    }
    
    public void OnChangeCloset(RectTransform newCloset)
    {
        if (openCloset == newCloset) return;
        openCloset.DOAnchorPosY(openCloset.sizeDelta.y, 0.5f).OnComplete(() =>
        {
            openCloset = newCloset;
            openCloset.DOAnchorPosY(0, 0.5f);
        });
    }
    
    public void OnShowInfo(int index)
    {
        if (!isShowingText)
        {
            info.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

            List<DialogueAction> commands = CustomTagsManager.ProccessMessage(dialog[index], out string clearMessage);

            info.GetComponentInChildren<Typewriter>().Write(commands, clearMessage);
        }
        else
            info.GetComponent<CanvasGroup>().alpha = 0;
    }
    
    public void OnHideInfo() => info.GetComponent<CanvasGroup>().DOFade(0, 0.5f);

    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
