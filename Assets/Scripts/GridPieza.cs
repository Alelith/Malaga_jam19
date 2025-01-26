using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPieza : MonoBehaviour {

    public int tipoPieza= 1;
    public float xmin=0, ymin=0;
    public float xmax=0, ymax=0;
    public int columns=0, rows=0;
    public int [,] matrizPieza;

    [SerializeField] RectTransform info, selff;

    void Start() {
        switch(tipoPieza){
            case 1: {
                columns = 7; 
                rows = 9;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    { 1,  1,   1,   1,  1,  1}, //1
                    { 1,  1,   1,   1,  1,  1}, //2
                    { 1,  1,  -1,  -1,  1,  1}, //3
                    { 1,  1,  -1,  -1,  1,  1}, //1
                    {-1, -1,  -1,  -1,  1,  1}, //2
                    {-1, -1,  -1,  -1,  1,  1}, //2
                    {-1, -1,  -1,  -1,  1,  1}, //2
                    {-1, -1,  -1,  -1,  1,  1}, //2
                    {-1, -1,  -1,  -1,  1,  1}, //2
                };
                matrizPieza = mtx;
                break;
            }
            case 2: {
                columns = 2; 
                rows = 4;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {2, 2},
                    {2, 2},
                    {2, 2},
                    {2, 2}
                };
                matrizPieza = mtx;
                break;
            }
            case 3: {
                columns = 2; 
                rows = 4;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {3, 3},
                    {3, 3},
                    {3, 3},
                    {3, 3}
                };
                matrizPieza = mtx;
                break;
            }
            case 4: {
                columns = 2; 
                rows = 6;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {4, 4},
                    {4, 4},
                    {4, 4},
                    {4, 4},
                    {4, 4},                    
                    {4, 4}
                };
                matrizPieza = mtx;
                break;
            }
            default: {
                columns = 1; 
                rows = 1;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {-1}
                };
                matrizPieza = mtx;
                break;
            }
        }
    }

    public bool IsInside(GridPieza piezaTest){
        bool testMe = true;

        testMe &= piezaTest.xmin <= this.xmin; // sobre xmin e ymin de this
        testMe &= this.xmin <= piezaTest.xmax;

        testMe &= piezaTest.ymin <= this.ymin;
        testMe &= this.ymin <= piezaTest.ymax;

        testMe &= piezaTest.xmin <= this.xmax; // sobre xmax e ymax de this
        testMe &= this.xmax <= piezaTest.xmax;

        testMe &= piezaTest.ymin <= this.ymax;
        testMe &= this.ymax <= piezaTest.ymax;

        return testMe;
    }

    public void OnDragPieza(RectTransform pieza) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(info,  Input.mousePosition, Camera.main,out Vector2 vector );
        pieza.anchoredPosition = vector + (Vector2.right * 500);
    }

    void Update() {
        xmin=selff.anchoredPosition.x;
        ymin=selff.anchoredPosition.y;
        xmax=xmin + selff.sizeDelta.x;
        ymax=ymin + selff.sizeDelta.y;
    }








}
