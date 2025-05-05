using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogic : MonoBehaviour {

    
    [SerializeField]
    GameObject exitButton;

    GridPieza solucionActual, solucionFinal;

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

        // Solución Actual
        solucionActual = new GridPieza();
        solucionActual.columns = columnas; 
        solucionActual.rows = filas;

        int[,] mtxB  = { // Es la que se utiliza de referencia para comprobar la solucion
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
        solucionActual.matrizPieza = mtxB;
        //Final y actual tienen los valores de sus coordenadas por defecto: 
        //min: 0,0
        //max: 600,700
        // ESTO ES ASÍ PORQUE EL RESTO DE PIEZAS TRABAJAN CON COORDENADAS RELATIVAS / LOCALES
        // A LA MOCHILA, ENTONCES LA MOCHILA EMPIEZA EN 0,0 HASTA 600,700
    
        // Deja la solución actual vacía del todo con -1's en cada celda
         for (int i=0; i < filas; i++) {
            for (int j=0; j < columnas; j++) {
                solucionActual.matrizPieza[i,j]=-1;
            }
        }
        // Solución actual: todo a false, que se va actualizar a cada rato
        // NO USAR COMO REFERENCIA DE LAS POSICIONES DE XMIN, XMAX, YMIN, YMAX
        // Para eso está la solucionFinal
    }

    void VaciarActual() {
        for (int i=0; i < filas; i++) {
            for (int j=0; j < columnas; j++) {
                solucionActual.matrizPieza[i,j]=-1;
            }
        }
    }

    public bool IsSolution() {
        for (int i=0; i < solucionActual.rows; i++) {
            for (int j=0; j < solucionActual.columns; j++) {

                if (solucionFinal.matrizPieza[i,j] > -1) { // Solo comprueba las piezas que estén bien
                    if (solucionActual.matrizPieza[i,j] != solucionFinal.matrizPieza[i,j]) {
                        // No coincide (no se hace AND)
                        return false;
                    }
                }

            }
        }
        return true; // Todas las celdas coinciden
    }

    void Update() {


        // 1: Limpiamos la solución actual
        VaciarActual();

        // 2: Por toda la lista de piezas 
        foreach (GridPieza pieza in piezas) {

            // 2.1: Comprobar que se pueda solapar this sobre la solución
            if (pieza.IsInside(solucionFinal)) {
                
                // 2.2: A partir de aquí solapan, pero puede que no sea la sol. final

                //    Como está dentro, hay una distancia entre el pivote de la solución y this
                //    Esa distancia, dará el desplazamiento para moverse en la matriz solución, recorriendo la matriz de this
                //    El pivote es (xmin,ymin)
                
                //                          amplitud   * porcentaje
                int RowOffset = (int)((float)(filas-1)     * (1-(Mathf.Abs(solucionFinal.xmax-pieza.xmin)/Mathf.Abs(solucionFinal.xmax-solucionFinal.xmin))));
                int ColOffSet = (int)((float)(columnas-1)  * (1-(Mathf.Abs(solucionFinal.ymax-pieza.ymin)/Mathf.Abs(solucionFinal.ymax-solucionFinal.ymin))));

                // 2.3: Escribir en la solución actual, la ocupación de la pieza deseada
                
                float contadorTipoPieza=0.0f;
                if (RowOffset + pieza.rows <= filas && ColOffSet + pieza.columns <= columnas) {
                    for (int i=0; i < pieza.rows; i++) {
                        for (int j=0; j < pieza.columns; j++) {

                            int RowInSolution =    i+RowOffset;
                            int ColumnInSolution = j+ColOffSet;

                            // Sobreescribe todo el rato, esté bien o mal. Para eso está el isSolution()
                            if (pieza.matrizPieza[i,j] > -1) { // Si es valida la pos. i,j
                                Debug.Log("i:"+i+", j:" + j + "  ==>  " +RowInSolution+","+ColumnInSolution + ":::" + "Filas: "+filas+", Columnas: " + columnas);
                                solucionActual.matrizPieza[RowInSolution,ColumnInSolution] = pieza.tipoPieza;
                                if (pieza.tipoPieza == solucionFinal.matrizPieza[RowInSolution,ColumnInSolution]) {
                                    contadorTipoPieza += 1.0f;
                                }
                            }

                        }
                    }

                    //Tinte de la imagen
                    float porcentajeTinte=contadorTipoPieza/pieza.numTipoPiezas;

                    if (porcentajeTinte < 1.0f) { // Rojo
                        pieza.imagen.color = new Color(1.0f,porcentajeTinte,porcentajeTinte,1.0f); 
                    } else {                      // Verde
                        pieza.imagen.color = new Color(0.5f,1.0f,0.5f,1.0f); 
                    }

                }



            }

        }
        
        if (IsSolution()) {
            exitButton.SetActive(true);
        }else{
            exitButton.SetActive(false);
        }

    }

}
