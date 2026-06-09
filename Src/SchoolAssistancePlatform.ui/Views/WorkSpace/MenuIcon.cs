using System;

using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace SchoolAssistancePlatform.UI.Views.WorkSpace;

internal static class MenuIcon
{
    public static Bitmap? Load(string assetPath)
    {
        try
        {
            var uri = new Uri(assetPath);
            using var stream = AssetLoader.Open(uri);
            return new Bitmap(stream);
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}
