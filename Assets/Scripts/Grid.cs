using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    int [][] solutionGrid, actualGrid;
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
    void Start()
    {
/*        solutionGrid = new int[][]{
        
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
