# Reuse your Xamarin.Forms pages in an iOS extension

## Intro

1. The current approach to implement extensions - native.
1. Either shared via the common iOS project, or linked files or copy-pasted.
1. Inability to share your XF code with extensions.

## Solution

XF native support via CreateViewController()

## Walkthrough

1. Create a project.
1. Update the main page to include some interactions.
1. Add extension.
1. Init XF - talk about limitations.
1. Create page, convert to the controller and render.
1. Reference back to the main article, warn about limitations and release settings and need to test on a device.

[Screenshots or a running app, GIF better but static image for the docs is required]

## Useful links

- [iOS extensions in Xamarin.iOS](https://docs.microsoft.com/xamarin/ios/platform/extensions)
- [Optimize Efficiency and Performance of an iOS App Extension](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/ExtensionCreation.html#//apple_ref/doc/uid/TP40014214-CH5-SW7)
