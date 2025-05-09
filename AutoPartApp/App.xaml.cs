﻿using System.Windows;
using System.Windows.Input;
using AutoPartApp.Services;

namespace AutoPartApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Set the initial language
        LanguageService.ChangeLanguage(AutoPartApp.Properties.Strings.BulgarianCultureCode);
    }
}