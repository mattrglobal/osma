# POC Mobile App

This repository contains a cross platform mobile app (iOS/Android) built using the Xamarin framework in C#. More specifically the two platform specific projects share a common UI through the use of Xamarin.Forms.

The app is a digital credential wallet, more specifically regarded as an `Edge Agent` in the Indy ecosystem.

This app depends upon the open source [AgentFramework](https://github.com/streetcred-id/agent-framework).

## Getting started
1. Clone it locally,
2. Open poc-mobile-app.sln and build!

For more information on the development practises featured in this repository please refer to [here](docs/development.md)

## Project Structure

    ├── docs                        # Docs folder
    ├── src                         # Source folder
    │   ├── poc-mobile-app.sln      # Main application solution file.
    │   ├── Poc.Mobile.App          # Location of Xamarin.Forms App shared between the two platforms invoked by each platform.
    │   ├── Poc.Mobile.App.Android  # Location of the Android platform app.
    │   ├── Poc.Mobile.App.iOS      # Location of the iOS platform app.
    └───└── Poc.Mobile.App.Services # Location of the shared services project, containing re-usable, platform agnostic services. 