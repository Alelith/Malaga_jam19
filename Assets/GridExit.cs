using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridExit : MonoBehaviour
{

    [SerializeField]
    Button salir;

    [SerializeField]
    GridLogic logicaPuzzle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (logicaPuzzle.IsSolution()){
            salir.interactable = true;
        }
    }
}
