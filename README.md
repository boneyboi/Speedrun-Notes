# Speedrun-Notes
TL;DR: A note editor component for LiveSplit

# Credits
Special Thanks to Furman and the Summer Research Fellowship program through the Center for Engaged Learning for funding this project!

### Description
Speed Run Notes is a note editor that displays text, images, and controller symbols for each split in your livesplit run.

### Features
  - Detatched display that is customizeable
  - Display shows notes for each split automatically when running
  - Import and Export functionality that will export notes into a folder
  - Auto Load feature (only for notes that were previously exported)
  - Rich Text note editor
  - Image support
  - Controller symbol support for:
    - Keyboard
    - Microsoft (Xbox)
    - Playstation
    - Nintendo
  - Controller detection support for Xbox only
  
### Necessary . dlls
  - To use Speed Run notes it is neccessary that the sharpDx .dlls be included with the component's .dll
  - If you want to build Speedrun notes, include the LiveSplit.Core.dll, UpdateManager.dll, and WinFormsColor.dll in the root folder
    - Also in visual studio install the nuget package for SharpDX
  
### How to install
  - Install all dependencies
  - Move all dependencies to the components folder
  - Move this component's .dll to the components folder

### How to build // compiling tutorial
  - Requirements:
    - Visual Studio
    - Install the Sharpdx Nuget (.dlls will appear in the root\bin\debug folder)
    - LiveSplit.Core.dll, WinFormsColor.dll, UpdateManager.dll (put them in the root of the project folder)
      - these can all be found in the livesplit folder
  - once opened in visual studio and all requirements have been added and referenced then it is possible to build the component
    

### Getting Started
  - Install all dependencies
  - Move all dependencies to the components folder
  - Move this component's .dll to the components folder
  - Open LiveSplit
  - Add the Speed Run Notes component under the other tab
  - To edit ntoes for a split: open the settings and click "Open Note Editor"
  - To export or import notes: open the settings and click export or import
  - To reset or make a new set of notes:
    - subtract the speed run notes component currently in use
    - then click ok in the settings
    - then add another speed run notes component
  - To use the fonts for controller symbols, go to the resources folder and install the fonts
