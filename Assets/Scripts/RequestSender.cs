using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestSender : MonoBehaviour
{
    private Queue<Request> _requestQueue = new Queue<Request>();
    private Request _tempRequest;
    private const string _authHeaderName = "Authorization";
    private const string _authKey = "Basic BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";
    private void Start()
    {
        StartCoroutine(Upload());
    }

    public void SendAddRequest(int id)
    {
        _tempRequest = new Request(ActionType.Add, id);
        _requestQueue.Enqueue(_tempRequest);
    }

    public void SendRemoveRequest(int id)
    {
        _tempRequest = new Request(ActionType.Remove, id);
        _requestQueue.Enqueue(_tempRequest);
    }

    private IEnumerator Upload()
    {
        while (true)
        {
            if (_requestQueue.Count > 0)
            {
                var request = _requestQueue.Dequeue();
                using (UnityWebRequest www = UnityWebRequest.Post("https://dev3r02.elysium.today/inventory/status", request.GetData()))
                {
                    www.SetRequestHeader(_authHeaderName, _authKey);

                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.LogError(www.error);
                    }
                    else
                    {
                        Debug.Log("Form upload complete!");
                    }
                }
            }

            yield return null;
        }
    }
}

public enum ActionType
{
    Add = 0,
    Remove = 1
}

public struct Request
{
    public ActionType ActionType { private set; get; }
    public int ID { private set; get; }

    public Request(ActionType actionType, int id)
    {
        ActionType = actionType;
        ID = id;
    }

    public string GetData()
    {
        return $"{ActionType.ToString()};{ID}";
    }
}
