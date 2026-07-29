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
        else
            panel.SetActive(true);
        if (!PlayerPrefs.HasKey("uuid"))
            PlayerPrefs.SetString("uuid", guid);

        // Wired here instead of as an Inspector-persistent UnityEvent listener: IL2CPP's
        // stripped WebGL builds crashed ("function signature mismatch") invoking a
        // parameterless listener off TMP_InputField.onEndEdit (a UnityEvent<string>) -
        // a runtime AddListener call doesn't go through that Invoker mechanism.
        nameField.onEndEdit.AddListener(_ => RegisterAccount());
    }

    public void RegisterAccount() {
        PlayerPrefs.SetString("name", nameField.text);
        api.RegisterTemporaryAccount();
        panel.SetActive(false);
    }
}
