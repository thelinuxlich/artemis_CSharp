![Artemis Build](https://api.travis-ci.org/thelinuxlich/artemis_CSharp.png)

This is a complete C# port and an extension of the awesome Entity System framework Artemis,
originally available in Java here: https://github.com/tescott/artemis

Available on Nuget: http://nuget.org/packages/Artemis

Regular Stable Versions: https://github.com/thelinuxlich/artemis_CSharp/tags

Documentation: http://thelinuxlich.github.com/artemis_CSharp/

StarWarrior example game here to get used to the framework:
https://github.com/thelinuxlich/starwarrior_CSharp

If you have any suggestions, critics, we'd love to hear!
Our forum: http://www.ploobs.com.br/forum/viewforum.php?f=39&sid=ac84dca015138021f78da3b200ef5f96

Development, Please:
+ use (free) StyleCop to hold up readability and quality of code. http://stylecop.codeplex.com/ 
  In combination with jetbrains R# http://www.jetbrains.com/resharper/ it makes your life much easier.
+ document at least all visible parts of your code.
  Use of GhostDoc http://submain.com/products/ghostdoc.aspx is highly recommended.
+ program against the interface, not the implementation and help us to improve code:
  http://oreilly.com/catalog/pnetcomp2/chapter/ch03.pdf
+ add unit tests if you implement new functionality.
+ avoid Linq on speed critical parts.
+ avoid Linq with ".Any" statements always.
+ bear in mind that any release should run on multiple platforms.
  If you do not have a platform, ask us for help!

Visual Studio 2010 (Windows 7 and older):
* Use ArtemisEntitySystemPcPhoneXboxVS2010 to check compatibility with and build
  (use "Batch build..."->"Select all") the library files for PC, Windows Phone 7 and XBOX 360.
  You find the build library in root\bin\ folder.
* Use ArtemisEntitySystemPcVS2010 to develop on Entity System and Unit-tests only.
* Requirements:
  * Windows 7 (Up to date inclusive newest SP)
  * Visual Studio 2010 SP1
  * .net framework 4.0 (client)
  * XNA GameStudio 4.+ (for xbox and windows phone 7 only)

Visual Studio 2012 (Windows 8 and higher):
* Use ArtemisEntitySystemPcStorePortableVS2012 to check compatibility with and build 
  (use "Batch build..."->"Select all") the library files for WindowsStore and Windows Portable.
  You find the build library in root\bin\ folder.
* Use ArtemisEntitySystemPcVS2012 to develop on Entity System and Unit-tests only.
* Requirements:
  * Windows 8 (Up to date inclusive newest SP)
  * Visual Studio 2012 (Express not supported)
  * .net framework 4.5 (client)
  * Windows 8 SDK

Supported Platforms:
* Pc - .Net framework 4 and higher
* Windows Store Apps (Metro)
* Silverlight 3 and higher (Portable Solution)
* Windows Phone 7 / 8
* Xbox
* Mono (Linux/Android/OSx)
