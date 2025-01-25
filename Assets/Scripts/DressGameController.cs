using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DressGameController : MonoBehaviour
{
    [SerializeField] List<GameObject> shirts;
    [SerializeField] List<GameObject> jeans;
    [SerializeField] List<GameObject> foots;

    readonly List<GameObject> correctDresses = new List<GameObject>();
    readonly List<GameObject> selectedDresses = new List<GameObject>();
    Dictionary<RectTransform, bool> anchors = new Dictionary<RectTransform, bool>();
    RectTransform tempDress;
    readonly Dictionary<string, Vector2> tempPosition = new Dictionary<string, Vector2>();

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
                    break;
                }
                if (anchor.Key.name.Contains("Shirt") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
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
                    break;
                }
                if (anchor.Key.name.Contains("Jean") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
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
                    break;
                }
                if (anchor.Key.name.Contains("Foot") && !anchor.Value && 
                    Vector2.Distance(tempDress.position, anchor.Key.position) < 1.5f)
                {
                    tempDress.position = anchor.Key.position;
                    anchors[anchor.Key] = true;
                    
                    selectedDresses.Add(tempDress.gameObject);
                    break;
                }
            }
        }
        
        tempDress = null;
    }
    
    public void OnCheckDresses()
    {
        bool correct = true;
        foreach (var dress in correctDresses)
            if (selectedDresses.Contains(dress))
                correct = false;
        if (correct)
            Debug.Log("Correct!");
        else
            Debug.Log("Incorrect!");
    }
}
