using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
	[SerializeField] private Text _scoreText;
	[SerializeField] private Text _ammoCountText;
    [SerializeField] private Text _gameoverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameoverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GAMEMANAGER IS NULL");
        }
    }

    public void UpdateScoreText(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    public void GameOverSequence()
    {
        _gameoverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

        StartCoroutine(ShowGameOverMessage());
    }

    IEnumerator ShowGameOverMessage()
    {
        while (true)
        {
            if(_gameoverText.gameObject.activeSelf == true)
            {
                _gameoverText.gameObject.SetActive(false);
            }
            else
            {
                _gameoverText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    
	public void UpdateAmmoCount(int ammoLeft)
	{
		_ammoCountText.text = "";
		
		for(int i = 0; i < ammoLeft; i++)
		{
			_ammoCountText.text = _ammoCountText.text + "I";
		}
	}

}
