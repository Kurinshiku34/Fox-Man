using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public GameManager managerGame;
    public Rigidbody2D rigidbody;
    private SpriteRenderer foxSprite;
    public VideoPlayer videoPlayer;
    [SerializeField] GameObject ghost1;
    [SerializeField] GameObject ghost2;
    [SerializeField] GameObject ghost3;
    [SerializeField] GameObject ghost4;
    public GameObject GameOver; public GameObject Jumpscare; public GameObject JumpscarePlayer;
    public GameObject tp;
    public GameObject heart1; public GameObject heart2; public GameObject heart3;
    public GameObject meat1; public GameObject meat2; public GameObject meat3; public GameObject meat4; 
    public Transform player; public Transform tpp; public Transform dead;
    Animator animator;
    public Animator portalanimator; public Animator ghost1anim; public Animator ghost2anim; public Animator ghost3anim; public Animator ghost4anim;
    public AudioSource hurt; public AudioSource eatGhost; public AudioSource teleportSound; public AudioSource Music;
    public bool eatskilleated; public bool tpskill;
    public float scores; public float health = 3; public float speedMultiplier = 1f; public float speed = 15f;
    public int meatAteNumber;
    public Vector2 initialDirection;
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }
    public LayerMask obstacleLayer;
    Vector2 movement;

    void Start()
    { // Gerekli olan þeyler çaðrýlýyor. Oyunun baþlangýç ayarlamalarý.
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        foxSprite = GetComponent<SpriteRenderer>();
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }
    public bool Occupied(Vector2 direction)
    { // Eðer collider'a çarpmazsa o yönde engelin(obstacle) olmadýðýný belirliyor.
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }
    public void SetDirection(Vector2 direction, bool forced = false) 
    { // Yön belirleme prensibi.
        if (forced || !Occupied(direction)) { this.direction = direction; nextDirection = Vector2.zero; }
        else { nextDirection = direction; }
    }
    private void FixedUpdate()
    { // Hareket çalýþma prensibi.
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }
    void Update() 
    { // Hareket etmenin binding'leri/atamalarý.
        if (nextDirection != Vector2.zero) { SetDirection(nextDirection); }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { SetDirection(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { SetDirection(Vector2.down); }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { SetDirection(Vector2.left); foxSprite.flipY = true; }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { SetDirection(Vector2.right); foxSprite.flipY = false; }

      // Tilkimizin gideceði yere bakmasýný saðlayan kod.
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        if (tpskill == true)
        { // Oyundaki portallara ýþýnlanmayý saðlayan kod.
            if (Input.GetKeyDown(KeyCode.E)) { StartCoroutine(teleport()); tpskill = false; teleportSound.Play(); }
        }
        
      // Canlarýn eksilmesi ve canýn bitince ölmeni saðlayan kodlar.
        if (health == 3) {
            Jumpscare.gameObject.SetActive(false);
            JumpscarePlayer.gameObject.SetActive(false);
        }
        if (health == 2) { heart3.gameObject.SetActive(false); }
        if (health == 1) { heart2.gameObject.SetActive(false); }
        if (health == 0) {
            StartCoroutine(JumpscareIE());
            heart1.gameObject.SetActive(false);
            GameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator JumpscareIE()
    {
        Music.Pause();
        JumpscarePlayer.gameObject.SetActive(true);
        Jumpscare.gameObject.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(4f);
        videoPlayer.Stop();
        Jumpscare.SetActive(false);
        JumpscarePlayer.SetActive(false);
        Music.UnPause();
        Destroy(Jumpscare);
        Destroy(JumpscarePlayer);
    }
    IEnumerator teleport()
    { // Iþýnlanma animasyonu birlikte portala ýþýnlanma kodu.
        animator.SetBool("isTeleporting", true); portalanimator.SetBool("isTeleporting", true);
        yield return new WaitForSecondsRealtime(1f);
        player.transform.position = tpp.transform.position;
        animator.SetBool("isTeleporting", false); portalanimator.SetBool("isTeleporting", false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.gameObject.tag == "Enemy" && managerGame.caneatghost == false)
        { // Eðer hayaletleri yiyemiyorsa canýn eksildiði kod.
            hurt.Play();
            health--;
            player.transform.position = dead.transform.position;
            StartCoroutine(respawnghostdeactive());
        }
        if (col.gameObject.tag == "meat")
        { // Et yeme sayacý.
            meatAteNumber++;
            managerGame.AddPoint();
        }

        IEnumerator respawnghostdeactive()
        { // Canýmýz azaldýðýnda baþlangýçta buga girmeyelim diye oyuncuya 2 saniyelik kaçýþ fýrsatý veriyoruz
            ghost1.SetActive(false); ghost2.SetActive(false); ghost3.SetActive(false); ghost4.SetActive(false);
            yield return new WaitForSecondsRealtime(2f);
            ghost1.SetActive(true); ghost2.SetActive(true); ghost3.SetActive(true); ghost4.SetActive(true);
        }


        if (col.gameObject.tag == "score") { scores++; }
        if (col.gameObject.tag == "eatskill") {
            managerGame.counter = 1;
            StartCoroutine(eatskills());
        }
        if (col.gameObject.tag == "tpskill") {
            portalanimator.SetBool("isPotatoEaten", true);
            tpskill = true;
        }

        IEnumerator eatskills()
        { // Tilki sihirli mantarý yedikten sonra olanlarýn kodu. (animasyon, vb.)
            animator.SetBool("isMushEaten", true);
            ghost1anim.SetBool("isMushEaten", true);
            ghost2anim.SetBool("isMushEaten", true);
            ghost3anim.SetBool("isMushEaten", true);
            ghost4anim.SetBool("isMushEaten", true);
            eatskilleated = true; managerGame.caneatghost = true;
            yield return new WaitForSecondsRealtime(10f);
            animator.SetBool("isMushEaten", false);
            ghost1anim.SetBool("isMushEaten", false);
            ghost2anim.SetBool("isMushEaten", false);
            ghost3anim.SetBool("isMushEaten", false);
            ghost4anim.SetBool("isMushEaten", false);
            eatskilleated = false; managerGame.caneatghost = false;
        }

// HAYALETLERÝN YENÝLDÝKLERÝNDE AKTÝFLEÞECEK KODLAR HEPSÝNÝN KÝ AYRI OLDUÐU ÝÇÝN ÖZÜR DÝLERÝM :(
//---------------------------------------------------------------------------------------------------
        if (col.gameObject.name == "Ghost1") {
            if (managerGame.caneatghost == true)
            { // StartCorotine ile ghostun setactivesini açýp kapýyoruz.
                eatGhost.Play();
                meat1.SetActive(true);
                managerGame.AddPointGhost();
                StartCoroutine(setactives1());
                managerGame.counter = managerGame.counter + 1;
            }
        }

        IEnumerator setactives1() {
            ghost1.SetActive(false);
            yield return new WaitForSecondsRealtime(5f);
            ghost1.SetActive(true);
        }
//---------------------------------------------------------------------------------------------------
        if (col.gameObject.name == "Ghost2") {
            if (managerGame.caneatghost == true) {
                eatGhost.Play();
                meat2.SetActive(true);
                managerGame.AddPointGhost();
                StartCoroutine(setactives2());
                managerGame.counter = managerGame.counter + 1;
            }
        }

        IEnumerator setactives2() {
            ghost2.SetActive(false);
            yield return new WaitForSecondsRealtime(5f);
            ghost2.SetActive(true);
        }
//---------------------------------------------------------------------------------------------------
        if (col.gameObject.name == "Ghost3") {
            if (managerGame.caneatghost == true) {
                eatGhost.Play();
                meat3.SetActive(true);
                managerGame.AddPointGhost();
                StartCoroutine(setactives3());
                managerGame.counter = managerGame.counter + 1;
            }
        }

        IEnumerator setactives3() {
            ghost3.SetActive(false);
            yield return new WaitForSecondsRealtime(5f);
            ghost3.SetActive(true);
        }
//---------------------------------------------------------------------------------------------------
        if (col.gameObject.name == "Ghost4") {
            if (managerGame.caneatghost == true) {
                eatGhost.Play();
                meat4.SetActive(true);
                managerGame.AddPointGhost();
                StartCoroutine(setactives4());
                managerGame.counter = managerGame.counter + 1;
            }
        }

        IEnumerator setactives4() {
            ghost4.SetActive(false);
            yield return new WaitForSecondsRealtime(5f);
            ghost4.SetActive(true);
        }
    }
}