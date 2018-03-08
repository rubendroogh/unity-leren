using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMap : MonoBehaviour {

    [SerializeField]
    private MapBuilder mapBuilder;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Text mainLoadingText;
    [SerializeField]
    private Image loadingBackground;

	void Start () {
        StartCoroutine(LoadingCoroutine());
	}

    public IEnumerator LoadingCoroutine()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    mapBuilder.InitializeMap();
                    loadingText.text = "Initializing map...";
                    break;
                case 1:
                    mapBuilder.CreateBaseLayer();
                    loadingText.text = "Creating base layer...";
                    break;
                case 2:
                    mapBuilder.CreateStructures();
                    loadingText.text = "Creating structures...";
                    break;
                case 3:
                    mapBuilder.CombineTilesInMap();
                    loadingText.text = "Combining tiles...";
                    Destroy(loadingText);
                    Destroy(mainLoadingText);
                    Destroy(loadingBackground);
                    break;
            }
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
