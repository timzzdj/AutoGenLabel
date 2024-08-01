#420201086011MATDJ
******************************************************************************************************************************************
SDK Loftware/Nicelabel Reference must be updated:
================================================= 
In General this can be found in C:\Program Files\Loftware\Loftware\bin.net\SDK.NET.dll or SDK.NET.Interface.dll

Labels, PDFs, and ArenaLogin Folders Reference should be reviewed:
============================================================
MainWindow.xaml.cs references Labels and PDFs folders within the bin/Debug/Labels or PDFs File Path 
(either copy the Labels/PDFs folder into that file path or update the Relative File Path Code).

ArenaAPI.cs references ArenaLogin folder within the bin/Debug/ArenaLogin File Path
(either copy the ArenaLogin folder into that file path or update the Relative File Path Code).

ArenaCredentialsLogin.txt could be reworked:
============================================
Since writing the document, AutoGenLabel project takes the credentials from ArenaCredentialsLogin.txt to process user login to Arena.
This could be reworked to take in login credentials from the user directly as a pop up window to prevent potential security issues by storing passwords in plaintext.
For the purpose of testing this application, reading the credentials from plain text was used. (This does not affect security when logging into Arena servers, due to https protocol).

Loftware/Nicelabel Image issues:
================================
Labels: Unit Label Format 2.25 x 2.00.nlbl and Unit Label Format 4.00 x 1.33.nlbl
These two labels outputs an error when loading in the Image Reference. (These labels may not print due to this issue).