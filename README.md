# Project Osma

This repository is the home of Project Osma, an open source mobile agent for achieving self sovereign identity (SSI).

The primary goals of this project is to provide a common project to progress emerging community standards around mobile agents.

This repository contains a cross platform mobile app (iOS/Android) built using the Xamarin framework in C#. More specifically the two platform specific projects share a common UI through the use of Xamarin.Forms.

## Background

### SSI

SSI is a term coined by Christoper Allen in 2016 with this [article](http://www.lifewithalacrity.com/2016/04/the-path-to-self-soverereign-identity.html), it describes a new paradigm of digital identity. Its premise rests on 10 principles described in the article. In short SSI is about giving a user digital self sovereignty by inverting current approaches to digital identity. Under SSI users are given access and control of their own data and a means in which to use it in a capacity that enables and protects their digital selves.  

### Agents

Agents are essentially software processes that act on behalf of a user and facilitate the usage of their digital identity.

### Standards

There are several key standards in the SSI space but arguably the most important are that of the [DID](https://w3c-ccg.github.io/did-primer/) (as well as other associated specs) and the [Verifiable Credentials](https://w3c.github.io/vc-data-model/) specs. 

## Project Affiliation

### AgentFramework

This mobile apps primary dependency is upon the open source project [AgentFramework](https://github.com/streetcred-id/agent-framework). This framework provides the baseline components for realising agents, Osma extends this framework in the context of a mobile app to realise a mobile agent.

### Indy

Much of the emerging standards Osma and AgentFramework implement are born out of the [Indy-Agent]() community.

## Getting started
1. Clone it locally,
2. Open osma-mobile-app.sln and build!

For more information on the development practises featured in this repository please refer to [here](docs/development.md)


