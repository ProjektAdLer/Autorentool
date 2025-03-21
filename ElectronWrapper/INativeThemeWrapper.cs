﻿using ElectronSharp.API.Entities;
using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface INativeThemeWrapper
{
    event Action Updated;

    Task<ThemeSourceMode> GetThemeSourceAsync();
    void SetThemeSource(ThemeSourceMode themeSourceMode);
    Task<bool> ShouldUseDarkColorsAsync();
}