# Basics of information security in infrastructure systems

## Explain how the app works :
1. Step is to start the _CentralDatabase_ part :
    * This is where the main database is formed.
    * The _CentralDatabase_ and _LocalDatabase_ are connected via WCF.
2. Step is to start the _LocalDatabase_ part :
    * Enter the region for which we want to form a local base.
    * Local database is being formed for the entered region.
    * The _LocalDatabase_ and _Client_ are connected via WCF.
3. Step is to start the _Client_ part :
    * Choose one of the options offered.
    * Depending on the option and the rights that the user has, the action is performed.

## The project contains the following components :
* Common -> class library, that contains common classes and interfaces for WCF
* CentralDatabase -> console application, server that has a main database for the entire application
* LocalDatabase -> console application, server that has a secondary database for specific clients for a given region
* Client -> console application, client who, depending on the role, manipulates data

## Used technologies :
* _Client-Server Architecture_ - implemented in C#
* _Windows Communication Foundation (WCF)_ - to transfer data between client and server
* _Data base_ - is implemented in the form of text files
* _Self-signed certificate_ - is a Chartered Accountant (CA) certificate 
* _Authentication and Authorization_ - Windows authentication and authorization based on RBAC module
* _AES algorithm in CBC mode_ - for encrypting and decrypting data
* _Windows Event Log_ - to save all changes to the database in the form of logs