using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class FlickrLoader : MonoBehaviour {

    static JsonSerializerSettings serializer;
    public Text resultText;
    public Image img;
    public Button button;
    System.Random random;
    // Web Call result variable
    string result;

    public FlickrLoader()
    {
        if (serializer == null)
        {
            serializer = new JsonSerializerSettings();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }
        random = new System.Random();
    }

    private string FlickrUrl(string action, string query = "")
    {
        if (!string.IsNullOrEmpty(query))
            query += "&";

        return "https://api.flickr.com/services/rest/?method=flickr.photos." + action + "&" + query + "api_key=ca370d51a054836007519a00ff4ce59e&format=json&nojsoncallback=1";
    }

    public void Search()
    {
        var search = Uri.EscapeUriString("landscape hdr");
        StartCoroutine(GetNewPhoto(FlickrUrl("search", "text=" + search)));
        //StartCoroutine(makeGetRequest(FlickrUrl("getRecent", "per_page=10")));
    }

    IEnumerator GetNewPhoto(string url)
    {
        button.interactable = false;

        yield return makeGetRequest(url);
        yield return GetRandomPhotoUrl(result);
        
        string imageUrl = result;

        WWW www = new WWW(result);
        StartCoroutine(ShowProgress(www));
        yield return www;

        Debug.Log("imageUrl " + imageUrl);
        Debug.Log("www.texture.width " + www.texture.width + " | www.texture.height " + www.texture.height);

        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 32);

        button.interactable = true;
    }

    IEnumerator ShowProgress(WWW www)
    {
        while (!www.isDone)
        {
            var progress = string.Format("Downloaded {0:P1}", www.progress);
            Debug.Log(progress);
            resultText.text = progress;
            yield return new WaitForSeconds(.1f);
        }
        resultText.text = "Photo loaded.";
        Debug.Log("Done");
    }

    IEnumerator GetRandomPhotoUrl(string photoListJson)
    {
        JToken photolist = ParsePhotoList(photoListJson);

        var photolistCount = photolist.Count();
        JToken imageId = RandomPhotoId(photolist, photolistCount);

        var imageInfoUrl = FlickrUrl("getSizes", "photo_id=" + imageId);
        yield return makeGetRequest(imageInfoUrl);

        if (result != null)
            result = GetPhotoUrl(result, "Large 1600"); // Medium
    }

    private static JToken ParsePhotoList(string response)
    {
        Debug.Log("Received: " + response);
        JToken token = JContainer.Parse(response);
        var photolist = token["photos"]["photo"];
        return photolist;
    }

    private JToken RandomPhotoId(JToken photolist, int photolistCount)
    {
        int randomNumber = random.Next(0, photolistCount);
        var imageId = photolist[randomNumber]["id"];
        return imageId;
    }

    private string GetPhotoUrl(string sizesJson, string labelText)
    {
        JToken imgSizesToken = JContainer.Parse(sizesJson);

        var sizes = imgSizesToken["sizes"]["size"];
        var size = sizes.FirstOrDefault(s => s["label"].Value<string>() == labelText);

        var imageUrl = (size == null || size["source"] == null) ? GetPhotoUrl(sizesJson, "Medium") : size["source"].ToString();
        return imageUrl;
    }
    
    IEnumerator makeGetRequest(string url)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.Send();

        Debug.Log("URL: " + url);

        if (req.isError)
        {
            Debug.Log("Error: " + req.error);
            result = null;
        }
        else
            result = req.downloadHandler.text;
    }

    IEnumerator makePostRequest<T>(string url, T data)
    {

        var serialized = JsonConvert.SerializeObject(data);

        Debug.Log("Serialized: " + serialized);

        UnityWebRequest req = UnityWebRequest.Post(url, serialized);

        yield return req.Send();

        if (req.isError)
            Debug.Log("Error: " + req.error);
        else
            Debug.Log("Received " + req.downloadHandler.text);
    }
}
