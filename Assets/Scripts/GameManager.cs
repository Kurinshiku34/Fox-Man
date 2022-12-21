using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private navmeshAI navmeshAII;
    public Player playerr;
    public Text scoreText; public Text highscoreText; public Text meatAteNumberText;
    public AudioSource LevelUp;
    public static GameManager instance;
    public bool caneatghost;
    public int counter = 1; public int cscore = 0; public int highscore = 0;
    private void Awake() { instance = this; }
    private void Start()
    { // Oyunun baþlangýcýnda gerekli bir kaç ayarlamalar.
      // Sayaçlar için önemli kýsýmlar.
        cscore = PlayerPrefs.GetInt("cscore", 0);
        highscore = PlayerPrefs.GetInt("highscore", 0);
        playerr.meatAteNumber = PlayerPrefs.GetInt("meatAteNumber", 0);
        meatAteNumberText.text = playerr.meatAteNumber.ToString() + "";
        scoreText.text = cscore.ToString() + "";
        highscoreText.text = "" + highscore.ToString();
    }
    void Update() { if (playerr.scores == 118) { LevelUp.Play(); SceneManager.LoadScene("Game"); } } // Oyundaki tüm coinleri toplayýnca leveli yeniden yükleme kodu.
    public void AddPoint()
    { // Skorlarý tutma kodu
        cscore += 100;
        scoreText.text = cscore.ToString() + ""; if (highscore < cscore)
            PlayerPrefs.SetInt("highscore", cscore);

        scoreText.text = cscore.ToString() + "";
            PlayerPrefs.SetInt("cscore", cscore);

        meatAteNumberText.text = playerr.meatAteNumber.ToString() + "";
            PlayerPrefs.SetInt("meatAteNumber", playerr.meatAteNumber);
    }


    public void AddPointGhost()
    { // Hayalletleri yedikçe katlanarak skor kazanma kodu.
        if (counter < 4) {
        cscore = cscore + 100 * counter;
        scoreText.text = cscore.ToString() + ""; if (highscore < cscore)
        PlayerPrefs.SetInt("highscore", cscore);
        }
        else {
            cscore = cscore + 400;
            scoreText.text = cscore.ToString() + ""; if (highscore < cscore)
                PlayerPrefs.SetInt("highscore", cscore);
            scoreText.text = cscore.ToString() + "";
            PlayerPrefs.SetInt("cscore", cscore);
        }
    }












}
