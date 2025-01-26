using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] string badFinal;
    [SerializeField] string goodFinal;

    [SerializeField] Image mask;
    [SerializeField] Sprite[] masks;

    int goodGamesPassed;
    
    float time;
    
    public GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        time += Time.deltaTime;
        
        if (time >= 45)
            mask.sprite = masks[1];
        else if (time >= 90)
            mask.sprite = masks[2];
        else if (time >= 135)
            mask.sprite = masks[3];
        else if (time >= 180)
            mask.sprite = masks[4];
        else if (time >= 300)
            SceneManager.LoadScene(badFinal);
        
        if (goodGamesPassed == 2)
            SceneManager.LoadScene(goodFinal);
    }
}
