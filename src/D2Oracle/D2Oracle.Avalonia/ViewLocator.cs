using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using D2Oracle.Core.ViewModels;

namespace D2Oracle.Avalonia;

public class ViewLocator : IDataTemplate
{
    public Control Build(object data)
    {
        var name = data.GetType().Name!.Replace("ViewModel", "View");
        var type = Assembly.GetEntryAssembly()!.GetTypes().SingleOrDefault(x => x.Name == name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}