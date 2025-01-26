using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPieza : MonoBehaviour {

    public int tipoPieza= 1;
    public float xmin=0, ymin=0;
    public float xmax=0, ymax=0;
    public int columns=0, rows=0;
    public int [,] matrizPieza;

    void Start() {
        switch(tipoPieza){
            case 1: {
                columns = 2; 
                rows = 3;
                int[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {-1, tipoPieza},
                    {tipoPieza,  tipoPieza},
                    {-1, tipoPieza}
                };
                matrizPieza = mtx;
                break;
            }
            case 2: {
                break;
            }
            case 3: {

                break;
            }default: {

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








}
