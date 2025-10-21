using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour
{
    private int coin = 0;
    public GameObject completeScreen;

    public TextMeshProUGUI coinText;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            coinText.text = "Coin: " + coin.ToString() + "/6";
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (coin == 6)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            completeScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
