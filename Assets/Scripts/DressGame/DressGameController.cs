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
    [SerializeField]
    [Tooltip("List of shirt GameObjects.")]
    List<GameObject> shirts;

    [SerializeField]
    [Tooltip("List of jean GameObjects.")]
    List<GameObject> jeans;

    [SerializeField]
    [Tooltip("List of foot GameObjects.")]
    List<GameObject> foots;

    [SerializeField]
    [Tooltip("The RectTransform of the open closet.")]
    RectTransform openCloset;

    [SerializeField]
    [Tooltip("The RectTransform of the info panel.")]
    RectTransform info;
    
    [SerializeField]
    [Tooltip("The Dialog component.")]
    Dialog dialog;

    [SerializeField]
    [Tooltip("The CanvasGroup for the general dialog.")]
    CanvasGroup generalDialog;

    [SerializeField]
    [Tooltip("The CanvasGroup for the exit button.")]
    CanvasGroup exitButton;

    [SerializeField]
    [Tooltip("List of correct dress GameObjects.")]
    List<GameObject> correctDresses = new List<GameObject>();

    /// <summary>
    /// List of currently selected dress GameObjects.
    /// </summary>
    List<GameObject> selectedDresses = new List<GameObject>();

    /// <summary>
    /// Dictionary that keeps track of the anchor positions for each dress.
    /// </summary>
    Dictionary<RectTransform, bool> anchors = new Dictionary<RectTransform, bool>();

    /// <summary>
    /// The currently dragged dress GameObject.
    /// </summary>
    RectTransform tempDress;

    /// <summary>
    /// Dictionary that keeps track of the temporary positions for each dress.
    /// </summary>
    readonly Dictionary<string, Vector2> tempPosition = new Dictionary<string, Vector2>();

    /// <summary>
    /// Dictionary that keeps track of the anchored positions for each dress.
    /// </summary>
    Dictionary<RectTransform, RectTransform> anchored = new(); 
    
    /// <summary>
    /// Indicates whether the text is currently being shown.
    /// </summary>
    bool isShowingText = false;

    void Start()
    {
        foreach (var shirt in shirts)
            tempPosition.Add(shirt.name, ((RectTransform)shirt.transform).anchoredPosition);
        foreach (var jean in jeans)
            tempPosition.Add(jean.name, ((RectTransform)jean.transform).anchoredPosition);
        foreach (var foot in foots)
            tempPosition.Add(foot.name, ((RectTransform)foot.transform).anchoredPosition);

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

    /// <summary>
    /// Called when a dress is being dragged.
    /// </summary>
    /// <param name="dress">The dress being dragged.</param>
    public void OnDragDress(RectTransform dress) => tempDress = dress;
    
    /// <summary>
    /// Called when a dress is dropped.
    /// </summary>
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
    
    /// <summary>
    /// Called when the dresses are checked for correctness.
    /// </summary>
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
    
    /// <summary>
    /// Called when the closet is changed.
    /// </summary>
    /// <param name="newCloset">The new closet to open.</param>
    public void OnChangeCloset(RectTransform newCloset)
    {
        if (openCloset == newCloset) return;
        openCloset.DOAnchorPosY(openCloset.sizeDelta.y, 0.5f).OnComplete(() =>
        {
            openCloset = newCloset;
            openCloset.DOAnchorPosY(0, 0.5f);
        });
    }
    
    /// <summary>
    /// Called when the dress information is shown.
    /// </summary>
    /// <param name="index">The index of the dress.</param>
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

    /// <summary>
    /// Called when the dress information is hidden.
    /// </summary>
    public void OnHideInfo() => info.GetComponent<CanvasGroup>().DOFade(0, 0.5f);

    /// <summary>
    /// Called when the initial settings are set.
    /// </summary>
    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
