using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllianceMatchSpawner : MonoBehaviour
{
    public GameObject matchPrefab;
    public GameObject dataManagerObject;
    private DataManager dataManager;
    private string filepath;
    private Color white = new Color(0.9764706f, 0.9607843f, 0.9490196f, 1f);
    private Color yellow = new Color(0.9294118f, 0.7568628f, 0.5568628f, 1f);
    private Color red = new Color(0.8901961f, 0.5882353f, 0.5333334f);
    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.persistentDataPath + $"/{PlayerPrefs.GetString("EventKey")}/subj";
        dataManager = dataManagerObject.GetComponent<DataManager>();
        SpawnMatches("");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnMatches(string selective)
    {
        if (transform.parent.childCount > 1)
        {

            for (int i = 1; i < transform.parent.childCount; i++)
            {
                Destroy(transform.parent.GetChild(i).gameObject);
            }
        }
        if (!Directory.Exists(filepath)) { return; }
        foreach (var match in Directory.GetFiles(filepath))
        {
            DataManager.AllianceMatch matchJson = JsonUtility.FromJson<DataManager.AllianceMatch>(File.ReadAllText(match));
            if (selective != "") // Search function in alliance view
            {
                if (!(matchJson.Team1.ToString() == selective || matchJson.Team2.ToString() == selective || matchJson.Team3.ToString() == selective)) { continue; }
            }
            GameObject newMatchCell = matchPrefab;
            /* Background Color for Alliance MatchCell*/
            switch (matchJson.AllianceColor)
            {
                case "Red":
                    newMatchCell.gameObject.GetComponent<RawImage>().color = new Color(0.9176471f, 0.2784313f, 0.3095479f, 1.0f); break;
                case "Blue":
                    newMatchCell.gameObject.GetComponent<RawImage>().color = new Color(0.2800916f, 0.3204849f, 0.9182389f, 1.0f); break;
            }

            childOfCell(0).GetComponent<Text>().text = matchJson.MatchNumber.ToString();
            childOfCell(1).GetComponent<Text>().text = matchJson.MatchType;
            childOfCell(2).GetComponent<Text>().text = $"Data Quality: {matchJson.DataQuality.ToString()}/5";
            childOfCell(3).GetComponent<Text>().text = $"Scouter Name: {matchJson.ScouterName}";
            childOfCell(4).gameObject.SetActive(matchJson.Replay);

            // Team 1 Stats
            childOfCell(5).GetComponent<Text>().text = matchJson.Team1.ToString();
            childOfCell(5).GetChild(0).GetComponent<BarDisplay>().setValue(matchJson.Team1Avoid);
            childOfCell(5).GetChild(0).GetComponent<BarDisplay>().setColor(matchJson.Team1Avoid == 3 ? white : matchJson.Team1Avoid == 2 ? yellow : red);
            childOfCell(5).GetChild(1).GetComponent<BarDisplay>().setValue(matchJson.Team1TravelSpeed);
            childOfCell(5).GetChild(1).GetComponent<BarDisplay>().setColor(matchJson.Team1TravelSpeed == 3 ? white : matchJson.Team1TravelSpeed == 2 ? yellow : red);
            childOfCell(5).GetChild(2).GetComponent<BarDisplay>().setValue(matchJson.Team1AlignSpeed);
            childOfCell(5).GetChild(2).GetComponent<BarDisplay>().setColor(matchJson.Team1AlignSpeed == 3 ? white : matchJson.Team1AlignSpeed == 2 ? yellow : red);

            // Team 2 Stats
            childOfCell(6).GetComponent<Text>().text = matchJson.Team2.ToString();
            childOfCell(6).GetChild(0).GetComponent<Text>().text = $"Avoidance Score {matchJson.Team2Avoid}";
            childOfCell(6).GetChild(0).GetComponent<Text>().color = matchJson.Team2Avoid == 3 ? white : matchJson.Team2Avoid == 2 ? yellow : red;
            childOfCell(6).GetChild(1).GetComponent<Text>().text = $"Travel Speed Score {matchJson.Team2TravelSpeed}";
            childOfCell(6).GetChild(1).GetComponent<Text>().color = matchJson.Team2TravelSpeed == 3 ? white : matchJson.Team2TravelSpeed == 2 ? yellow : red;
            childOfCell(6).GetChild(2).GetComponent<Text>().text = $"Align Speed Score {matchJson.Team2AlignSpeed}";
            childOfCell(6).GetChild(2).GetComponent<Text>().color = matchJson.Team2AlignSpeed == 3 ? white : matchJson.Team2AlignSpeed == 2 ? yellow : red;

            // Team 3 Stats
            childOfCell(7).GetComponent<Text>().text = matchJson.Team3.ToString();
            childOfCell(7).GetChild(0).GetComponent<Text>().text = $"Avoidance Score {matchJson.Team3Avoid}";
            childOfCell(7).GetChild(0).GetComponent<Text>().color = matchJson.Team3Avoid == 3 ? white : matchJson.Team3Avoid == 2 ? yellow : red;
            childOfCell(7).GetChild(1).GetComponent<Text>().text = $"Travel Speed Score {matchJson.Team3TravelSpeed}";
            childOfCell(7).GetChild(1).GetComponent<Text>().color = matchJson.Team3TravelSpeed == 3 ? white : matchJson.Team3TravelSpeed == 2 ? yellow : red;
            childOfCell(7).GetChild(2).GetComponent<Text>().text = $"Align Speed Score {matchJson.Team3AlignSpeed}";
            childOfCell(7).GetChild(2).GetComponent<Text>().color = matchJson.Team3AlignSpeed == 3 ? white : matchJson.Team3AlignSpeed == 2 ? yellow : red;

            childOfCell(8).GetChild(0).GetComponent<Text>().text = $"Auto Center Notes: {matchJson.AutoCenterNotes}";
            childOfCell(8).GetChild(1).GetComponent<Text>().text = $"High Notes: {matchJson.HighNotes}/{matchJson.HighNotePotential}";
            childOfCell(8).GetChild(2).GetComponent<Text>().text = $"Amplify Count: {matchJson.AmplifyCount}";
            childOfCell(8).GetChild(3).GetComponent<Text>().text = $"Team At Amp: {matchJson.TeamAtAmp}";
            childOfCell(8).GetChild(4).GetComponent<Text>().text = $"Bots in Harmony: {matchJson.Harmony}";
            childOfCell(8).GetChild(5).gameObject.SetActive(matchJson.Coopertition);
            childOfCell(8).GetChild(6).GetComponent<Text>().text = $"Fouls: {matchJson.Fouls}";
            childOfCell(9).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = $"Ranking Explanation: {matchJson.RankingComments}\nGeneral Strategy: {matchJson.StratComments}\nOther Comments:{matchJson.OtherComments}";
            childOfCell(10).GetComponent<Text>().text = matchJson.WinMatch ? "WIN" : "LOSS";

            if (SceneManager.GetActiveScene().name == "Rankings")
            {
                newMatchCell.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
            }
            else
            {
                newMatchCell.transform.localScale = new Vector3(0.77f, 0.77f, 0.77f);
            }

            newMatchCell = Instantiate(newMatchCell, transform.parent);

        }
    }

    Transform childOfCell(int i) {
        return matchPrefab.transform.GetChild(i);
    }
}
