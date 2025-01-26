using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogic : MonoBehaviour {

    GridPieza solucionActual, solucionFinal;

    [SerializeField]
    List<GridPieza> piezas;

    void Awake() {

        int columnas=5, filas=4;

        // Solución final
        solucionFinal = new GridPieza();
        solucionFinal.columns = columnas;
        solucionFinal.rows = filas;
        int[,] mtxA  = { // Es la que se utiliza de referencia para comprobar la solucion
            {-1, -1,  -1,  -1,  -1},
            {-1, -1,  1,    2,   2},
            {-1,  1,  1,    2,   2},
            {-1, -1,  1,   -1,  -1}
        };  // 1,2,3,4 es solución
        solucionFinal.matrizPieza = mtxA;

        // Solución actual: todo a false, que se va actualizar a cada rato
        solucionActual = new GridPieza();
        solucionActual.columns = columnas; 
        solucionActual.rows = filas;
        solucionActual.matrizPieza = new int [filas,columnas];
        CleanActual();

    }

    void CleanActual() {
        for (int i=0; i < solucionActual.rows; i++) {
            for (int j=0; j < solucionActual.columns; j++) {
                solucionActual.matrizPieza[i,j]=-1;
            }
        }
    }

    public bool IsSolution() {
        bool test=true;
        for (int i=0; i < solucionActual.rows; i++) {
            for (int j=0; j < solucionActual.columns; j++) {
                test &= solucionActual.matrizPieza[i,j] == solucionFinal.matrizPieza[i,j]; // Coincide (no se hace AND)
            }
        }
        return test;
    }

    public void UpdateActualSolution() {

        // 1: Limpiamos la solución actual
        CleanActual();
        
        int filas=solucionFinal.rows;
        int columnas=solucionFinal.columns;

        // 2: Por toda la lista de piezas 
        foreach (var pieza in piezas) {

            // 2.1: Comprobar que se pueda solapar this sobre la solución
            if (pieza.IsInside(solucionFinal)) {
                
                // 2.2: A partir de aquí solapan, pero puede que no sea la sol. final

                //    Como está dentro, hay una distancia entre el pivote de la solución y this
                //    Esa distancia, dará el desplazamiento para moverse en la matriz solución, recorriendo la matriz de this
                //    El pivote es (xmin,ymin)

                int RowOffset = (int)((float)filas    * (solucionFinal.xmax-pieza.xmin)/(solucionFinal.xmax-solucionFinal.xmin));
                int ColOffSet = (int)((float)columnas  * (solucionFinal.ymax-pieza.ymin)/(solucionFinal.ymax-solucionFinal.ymin));
                
                // 2.3: Escribir en la solución actual, la ocupación de la pieza deseada

                for (int i=0; i < filas; i++) {
                    for (int j=0; j < columnas; j++) {

                        int RowInSolution =    i+RowOffset;
                        int ColumnInSolution = j+ColOffSet;

                        // Sobreescribe todo el rato, esté bien o mal. Para eso está el isSolution()
                        if (pieza.matrizPieza[i,j] > -1) { // Si es valida la pos. i,j
                            solucionActual.matrizPieza[RowInSolution,ColumnInSolution] = pieza.tipoPieza;
                        }

                    }
                }

            } 
        }



    }

}
