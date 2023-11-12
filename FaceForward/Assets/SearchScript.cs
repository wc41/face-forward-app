using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class SearchScript : MonoBehaviour
{
    public GameObject toSearch;
    public GameObject Products;
    /*private string searchTerms;*/

    // Start is called before the first frame update
    void Start()
    {
        /*TextMeshProUGUI text = toSearch.GetComponent<TextMeshProUGUI>();
        Debug.Log(text);
        Debug.Log(text.text);
        searchTerms = (string.IsNullOrEmpty(text.text)) ? text.text : "";*/
        /*searchTerms = string.Empty;*/
    }

    // Update is called once per frame
    public void UpdateGrid(string s)
    {
        /*searchTerms = "" + toSearch.GetComponent<TextMeshProUGUI>().text.Trim();*/
        /*searchTerms = (!string.IsNullOrEmpty(toSearch.GetComponent<TextMeshProUGUI>().text.Trim().ToString())) ? toSearch.GetComponent<TextMeshProUGUI>().text.ToString() : "";
        Debug.Log(string.IsNullOrEmpty(toSearch.GetComponent<TextMeshProUGUI>().text.Trim().ToString()));*/
        /*Debug.Log(toSearch.GetComponent<TextMeshPro>());
        searchTerms = toSearch.GetComponent<TextMeshProUGUI>().text;*/
        /*Debug.Log(searchTerms);*/
        /*string search = searchTerms;*/
        for (int i = 0; i < Products.transform.childCount; i++)
        {
            bool found = false;
            GameObject Product = Products.transform.GetChild(i).gameObject;
            for (int j = 0; j < Product.transform.childCount; j++)
            {
                GameObject ProductProp = Product.transform.GetChild(j).gameObject;
                if (ProductProp.name.Equals("BrandName") || ProductProp.name.Equals("ProductName"))
                {
                    /*Debug.Log(ProductProp.GetComponent<TextMeshProUGUI>().text);*/
                    /*Debug.Log(ProductProp.GetComponent<TextMeshProUGUI>().text.Contains(searchTerms));*/
                    string lookThrough = ProductProp.GetComponent<TextMeshProUGUI>().text.Trim();
                    /*Debug.Log("Text Content: " + lookThrough);
                    Debug.Log("Search: " + searchTerms);
                    Debug.Log(lookThrough.IndexOf(s, StringComparison.OrdinalIgnoreCase) > 0);
                    Debug.Log(lookThrough.IndexOf(s));*/
                    if (lookThrough.Contains(s, StringComparison.OrdinalIgnoreCase) || lookThrough.IndexOf(s) > 0) {
                        found = true;
                    }
                }
            }
            if (found) { Product.SetActive(true); }
            else { Product.SetActive(false); }
        }
    }
}
