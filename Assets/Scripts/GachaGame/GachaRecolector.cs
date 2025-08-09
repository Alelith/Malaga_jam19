using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GachaRecolector : MonoBehaviour
{

    /// <summary>
    /// Current score points.
    /// </summary>
    int points = 0;

    [SerializeField]
    [Tooltip("Text component to display the score.")]
    TMP_Text countText;

    [SerializeField]
    [Tooltip("Rigidbody2D component of the collector.")]
    Rigidbody2D gachaCollector;

    [SerializeField]
    [Tooltip("Multiplier for the collector's movement speed.")]
    float gachaMultiplier = 1.0f;

    void OnEnable() {
        points=0;    
    }

    void Update() {

        Vector2 pos = new Vector2(transform.position.x,transform.position.y);
   
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            gachaCollector.totalForce = Vector2.zero;
            gachaCollector.AddForce(Vector2.left*Time.deltaTime*gachaMultiplier);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            gachaCollector.totalForce = Vector2.zero;
            gachaCollector.AddForce(Vector2.right*Time.deltaTime*gachaMultiplier);
        }

        // Update UI
        countText.text =  $"{points}";
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ObjetoGacha")) {
            Destroy(other.gameObject);
            points++;
        }
    }

}
