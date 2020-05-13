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

In this walkthrough we are going to create a Xamarin.Forms application, a Xamarin.iOS Extension and reuse shared code within the Extension project:

1. Open Visual Studio and create a new Xamarin.Forms project using the `Blank Forms App` template, name it `FormsShareExtension`:

    ![Create Project](/ReadmeItems/1.walkthrough-createproject.png)

1. Open `FormsShareExtension/MainPage.xaml`, replace the content with the following layout:

    ```xaml
    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage
        x:Class="FormsShareExtension.MainPage"
        xmlns="http://xamarin.com/schemas/2014/forms"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        xmlns:d="http://xamarin.com/schemas/2014/forms/design"
        xmlns:local="clr-namespace:FormsShareExtension"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        x:DataType="local:MainPageViewModel"
        BackgroundColor="Orange"
        mc:Ignorable="d">
        <ContentPage.BindingContext>
            <local:MainPageViewModel Message="Hello from Xamarin.Forms!" />
        </ContentPage.BindingContext>
        <StackLayout HorizontalOptions="Center" VerticalOptions="Center">
            <Label
                Margin="20"
                Text="{Binding Message}"
                VerticalOptions="CenterAndExpand" />
            <Button Command="{Binding DoCommand}" Text="Do the job!" />
        </StackLayout>
    </ContentPage>
    ```

1. Right click on the `FormsShareExtension` project, add `Add` > `New Class` > `Empty Class`, name it `MainPageViewModel` and press `Create`. Replace the content of the class with the following code:

    ```csharp
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    namespace FormsShareExtension
    {
        public class MainPageViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _message;
            public string Message
            {
                get { return _message; }
                set
                {
                    if (_message != value)
                    {
                        _message = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                    }
                }
            }

            private ICommand _doCommand;
            public ICommand DoCommand
            {
                get { return _doCommand; }
                set
                {
                    if(_doCommand != value)
                    {
                        _doCommand = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoCommand)));
                    }
                }
            }

            public MainPageViewModel()
            {
                DoCommand = new Command(OnDoCommandExecuted);
            }

            private void OnDoCommandExecuted(object state)
            {
                Message = $"Job {Environment.TickCount} has been completed!";
            }
        }
    }
    ```

    The code is shared across all platforms and will be used by an iOS Extension as well.

1. Right click on the solution, select `Add` > `New Project` > `iOS` > `Extension` > `Action Extension`, name it `MyAction` and press **Create**:

    ![Create Extension](/ReadmeItems/2.walkthrough-createextension.png)

1. In order to use Xamarin.Forms in the iOS Extension and the shared code, we need to add required references:

    - Right click on iOS Extension `References` > `Add Reference` > `Projects` > `FormsShareExtension` and press **OK**.

    - Right click on iOS Extension `Packages` > `Manage NuGet Packages...` > `Xamarin.Forms`  and press **Add Package**.

1. Expand the extension project and modify an entry point to initialize Xamarin.Forms and create pages. Per iOS requirements, an Extension must define the entry point in **Info.plist** as `NSExtensionMainStoryboard` or `NSExtensionPrincipalClass`. And once the entry point is activated, in our case it is the `ActionViewController.ViewDidLoad` method, we can create instance of a Xamarin.Forms page and show it to an user. Open the entry point and replace the `ViewDidLoad` method with the following implementation:

    ```csharp
            public override void ViewDidLoad()
            {
                base.ViewDidLoad();

                // Initialize Xamarin.Forms framework
                global::Xamarin.Forms.Forms.Init();
                // Create an instance of XF page with associated View Model
                var xfPage = new MainPage();
                var viewModel = (MainPageViewModel)xfPage.BindingContext;
                viewModel.Message = "Welcome to XF Page created from an iOS extension";
                // Override the behavior to complete the execution of the extension when a user press the button
                viewModel.DoCommand = new Command(() => DoneClicked(this));
                // Convert XF page to a native UIViewController which can be consumed by the iOS Extension
                var newController = xfPage.CreateViewController();
                // Present new view controller as a regular view controller
                this.PresentModalViewController(newController, false);
            }
    ```

    Build and run the application.

    ![Create Extension](/ReadmeItems/3.walkthrough-runapp.png)

    Activate the Extension, navigate to Safari browser, type in any web address, e.g. [microsoft.com](https://microsoft.com) press navigate and then press the **Share** icon at the bottom of the page to see available action extensions. From the list of available extensions select the MyAction Extension by tapping on it:

    ![Create Extension](/ReadmeItems/4.walkthrough-run1.png)  

    ![Create Extension](/ReadmeItems/5.walkthrough-run2.png)

    ![Create Extension](/ReadmeItems/6.walkthrough-run3.png)

1. The original entry point view controller is visible because it is created and activated by iOS. In order to fix that, change the modal presentation style to **FullScreen** for the new controller by adding the following like right before the `PresentModalViewController` call:

    ```csharp
        newController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
    ```

    Build and run in iOS Simulator. For the device build make sure you use proper build settings and the **Release** configuration as [described here](https://docs.microsoft.com/xamarin/ios/platform/extensions#debug-and-release-versions-of-extensions). The demo:

    ![Demo - Xamarin.Forms in iOS Extension](/ReadmeItems/8.walkthrough-result-demo.gif)

## Useful links

- [iOS extensions in Xamarin.iOS](https://docs.microsoft.com/xamarin/ios/platform/extensions)
- [Xamarin.Forms in Xamarin Native Projects](https://docs.microsoft.com/xamarin/xamarin-forms/platform/native-forms)
- [Optimize Efficiency and Performance of an iOS App Extension](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/ExtensionCreation.html#//apple_ref/doc/uid/TP40014214-CH5-SW7)
- [Sample source code](https://github.com/alexeystrakh/xamarin-forms-ios-extension)
