using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace Assets.Scripts.DTO
{
    public class ReelsDTO
    {
        private const string url = "https://localhost:5001/api/v1/Reels/GetReelsBySlotId/";

        public IEnumerator GetReelsBySlotId(int id)
        {
            UnityWebRequest request = UnityWebRequest.Get(url + id);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                yield break;
            }

            JSONNode Reels = JSON.Parse(request.downloadHandler.text);
            Debug.LogWarning(Reels.Count);
            var t = Reels;
        }
    }
}
