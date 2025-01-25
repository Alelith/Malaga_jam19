using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogic : MonoBehaviour {

    int[][] solutionGrid;

    int[][] actualGrid;

    int rows=4,columns=5;

    public bool solved = false;

    public void IsSolution() {
        solved=true;
        for (int i=0; i < rows && solved; ++i) {
            for (int j=0; j < columns && solved; ++j) {
                solved &= solutionGrid[i][j] == actualGrid[i][j];
            }
        }
    }

    // Start is called before the first frame update
    void Start() {

        for (int i=0; i < rows; ++i) {
            for (int j=0; j < columns; ++j) {
                solutionGrid[i][j] = actualGrid[i][j] = -1; ///
            }
        }

    }

    public void SetActual(int r, int c, int value) {
        if (r < rows && c < columns) {
            actualGrid[r][c]=value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
