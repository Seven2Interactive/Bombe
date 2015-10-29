# Bombe
A set of Unity3d Helper classes to make life easier.


In general, this is a port of some utility functions from the Haxe Flambe framework.
https://github.com/aduros/flambe


## Usage
Simply add the Bombe folder anywhere in the Assets folder, or any subfolder of your choosing. 
All code is written inside the Bombe namespace, so make sure to add:

```C#
using Bombe;
```


## Extras
The BombeExtras folder is there for tying into other 3rd party libraries. 
The folder structure used for this is BombeExtras/[BombeFolderName]/[3rdPartyName]/

### Example
Spine:
Currently there's a PlaySpine IAction in there which adds Script functionality to play a SpineAnimation.
It's set in the BombeExtras/Script/Spine

All extra functionality is still inside the Bombe namespace for ease of use. 

You can either drop the entire BombeExtras folder into your project or copy in the 3rd party libraries you need.