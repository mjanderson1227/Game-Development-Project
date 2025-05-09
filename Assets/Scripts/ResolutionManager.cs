using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private RefreshRate currentRefreshRate;
    private int currentResolutionIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        foreach (var resolution in resolutions)
        {
            if (resolution.refreshRateRatio.Equals(currentRefreshRate))
            {
                filteredResolutions.Add(resolution);
            }
        }

        List<string> options = new List<string>();

        foreach (var (res, index) in filteredResolutions.Select((val, idx) => (val, idx)))
        {
            string resString = $"{res.width}x{res.height} {res.refreshRateRatio} Hz";

            options.Add(resString);
            if (res.width == Screen.width && res.height == Screen.height)
            {
                currentResolutionIndex = index;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
