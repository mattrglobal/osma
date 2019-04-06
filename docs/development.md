# Development

The following document describes several conventions and technologies adopted by this project to facilitate ease of development.

## Gorilla Player UI Development

The Xamarin.Forms project is setup to support [Gorilla Player](https://grialkit.com/gorilla-player/) as a means by which quick UI previewing can occur. To get started click [here](https://github.com/UXDivers/Gorilla-Player-Support/wiki/Getting-Started).

To harness the full power of Gorilla Player the application must be built with Gorilla Players SDK enabled, in order to resolve any third party UI assemblies. To do this, select the Gorilla build configuration when building the app to a device.

Design time data in Gorilla player allows for the quick preview of populated views, this data is controlled via the `DesignTimeData.json` file located at the root of the Poc.Mobile.App folder.

Any other advanced settings associated to Gorilla Player and this app can be configured via the `Gorilla.json` also located at the root of the Poc.Mobile.App folder.

[General Wiki link](https://github.com/UXDivers/Gorilla-Player-Support/wiki)

## LFS
[Git LFS](https://git-lfs.github.com/) is an extension and specification for managing large files with Git. With Git LFS installed, pushing and pulling should work as normal.

Note git lfs is leveraged in this project to manage the c-callable library dependencies.

When you first clone the repository, depending on the version of your git client you may also need to call `git lfs pull` in order to pull the libraries managed by git lfs.

## Architecture 

This project adopts the [MVVM](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) architecture pattern. Using [DI](https://en.wikipedia.org/wiki/Dependency_injection) for dependency management. The core of this app resides in `src/Poc.Mobile.App` which is broken down as follows

    ├── Poc.Mobile.App              # Source folder
    │   ├── Behaviours              # Location of any custom behaviours for view elements resides.
    │   ├── Converters              # Location of any custom converters for view elements or other components resides.
    │   ├── Extensions              # Location of any extensions to core components.
    │   ├── Services                # Location of any shared services coupled to Xamarin.Forms (Note - all other shared services reside in Poc.Mobile.App.Services). 
    │   ├── Utilites                # Location of any custom utilities for view elements.
    │   ├── ViewModels              # Location of the view models.
    └───└── Views                   # Location of the views.

Note - As mentioned above any shared services that are not bound to Xamarin.Forms, should be located in `src/Poc.Mobile.App.Services`. Platform specific services should be located in their respective projects, with the common contract or interface defining them located in the either `src/Poc.Mobile.App` or `src/Poc.Mobile.App.Services`. Invoking the platform specific service for each platform should be managed via the `PlatformModule.cs` defined in each platform specific project

## XAML View Conventions

This project uses Xamarin.Forms and more specifically the view elements are defined in `XAML` rather than as the `code-behind` approach. See [here](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/creating-mobile-apps-xamarin-forms/summaries/chapter07) for a comparison. Below defines several conventions adopted by this project to manage the design of the views.

### Pages
#### Format
`{Name}Page`

#### Example
`ExamplePage`
```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage>
    <ContentPage.Content>
        <Label Text="A lovely page" />
    </ContentPage.Content>
</ContentPage>
```

### ItemView

 Item views are reusable blocks. Used most commonly in ListViews.
 Context is shared with their parent.

#### Format
`{Name}ItemView`

Using `ExampleItemView.xaml` in a `ListView`
```xml
<ListView ItemsSource="{Binding ItemsSource}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <views:ExampleItemView/>  <!-- Current context shared -->
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```
`ExampleItemView.xaml`
```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentView>
  <ContentView.Content>
      <Label Text="{Binding ItemText}" />
    </ContentView.Content>
</ContentView>
```

### Controls

Controls are reusable components that are used across multiple views, they do not directly look at context.

#### Format

`{Name}Control`

#### Example
```xml
<ExampleControl ExampleAttribute="Test"/>
```