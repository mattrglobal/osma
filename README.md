# POC Mobile App

This repository contains a cross platform mobile app (iOS/Android) built using the Xamarin framework in C#. More specifically the two platform specific projects share a common UI through the use of Xamarin.Forms.

The app is a digital credential wallet, more specifically regarded as an `Edge Agent` in the Sovrin ecosystem.

## Project Structure

    ├── src                         # Source folder
    │   ├── poc-mobile-app.sln      # Main application solution file.
    │   ├── Poc.Mobile.App          # Location of Xamarin.Forms App shared between the two platforms invoked by each platform.
    │   ├── Poc.Mobile.App.Android  # Location of the Android platform app.
    │   ├── Poc.Mobile.App.iOS      # Location of the iOS platform app.
    └───└── Poc.Mobile.App.Services # Location of the shared services project, containing re-usable, platform agnostic services. 

##Gorilla Player UI Development

The Xamarin.Forms project is setup to support [Gorilla Player](https://grialkit.com/gorilla-player/) as a means by which quick UI previewing can occur. To get started click [here](https://github.com/UXDivers/Gorilla-Player-Support/wiki/Getting-Started).

To harness the full power of Gorilla Player the application must be built with Gorilla Players SDK enabled, in order to resolve any third party UI assemblies. To do this, select the Gorilla build configuration when building the app to a device.

Design time data in Gorilla player allows for the quick preview of populated views, this data is controlled via the DesignTimeData.json file located at the root of the Poc.Mobile.App folder.

Any other advanced settings associated to Gorilla Player and this app can be configured via the Gorilla.json also located at the root of the Poc.Mobile.App folder.

[General Wiki link](https://github.com/UXDivers/Gorilla-Player-Support/wiki)

## LFS
[Git LFS](https://git-lfs.github.com/) is an extension and specification for managing large files with Git. With Git LFS installed, pushing and pulling should work as normal.


## Structure & Naming of XAML

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
