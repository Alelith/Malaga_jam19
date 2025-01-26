using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {

    [SerializeField]
    int tipoPieza= 1;

    [SerializeField]
    int xmin=0, ymin=0;

    [SerializeField]
    int xmax=0, ymax=0;

    public int columns=0, rows=0;

    public bool [,] matrizPieza;

    public Pieza GetSolucion() { // Caso especial: la solución
        Pieza sol= new Pieza();
        sol.columns = 5; 
        sol.rows = 4;
        bool[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
            {false, false,  false,  false,  false},
            {false, false,  true,   false,  false},
            {false, true,   true,   false,  false},
            {false, false,  true,   false,  false}
        };
        sol.matrizPieza = mtx;
        return sol;
    }

    void Start() {
        switch(tipoPieza){
            case 1: {
                columns = 2; 
                rows = 3;
                bool[,] mtx  = { // Es la que se utiliza de referencia para comprobar la solucion
                    {false, true},
                    {true,  true},
                    {false, true}
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

    public bool IsInside(Pieza piezaTest){
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


    public bool IsSolution() {

        Pieza solucion = GetSolucion();

        // 1: Comprobar que se pueda solapar this sobre la solución
        if (IsInside(solucion)){
            // 2: A partir de aquí solapan, pero puede que no sea la sol. final

            //    Como está dentro, hay una distancia entre el pivote de la solución y this
            //    Esa distancia, dará el desplazamiento para moverse en la matriz solución, recorriendo la matriz de this
            //    El pivote es (xmin,ymin)

            int RowOffset = (int)((float)solucion.rows     *(solucion.xmax-this.xmin)/(solucion.xmax-solucion.xmin));
            int ColOffSet = (int)((float)solucion.columns  *(solucion.ymax-this.ymin)/(solucion.ymax-solucion.ymin));

            //    Si no coincide this con la solucion ->
            //    PD: Habrá que guardar la actual e ir cambiandola
            
            bool IsPartialSolution = true;
            

            for (int i=0; i < solucion.rows; i++) {
                for (int j=0; j < solucion.columns; j++) {

                    int RowInSolution = i+RowOffset;
                    int ColumnInSolution = j+ColOffSet;

                    IsPartialSolution &= this.matrizPieza[i,j] == solucion.matrizPieza[RowInSolution,ColumnInSolution];

                }    
            }

            if (IsPartialSolution) {
                
            }

        }

        return false;

    }





}
