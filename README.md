# Reuse your Xamarin.Forms pages in an iOS extension

iOS extensions allow to customize existing system behavior by adding extra functionality to [predefined by iOS and macOS Extension Points](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/index.html#//apple_ref/doc/uid/TP40014214-CH20-SW2), such as custom context actions, password autofill, incoming calls filters, notification content modifiers, and other. Xamarin.iOS supports extensions and [this guide](https://docs.microsoft.com/xamarin/ios/platform/extensions) will walk you through creating an iOS extension using Xamarin tools.

Extensions are distributed as part of a Container app and activated from a specific Extension Point in a Host app. The Container app is usually a simple iOS application, which provides an user with information about the Extension, how to activate and use it. There are three main ways to share code between an Extension and a Container app:

1. Common iOS project.

    You can put all the shared code between the Container and the Extension into a shared iOS library and reference the library from both projects. Usually, the shared library contains native UIViewControllers and it has to be a Xamarin.iOS library.

1. File links.

    In some cases the Container app provides most of the functionality while the Extension needs to render a single `UIViewController`. With few files to share, it's common to add a file link to the Extension app from the file located in the Container app.

1. Common Xamarin.Forms project.

    If your pages are already shared with another platform, such as Android, using Xamarin.Forms framework, the common approach is to reimplement required pages natively in the Extension project, because the iOS Extension works with native UIViewControllers and not Xamarin.Forms pages. You have to perform a few extra steps to use Xamarin.Forms in the iOS Extension and this post explains how you can do that.

## Xamarin.Forms in an iOS Extension project

The ability to use Xamarin.Forms in a native project is provided via [Native Forms](https://docs.microsoft.com/xamarin/xamarin-forms/platform/native-forms). It allows `ContentPage`-derived pages to be added directly to native Xamarin.iOS projects. The `CreateViewController` extension method converts an instance of a Xamarin.Forms page to a native `UIViewController`, which could be used or modified as a regular controller. And because an iOS Extension is a special kind of a native iOS project, you can use the same approach here.

> [!IMPORTANT]
> There are many [known limitations](https://docs.microsoft.com/xamarin/ios/platform/extensions#limitations) for iOS Extensions. Although you can use Xamarin.Forms in an iOS Extension, you should do it very carefully, monitoring memory usage and startup time. Otherwise the Extension could be terminated by iOS without any way to handle this gracefully.

## Walkthrough

In this walkthrough we are going to create a Xamarin.Forms application, a Xamarin.iOS Extension and reuse shared code in the Extension project:

1. Open Visual Studio and create a new Xamarin.Forms project using the **Blank Forms App** template, name it **FormsShareExtension**:

    ![Create Project](/ReadmeItems/1.walkthrough-createproject.png)

1. Open **FormsShareExtension/MainPage.xaml**, replace the content with the following layout:

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

1. Right click on the **FormsShareExtension** project, select **Add > New Class > Empty Class**, name it **MainPageViewModel.cs** and press **Create**. Replace the content of the class with the following code:

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

1. Right click on the solution, select **Add > New Project > iOS > Extension > Action Extension**, name it **MyAction** and press **Create**:

    ![Create Extension](/ReadmeItems/2.walkthrough-createextension.png)

1. In order to use Xamarin.Forms in the iOS Extension and the shared code, we need to add required references:

    - Right click on iOS Extension, select **References > Add Reference > Projects > FormsShareExtension** and press **OK**.

    - Right click on iOS Extension, select **Packages > Manage NuGet Packages... > Xamarin.Forms**  and press **Add Package**.

1. Expand the Extension project and modify an entry point to initialize Xamarin.Forms and create pages. Per iOS requirements, an Extension must define the entry point in **Info.plist** as `NSExtensionMainStoryboard` or `NSExtensionPrincipalClass`. And once the entry point is activated, in our case it is the `ActionViewController.ViewDidLoad` method, we can create an instance of a Xamarin.Forms page and show it to an user. Open the entry point and replace the `ViewDidLoad` method with the following implementation:

    ```csharp
            public override void ViewDidLoad()
            {
                base.ViewDidLoad();

                // Initialize Xamarin.Forms framework
                global::Xamarin.Forms.Forms.Init();
                // Create an instance of XF page with associated View Model
                var xfPage = new MainPage();
                var viewModel = (MainPageViewModel)xfPage.BindingContext;
                viewModel.Message = "Welcome to XF Page created from an iOS Extension";
                // Override the behavior to complete the execution of the Extension when a user press the button
                viewModel.DoCommand = new Command(() => DoneClicked(this));
                // Convert XF page to a native UIViewController which can be consumed by the iOS Extension
                var newController = xfPage.CreateViewController();
                // Present new view controller as a regular view controller
                this.PresentModalViewController(newController, false);
            }
    ```

    The `MainPage` is instantiated using a standard constructor and before you can use it in the Extension, convert it to a native `UIViewController` by using the `CreateViewController` extension method. Build and run the application:

    ![Create Extension](/ReadmeItems/3.walkthrough-runapp.png)

    To activate the Extension, navigate to Safari browser, type in any web address, e.g. [microsoft.com](https://microsoft.com), press navigate and then press the **Share** icon at the bottom of the page to see available action extensions. From the list of available extensions select the **MyAction** Extension by tapping on it:

    ![Create Extension](/ReadmeItems/4.walkthrough-run1.png) ![Create Extension](/ReadmeItems/5.walkthrough-run2.png) ![Create Extension](/ReadmeItems/6.walkthrough-run3.png)

    The Extension is activated and Xamarin.Forms page is displayed to an user. All the bindings and commands work as in the Container app.

1. The original entry point view controller is visible because it is created and activated by iOS. In order to fix that, change the modal presentation style to `UIModalPresentationStyle.FullScreen` for the new controller by adding the following line right before the `PresentModalViewController` call:

    ```csharp
        newController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
    ```

    Build and run in iOS simulator or a device. The demo:

    ![Demo - Xamarin.Forms in iOS Extension](/ReadmeItems/8.walkthrough-result-demo.gif)

    > [!IMPORTANT]
    > For the device build make sure to use proper build settings and the **Release** configuration as [described here](https://docs.microsoft.com/xamarin/ios/platform/extensions#debug-and-release-versions-of-extensions).

## Useful links

- [iOS Extensions in Xamarin.iOS](https://docs.microsoft.com/xamarin/ios/platform/extensions)
- [Xamarin.Forms in Xamarin Native Projects](https://docs.microsoft.com/xamarin/xamarin-forms/platform/native-forms)
- [Optimize Efficiency and Performance of an iOS App Extension](https://developer.apple.com/library/archive/documentation/General/Conceptual/ExtensibilityPG/ExtensionCreation.html#//apple_ref/doc/uid/TP40014214-CH5-SW7)
- [Sample source code](https://github.com/xamcat/xamarin-forms-ios-extension)
