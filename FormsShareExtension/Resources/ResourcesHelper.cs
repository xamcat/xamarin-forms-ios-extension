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

        public static ResourceDictionary LoadAppResources(this ResourceDictionary resourceDictionary)
        {
            resourceDictionary.MergedDictionaries.Clear();
            resourceDictionary.MergedDictionaries.Add(new FormsShareExtension.Resources.Dict1());
            resourceDictionary.MergedDictionaries.Add(new FormsShareExtension.Resources.Dict2());

            return resourceDictionary;
        }

        public static void Apply(this ResourceDictionary resourceDictionary, ContentPage content)
        {
            foreach (var resourceDict in resourceDictionary.MergedDictionaries)
            {
                content.Resources.Add(resourceDict);
            }
        }
    }
}
