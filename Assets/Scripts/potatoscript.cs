using UnityEngine;

public class potatoscript : MonoBehaviour {
    public Player playermov;
    public AudioSource potatoPickUp;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && playermov.tpskill == false) { potatoPickUp.Play(); Destroy(gameObject); }
    }//Pattis alma kodu farklý olmasýnýn amacý eðer bir tane hâli hazýrda almýþsa ikinciyi almasýný engellemek.
}
