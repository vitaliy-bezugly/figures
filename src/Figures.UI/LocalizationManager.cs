using System;
using System.Collections.Generic;
using System.Windows;

namespace Figures.UI;

public class LocalizationManager
{
    private readonly ResourceDictionary _resources;
    private readonly List<string> _supportedLanguages = new() {"en-US", "uk-UA"};

    public LocalizationManager(ResourceDictionary resources)
    {
        _resources = resources;
    }

    public void InitializeDefaultLanguage()
    {
        SwitchLanguage("en-US");
    }
    
    public void SwitchLanguage(string language)
    {
        if(!_supportedLanguages.Contains(language))
            throw new ArgumentException("Language is not supported");
        
        var dictionary = new ResourceDictionary
        {
            Source = new Uri($"..\\Resources\\Resources.{language}.xaml", UriKind.Relative)
        };

        _resources.MergedDictionaries.Add(dictionary);
    }
    
    public string GetLocaleStringByKey(string key)
    {
        var value = _resources[key] as string;
        if (value is null)
            throw new ArgumentException("Value not found. Invalid key");
        
        return value;
    }
}