using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour
{
    private int coin = 0;

    public TextMeshProUGUI coinText;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            coinText.text = "Coin: " + coin.ToString();
            Destroy(other.gameObject);
        }
    }
}
