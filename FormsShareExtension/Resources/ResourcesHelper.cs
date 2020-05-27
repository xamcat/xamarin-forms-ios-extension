using System;
using Xamarin.Forms;

namespace FormsShareExtension.Resources
{
    public static class ResourcesHelper
    {
        public static ResourceDictionary LoadAppResources()
        {
            var resources = new ResourceDictionary();
            return resources.LoadAppResources();
        }

        public static ResourceDictionary LoadAppResources(this ResourceDictionary source)
        {
            source.MergedDictionaries.Clear();
            source.MergedDictionaries.Add(new FormsShareExtension.Resources.Colors1());
            source.MergedDictionaries.Add(new FormsShareExtension.Resources.Dict1());
            source.MergedDictionaries.Add(new FormsShareExtension.Resources.Dict2());

            return source;
        }

        public static void Apply(this ResourceDictionary source, ResourceDictionary target)
        {
            foreach (var resourceDict in source.MergedDictionaries)
            {
                target.Add(resourceDict);
            }
        }

        public static void Apply(this ResourceDictionary source, ContentPage target)
        {
            foreach (var resourceDict in source.MergedDictionaries)
            {
                target.Resources.Add(resourceDict);
            }
        }
    }
}
