using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaObjectFall : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ObjetoGacha")) {
            Destroy(other.gameObject); //this, el objeto
        }

    }
}
