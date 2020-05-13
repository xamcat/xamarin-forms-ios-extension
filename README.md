# Reuse your Xamarin.Forms pages in an iOS extension

## Background

iOS extensions allow to customize existing system behavior by adding extra functionality to [predefined by iOS Extension Points](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/index.html#//apple_ref/doc/uid/TP40014214-CH20-SW2) such as custom context actions, password autofill, incoming calls filtering, notification content modifiers, and other. Xamarin.iOS supports extensions and [this guide](https://docs.microsoft.com/xamarin/ios/platform/extensions) will walk you through creating an iOS extension using Xamarin tools.

Extensions are distributed as part of a Container app and activated from a specific Extension Point in a Host app. The Container app is usually a regular iOS application, which provides an user with information about the Extension, how to install and activate it. There are three main ways to share code between an Extension and a Container app:

1. Common iOS project.

    With a shared iOS project you can put all the shared code between the Container and the Extension and reference it from both projects. Usually the shared core contains UIViewControllers and the shared project has to be a Xamarin.iOS library.

1. File links.

    In some cases the Container app provides all the functionality while the Extension need to render single UIViewController. With just few files to share, it's common to add a file link to the Extension app to the file located in the Container app.

1. Copy-paste.

    It's not very common to see developers copy-pasting code from one project to another but in some cases, if your pages are already shared with another platform, such as Android, using Xamarin.Forms platform, the copy-paste seems to be the only option. It happens because iOS Extension expects native UIViewController to display.

This post explains how you can reuse your Xamarin.Forms pages in an Xamarin.iOS Extension without copy-pase.

## Xamarin.Forms in an iOS Extension project

The ability to use Xamarin.Forms in a native project is provided via [Native Forms](https://docs.microsoft.com/xamarin/xamarin-forms/platform/native-forms). It allows `ContentPage`-derived pages to be added directly to native Xamarin.iOS. The `CreateViewController` extension method converts an instance of a Xamarin.Forms page to a native UIViewController, which could be used or extended as a regular controller. And because an iOS Extension is a special kind of a native iOS project, you can you the same technics here.

There are many [known limitations](https://docs.microsoft.com/xamarin/ios/platform/extensions#limitations) for Extensions. Although you can use Xamarin.Forms in an iOS Extension, you should do it very carefully, monitoring memory usage and startup time. Otherwise the Extension can be terminated by iOS without any way to handle this case gracefully.

## Walkthrough

In this walkthrough we are going to create a Xamarin.Forms application, a Xamarin.iOS Extension and reuse shared code within the Extension project.

1. Open Visual Studio and create a new Xamarin.Forms project using the `Blank Forms App` template, name it `FormsShareExtension`:

    ![Create Project](/ReadmeItems/1.walkthrough-createproject.png)

1. Update the main page to include some interactions.
1. Add extension.
1. Init XF - talk about limitations.
1. Create page, convert to the controller and render.
1. Reference back to the main article, warn about limitations and release settings and need to test on a device.

[Screenshots or a running app, GIF better but static image for the docs is required]

## Useful links

- [iOS extensions in Xamarin.iOS](https://docs.microsoft.com/xamarin/ios/platform/extensions)
- [Xamarin.Forms in Xamarin Native Projects](https://docs.microsoft.com/xamarin/xamarin-forms/platform/native-forms)
- [Optimize Efficiency and Performance of an iOS App Extension](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/ExtensionCreation.html#//apple_ref/doc/uid/TP40014214-CH5-SW7)
- [Sample source code](https://github.com/alexeystrakh/xamarin-forms-ios-extension)
