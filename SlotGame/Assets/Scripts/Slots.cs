using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public List<Image> images;
    public Reels[] reels;
    public Text multiplier;
    bool startSpin = false;

    void Start()
    {
        AddSymbols();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpin && Input.GetMouseButtonDown(0))
        {
            startSpin = true;
            StartCoroutine(Spinning());           
            startSpin = false;
        }
    }

    IEnumerator Spinning()
    {
        foreach(Reels reel in reels)
        {
            reel.spin = true;
            yield return new WaitForSeconds(Random.Range(1, 3));
            reel.spin = false;
            reel.RandomPosition(reel.transform.childCount);
        }

        multiplier.text = "Multiplier: " + Multipliers(reels);
    }

    int Multipliers(Reels[] reels)
    {
        Debug.LogWarning(reels[0].positions[(reels[0].transform.childCount / 2)]);
        int firstPosition = reels[0].positions[(reels[0].transform.childCount/2) +1];
        int multipliers = -1;

        foreach(Reels reel in reels)
        {
            if (reel.positions[(reel.transform.childCount / 2) +1] == firstPosition) multipliers++;
            else return multipliers;         
        }

        return multipliers;
    }

    void AddSymbols()
    {
        var Symbols = new int[,]
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
                Image image = Instantiate(images[Symbols[j,i]]) as Image;
                if (image != null)
                {
                    image.transform.parent = reels[i].transform;
                    image.transform.localPosition = new Vector2(0, j * 100 + (Symbols.GetLength(0)/2 * -100));
                }            
            }
            
        }
    }

    
}
