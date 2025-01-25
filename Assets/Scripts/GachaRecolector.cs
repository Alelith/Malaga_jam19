using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GachaRecolector : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D recolector;

    [SerializeField]
    float multiplicador = 1.0f;

    void Update() {

        Vector2 pos = new Vector2(transform.position.x,transform.position.y);
   
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            recolector.totalForce = Vector2.zero;
            recolector.AddForce(Vector2.left*Time.deltaTime*multiplicador);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            recolector.totalForce = Vector2.zero;
            recolector.AddForce(Vector2.right*Time.deltaTime*multiplicador);
        }

    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ObjetoGacha")) {
            Destroy(other.gameObject);
        }
    }

}
