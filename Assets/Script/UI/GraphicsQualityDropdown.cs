using System.Collections.Generic;
using TMPro; // �� �����ǉ�
using UnityEngine;
using UnityEngine.UI;

public class GraphicsQualityDropdown : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown; // �� TMP_Dropdown �ɕύX

    void Start()
    {
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));

            int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
            qualityDropdown.value = savedQuality;
            qualityDropdown.RefreshShownValue();
        }
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("GraphicsQuality", index);
        PlayerPrefs.Save();
        Debug.Log("Graphics quality set to: " + QualitySettings.names[index]);
    }
}
