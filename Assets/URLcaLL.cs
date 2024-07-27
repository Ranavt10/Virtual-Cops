using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class URLcaLL : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();

    async void Start()
    {
        string url = "https://jsonplaceholder.typicode.com/posts";
        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Debug.Log(responseString);
           
        }
        else
        {
            Debug.Log("Error: " + response.StatusCode);
        }
    }
}
