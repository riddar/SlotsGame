using Assets.Scripts.DTO;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public List<Image> images;
    public Reels[] reels;
    bool startSpin = false;
    private string amountWon;
    private int[,] Symbols;
    public Text multiplier;
    public Text Won;
    public Text Wallet;
    public Text Bet;
    private double balance = 0;
    private double Lost = 0;
    private double won = 0;
    private double bet = 0;

    void Start()
    {
        AddSymbols();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpin && Input.GetKeyDown(KeyCode.Space))
        {
            startSpin = true;
            StartCoroutine(Spinning());
            startSpin = false;
        }
    }

    IEnumerator Spinning()
    {
        int reelnumber = 0;
        foreach (Reels reel in reels)
        {
            reel.spin = true;
            yield return new WaitForSeconds(Random.Range(1, 3));
            reel.spin = false;
            List<int> newSymbols = reel.RandomPosition(reels[reelnumber].transform.childCount, Symbols, reelnumber);
            for(int j = 0; j <= Symbols.GetLength(0) - 1; j++)
            {
                Symbols[j, reelnumber] = newSymbols[j];
            }
            reelnumber++;
        }   

        Multipliers(Symbols);
    }

    void Multipliers(int[,] symbols)
    {
        int multipliers = -1;
        int firstSymbol = symbols[(symbols.GetLength(0)/2+1), 0];
        for (int i = 0; i <= symbols.GetLength(1) + 1; i++)
        {
            int symbol = symbols[(symbols.GetLength(0) / 2)+1, i];
            Debug.LogWarning(symbol.ToString() + firstSymbol.ToString());
            if (symbol == firstSymbol)
                multipliers++;
            else
            {
                multipliers = 0;
                break;
            }
                

        }

        if (multipliers > 0)
        {
            won = multipliers * double.Parse(Bet.text);
            Won.text = "Won: " + won;
            balance += won;
            Wallet.text = "Balance: " + balance;
        }  
        else
        {
            balance -= int.Parse(Bet.text);
            Wallet.text = "Balance: " + balance;
            Lost = bet;
        }
            
        StartCoroutine(WalletUpload(1, balance, bet, won, Lost));
        multiplier.text = "Multiplier: " + multipliers;
    }

    void AddSymbols()
    {
        StartCoroutine(GetReelsBySlotId(1));
        StartCoroutine(GetWalletBySlotId(1));

        Symbols = new int[,]
        {
           {1,0,0,1,1},
           {1,1,1,1,1},
           {1,2,2,1,1},
           {3,3,3,1,1},
           {4,4,4,1,1},
           {1,0,3,1,1},
           {1,2,4,1,1},
           {1,3,0,1,1},
           {3,4,3,1,1},
           {4,0,4,1,1},
           {1,3,0,1,1},
           {1,4,0,1,1},
           {1,0,0,1,1}
        };

        for (int i = 0; i <= Symbols.GetLength(1) - 1; i++)
        {
            for (int j = 0; j <= Symbols.GetLength(0) - 1; j++)
            {
                Image image = Instantiate(images[Symbols[j, i]]) as Image;
                if (image != null)
                {
                    image.transform.SetParent(reels[i].transform);
                    image.transform.localPosition = new Vector2(0, j * 100 + (Symbols.GetLength(0) / 2 * -100));
                }
            }
        }
    }

    public IEnumerator GetReelsBySlotId(int id)
    {
        string url = "https://localhost:5001/api/v1/Reels/GetReelsBySlotId/";
        UnityWebRequest request = UnityWebRequest.Get(url + id);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogWarning(request.error);
            yield break;
        }

        JSONNode Reels = JSON.Parse(request.downloadHandler.text);
        Debug.Log(Reels);
    }

    

    public IEnumerator GetWalletBySlotId(int id)
    {
        string url = $"https://localhost:5001/api/v1/Wallets/" + id;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                yield break;
            }

            JSONNode wallet = JSON.Parse(request.downloadHandler.text);
            Debug.Log(wallet);

            balance = int.Parse(wallet["balance"].ToString());
            won = int.Parse(wallet["creditsWon"].ToString());
            bet = int.Parse(wallet["betAmount"].ToString());

            Wallet.text = "balance: " + wallet["balance"].ToString();
            Won.text = "Won: " + wallet["creditsWon"].ToString();
            Bet.text = "BetAmount: " + wallet["betAmount"].ToString();
        } 
    }

    public IEnumerator WalletUpload(int id, double balance, double betAmount, double creditsWon, double creditsLost )
    {
        string wallet = "{\"id\": "+ id +",\"balance\": " +balance+ ", \"betAmount\": "+betAmount+", \"creditsWon\": "+creditsWon+", \"creditsLost\": "+creditsLost+", \"slots\": null}";

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(wallet);
        using (UnityWebRequest www = UnityWebRequest.Put("https://localhost:5001/api/v1/Wallets/" + id, myData))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
                Debug.Log("Upload complete!");
        }
    }
}
