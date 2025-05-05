using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogic : MonoBehaviour {

 
 //   [SerializeField]
    GridPieza solucionActual;
    GridPieza solucionFinal;

    [SerializeField]
    List<GridPieza> piezas;

    int columnas=7, filas=10;

    void Awake() {

        // Solución final
        solucionFinal = new GridPieza();
        solucionFinal.columns = columnas;
        solucionFinal.rows = filas;
        int[,] mtxA  = { // Es la que se utiliza de referencia para comprobar la solucion
            {1, 1,  1,  1,  1,  1,  1}, //1
            {1, 1,  1,  1,  1,  1,  1}, //2
            {1, 1,  2,  2,  2,  1,  1}, //3
            {1, 1,  2,  2,  2,  1,  1}, //1
            {4, 4,  2,  2,  2,  1,  1}, //2
            {4, 4,  2,  2,  2,  1,  1}, //3
            {4, 4,  3,  3,  3,  1,  1}, //7
            {4, 4,  3,  3,  3,  1,  1}, //8
            {4, 4,  3,  3,  3,  1,  1}, //9
            {4, 4,  3,  3,  3,  -1,  -1} //10
        };  // 1,2,3,4 es solución
        solucionFinal.matrizPieza = mtxA;

        // Solución actual: todo a false, que se va actualizar a cada rato
        // NO USAR COMO REFERENCIA DE LAS POSICIONES DE XMIN, XMAX, YMIN, YMAX
        // Para eso está la solucionFinal

        solucionActual = new GridPieza();
        solucionActual.columns = columnas; 
        solucionActual.rows = filas;
        solucionActual.matrizPieza = new int [filas,columnas];
        solucionFinal.IsLocal = solucionActual.IsLocal = true;
//        solucionFinal.UpdateCoords();
//        solucionActual.UpdateCoords();
        //NO => Final y actual tienen los valores por defecto: 
        //min: 0,0
        //max: 600,700

        for (int i=0; i < filas; i++) {
            for (int j=0; j < columnas; j++) {
                solucionActual.matrizPieza[i,j]=-1;
            }
        }

    }

    void CleanActual() {
        for (int i=0; i < filas; i++) {
            for (int j=0; j < columnas; j++) {
                solucionActual.matrizPieza[i,j]=-1;
            }
        }
    }

    public bool IsSolution() {
        bool test=true;
        for (int i=0; i < solucionActual.rows; i++) {
            for (int j=0; j < solucionActual.columns; j++) {
                if (solucionFinal.matrizPieza[i,j] > -1) { // Solo comprueba las piezas que estén bien
                    test &= solucionActual.matrizPieza[i,j] == solucionFinal.matrizPieza[i,j]; // Coincide (no se hace AND)
                }
            }
        }
        return test;
    }

    void Update() {

        // 1: Limpiamos la solución actual
        CleanActual();

        // 2: Por toda la lista de piezas 
        foreach (GridPieza pieza in piezas) {

            // 2.1: Comprobar que se pueda solapar this sobre la solución
            if (pieza.IsInside(solucionFinal)) {
                
                // 2.2: A partir de aquí solapan, pero puede que no sea la sol. final

                //    Como está dentro, hay una distancia entre el pivote de la solución y this
                //    Esa distancia, dará el desplazamiento para moverse en la matriz solución, recorriendo la matriz de this
                //    El pivote es (xmin,ymin)

                int RowOffset = (int)((float)filas     * Mathf.Abs(solucionFinal.xmax-pieza.xmin)/Mathf.Abs(solucionFinal.xmax-solucionFinal.xmin));
                int ColOffSet = (int)((float)columnas  * Mathf.Abs(solucionFinal.ymax-pieza.ymin)/Mathf.Abs(solucionFinal.ymax-solucionFinal.ymin));

                // 2.3: Escribir en la solución actual, la ocupación de la pieza deseada

                Debug.Log(pieza.tipoPieza + " INSIDE!");

                for (int i=0; i < pieza.rows; i++) {
                    break;
                    for (int j=0; j < pieza.columns; j++) {

                        int RowInSolution =    i+RowOffset;
                        int ColumnInSolution = j+ColOffSet;

                        // Sobreescribe todo el rato, esté bien o mal. Para eso está el isSolution()
                        if (pieza.matrizPieza[i,j] > -1) { // Si es valida la pos. i,j
                            Debug.Log("i:"+i+","+RowInSolution+" # j:" + j + ","+ColumnInSolution);
                            Debug.Log("Filas: "+filas+", Columnas: " + columnas);
                            solucionActual.matrizPieza[RowInSolution,ColumnInSolution] = pieza.tipoPieza;
                        }

                    }
                }

            } else {
                Debug.Log(pieza.tipoPieza + "OUTSIDE!");
            }

        }



    }

}
