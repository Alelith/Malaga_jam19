using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridCoord {
    public int X,Y;
    public Vector3 worldCoordPos;
}

public class GridLogic : MonoBehaviour {

    Dictionary<Pieza, List<GridCoord>> solutionGrid;

    Dictionary<Pieza, List<GridCoord>> actualGrid;

    int rows=4, columns=5;

    float gridCellSize;

    public void SnapToCell(Pieza pieza){
        
    }

    /*[SerializeField]
    public float fxmin;
    [SerializeField]
    public float fxmax;
    [SerializeField]
    public float fymin;
    [SerializeField]
    public float fymax; 

    public GridCoord ToGridCoords(float x, float y) {
        float distFX = fxmax-fxmin;
        float distFY = fymax-fymin;
        GridCoord coords;
        coords.x = (int)(fxmax-x)/distFX;
        coords.y = (int)(fymax.y)/distFY;
        return coords;
    }

    public bool IsSolution() {
        bool solved=true;
        for (int i=0; i < rows && solved; ++i) {
            for (int j=0; j < columns && solved; ++j) {
                solved &= solutionGrid[i][j] == actualGrid[i][j];
            }
        }
        return solved;
    }

    // Start is called before the first frame update
    void Start() {

        for (int i=0; i < rows; ++i) {
            for (int j=0; j < columns; ++j) {
                solutionGrid[i][j] = actualGrid[i][j] = -1; ///
            }
        }

        // Otros elementos van rellenados a mano en solutionGrid
        // Cada valor es un objeto, excepto -1 que es el vacio



        
    }

    public void SetActual(int r, int c, int value) {
        if (r < rows && c < columns) {
            actualGrid[r][c]=value;
        }
    }*/

}
