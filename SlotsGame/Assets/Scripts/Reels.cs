using System;
using System.Collections.Generic;
using UnityEngine;

public class Reels : MonoBehaviour
{
    public bool spin;
    public List<int> positions;
    int speed;

    // Start is called before the first frame update
    void Start()
    {
        spin = false;
        speed = 1500;
    }

    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            foreach (Transform image in transform)
            {
                image.transform.Translate(Vector2.down * Time.smoothDeltaTime * speed, Space.World);

                if (image.transform.position.y <= 0)
                    image.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + 600);
            }
        }
    }

    public List<int> RandomPosition(int count, int[,] symbols, int reel)
    {
        positions.Clear();
        for (int i = 1; i <= count; i++)
        {
            positions.Add(i * 100 + (count / 2 * -100));
        }

        int places = RandomNumber(positions.Count);
        positions = RotatePositionListLeft(positions, places);

        List<int> newSymbols = RotateSymbolsListLeft(symbols, places, reel);

        int j = 0;
        foreach (Transform image in transform)
        {
            var randPosition = new Vector2(image.transform.position.x, positions[j] + transform.parent.GetComponent<RectTransform>().transform.position.y);
            image.transform.position = randPosition;
            j++;
        }

        return newSymbols;
    }

    private List<int> RotateSymbolsListLeft(int[,] symbols, int places, int reel)
    {
        List<int> newitems = new List<int>();
        for (int i = places; i < symbols.GetLength(0); i++)
        {
            newitems.Add(symbols[i, reel]);
        }

        for (int j = 0; j < places; j++)
        {
            newitems.Add(symbols[j, reel]);
        }

        return newitems;
    }

    public List<int> RotatePositionListLeft(List<int> items, int places)
    {
        List<int> newItems = new List<int>();

        for (int i = places; i < items.Count; i++)
        {
            newItems.Add(items[i]);
        }

        for (int i = 0; i < places; i++)
        {
            newItems.Add(items[i]);
        }

        return newItems;
    }

    int RandomNumber(int range) => UnityEngine.Random.Range(0, range);
}