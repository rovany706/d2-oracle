// Source https://community.devexpress.com/blogs/wpf/archive/2022/02/07/dependency-injection-in-a-wpf-mvvm-application.aspx

using System;
using System.Windows.Markup;

namespace D2Oracle.WPF;

public class DISource : MarkupExtension
{
    public static Func<Type, object> Resolver { get; set; }
    public Type Type { get; set; }
    public override object ProvideValue(IServiceProvider serviceProvider) => Resolver?.Invoke(Type);
}