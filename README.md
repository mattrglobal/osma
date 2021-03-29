# ARCHIVE NOTICE

OSMA has joined the [Hyperledger](https://www.hyperledger.org/) family under the [Aries project](https://www.hyperledger.org/projects/aries), for further interest in this repository please refer to [here](https://github.com/hyperledger/aries-mobileagent-xamarin).

# Project Osma

This repository is the home of Project Osma, an open source mobile agent for achieving self sovereign identity (SSI).

The primary goals of this project is to provide a common project to progress emerging community standards around mobile agents.

This repository contains a cross platform mobile app (iOS/Android) built using the Xamarin framework in C#. More specifically the two platform specific projects share a common UI through the use of Xamarin.Forms.

## Background

### SSI (Self Sovereign Identity)

SSI is a term first coined by Devon Loffreto in 2010 symbolizing a new paradigm for digital identity, described at this point in time as "individual human identity as the origin of source authority.". In 2016 Christopher Allen expanded on this concept by writting [this article](http://www.lifewithalacrity.com/2016/04/the-path-to-self-soverereign-identity.html) citing 10 key principles. In short SSI is about giving a user digital self sovereignty by inverting current approaches to digital identity. Under SSI users are given access and control of their own data and a means in which to use it in a capacity that enables and protects their digital selves.

### Agents

Agents are essentially software processes that act on behalf of a user and facilitate the usage of their digital identity.

### Standards

There are several key standards in the SSI space but arguably the most important are that of the [DID](https://w3c-ccg.github.io/did-primer/) (as well as other associated specs) and the [Verifiable Credentials](https://w3c.github.io/vc-data-model/) specs.

## Project Affiliation

### Aries Framework Dotnet

This mobile apps primary dependency is upon the open source project [Aries Framework Dotnet](https://github.com/hyperledger/aries-framework-dotnet). This framework provides the baseline components for realising agents, Osma extends this framework in the context of a mobile app to realise a mobile agent.

### Aries

Much of the emerging standards Osma and Aries Framework implement are born out of the [Aries](https://github.com/hyperledger/aries-rfcs) community.

## Getting started

1. Clone it locally,
2. Run `git lfs pull` in order to pull the dependent native libraries with LFS. If you do not have this installed please refer to [here](docs/development.md)
3. Open osma-mobile-app.sln and build!

For more information on the development practises featured in this repository please refer to [here](docs/development.md)

## A Quick Demo

The following demo describes how you can connect with another agent.

1. Clone [Aries Framework Dotnet](https://github.com/hyperledger/aries-framework-dotnet)
2. From the `/scripts` folder in the repository run `./start-web-agents.sh` - Note this shell script relys on [ngrok](https://ngrok.com/) to run the agents on a publically accessable addresses. Please ensure that your ngrok version is 2.3.28 or higher. You may get an error if the port stated is protected or in use - curl: (7) Failed to connect to localhost port <port>: Connection refused. If this happens simply scan for an open port and change the port number for web_addr in `/scripts/web-agents-ngrok-config.yaml` and also on line number 6 of `/scripts/start-web-agents.sh`. For Macbook open network utility to scan ports.
3. Note the public URL's that are outputed by the script with the following text `Starting Web Agents with public urls http://... http://...`
4. Browse to one of the urls noted above.
5. In the rendered UI, click `Connections`->`Create Invitation`, a QR code should be displayed.
6. In the osma mobile app, deployed to a mobile phone, click the connect button in the top right corner of the connections tab.
7. Scan the QR code rendered in the browser with the osma mobile app and click connect in the rendered UI.
8. Congrats, you should now be redirected in the mobile app back to the connections page showing a new connection with the AgentFramework web agent!
