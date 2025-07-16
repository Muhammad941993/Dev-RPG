using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    private Text _levelText;
    private BaseStats _baseStats;

    void Awake()
    {
        _baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        _levelText = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        _levelText.text = $"{_baseStats.GetLevel():0}";
    }
}
