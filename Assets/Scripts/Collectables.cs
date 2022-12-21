using UnityEngine;
public class Collectables : MonoBehaviour {
    public AudioSource pickUp;
    private void OnTriggerEnter2D(Collider2D col) 
    { if (col.gameObject.tag == "Player") { pickUp.Play(); gameObject.SetActive(false); } }
    //Deðdiðimiz cismi alma kodu.
}