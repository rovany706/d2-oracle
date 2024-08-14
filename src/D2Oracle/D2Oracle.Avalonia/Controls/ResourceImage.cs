using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace D2Oracle.Avalonia.Controls;

public class ResourceImage : UserControl
{
    public static readonly StyledProperty<string?> ResourceNameProperty = AvaloniaProperty.Register<ResourceImage, string?>(
        "ResourceName");

    public string? ResourceName
    {
        get => GetValue(ResourceNameProperty);
        set => SetValue(ResourceNameProperty, value);
    }
    
    public sealed override void Render(DrawingContext context)
    {
        if (ResourceName != null)
        {
            var uri = new Uri($"avares://D2Oracle/Assets/{ResourceName}.png");
            var bitmap = LoadFromResource(uri);
            var renderSize = Bounds.Size;
            context.DrawImage(bitmap, new Rect(renderSize));
        }
        
        base.Render(context);
    }
    
    private static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
    }
}