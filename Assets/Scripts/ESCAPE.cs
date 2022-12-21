using UnityEngine;
using UnityEngine.SceneManagement;
public class ESCAPE : MonoBehaviour { private void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene("Menu"); } } }