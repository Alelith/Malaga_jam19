using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DressGameController : MonoBehaviour, IInitialSettings
{
    [SerializeField] List<GameObject> shirts;
    [SerializeField] List<GameObject> jeans;
    [SerializeField] List<GameObject> foots;
    [SerializeField] RectTransform openCloset;

    List<GameObject> correctDresses = new List<GameObject>();
    List<GameObject> selectedDresses = new List<GameObject>();
    Dictionary<RectTransform, bool> anchors = new Dictionary<RectTransform, bool>();
    RectTransform tempDress;
    readonly Dictionary<string, Vector2> tempPosition = new Dictionary<string, Vector2>();

    Dictionary<RectTransform, RectTransform> anchored = new(); 

    void Start()
    {
        foreach (var shirt in shirts)
            tempPosition.Add(shirt.name, ((RectTransform)shirt.transform).anchoredPosition);
        foreach (var jean in jeans)
            tempPosition.Add(jean.name, ((RectTransform)jean.transform).anchoredPosition);
        foreach (var foot in foots)
            tempPosition.Add(foot.name, ((RectTransform)foot.transform).anchoredPosition);
        
        correctDresses.Add(shirts[Random.Range(0, shirts.Count)]);
        correctDresses.Add(jeans[Random.Range(0, shirts.Count)]);
        correctDresses.Add(foots[Random.Range(0, shirts.Count)]);

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
        if (tempDress)
            tempDress.anchoredPosition = new(Input.mousePosition.x / Screen.width * 1920,
                Input.mousePosition.y / Screen.height * 1080);
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
        if (correctDressesCount == 3)
            Debug.Log("Correct!");
        else
            Debug.Log("Incorrect!");
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

    public void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
