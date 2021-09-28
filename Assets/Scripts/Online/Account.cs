using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TMPro;

public class Account : MonoBehaviour
{
    [SerializeField]
    private string guid;

    public TMP_InputField nameField;
    public ApiService api = new ApiService();
    public GameObject panel;
    // Start is called before the first frame update
    void Awake()
    {

        Guid g = Guid.NewGuid();
        guid = Guid.NewGuid().ToString();
        if (PlayerPrefs.HasKey("name"))
            panel.SetActive(false);
        if (!PlayerPrefs.HasKey("uuid"))
            PlayerPrefs.SetString("uuid", guid);
    }

    public void RegisterAccount() {
        PlayerPrefs.SetString("name", nameField.text);
        api.RegisterTemporaryAccount();
        panel.SetActive(false);
    }
}
