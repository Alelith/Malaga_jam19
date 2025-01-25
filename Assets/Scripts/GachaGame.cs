using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class GachaGame : MonoBehaviour {

    [SerializeField]
    Transform[] puntosSpawners;
    int maxObjetosEnPantalla = 3;
    int objetosActual = 0;

    [SerializeField]
    float minTiempo=0.4f,maxTiempo=1.11f;

    [SerializeField]
    GameObject contador, puntaje;

    [SerializeField]
    GameObject[] objetoPrefab;

    // Start is called before the first frame update
    void Awake() {
        
    }

    // Update is called once per frame
    void Update() {
        StartCoroutine(EsperaSpawn());
    }

    int lastPosicionSpawn=-1;
    IEnumerator EsperaSpawn() {
        
      if (objetosActual < maxObjetosEnPantalla) {
        objetosActual++;
        float tiempo=Random.Range(minTiempo,maxTiempo);
        yield return new WaitForSeconds(tiempo);

        int nuevaPosicion;
        do{
            nuevaPosicion=Random.Range(0,5);
        }while(nuevaPosicion == lastPosicionSpawn);

        Instantiate(objetoPrefab[Random.Range(0, objetoPrefab.Length)],puntosSpawners[nuevaPosicion]);
        lastPosicionSpawn = nuevaPosicion;
        objetosActual--;
      }
    }

}
