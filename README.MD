# MU Client
Godot version

# Requirement
- Godot Csharp version (latest)
- Dotnet (latest)

# Setup

## Godot Editor
- Open Your Godot Editor Settings...  
    1. General -> Docks -> FileSystem -> Other File Extensions  
    > Add ",map,ozb,ozj,ozt,att,bmd"  
    You must modify this one to show all Client data files inside FileSystem dock
    2. General -> Text Editor -> Behavior -> Indent -> Type  
    > Set as Spaces  
    You must set indent to Spaces to prevent conflic with VS Code editor
## Visual Studio Code
- To Start and Debug this project in VS Code, you must add a new Environment variable.  
For these launch configurations to work, you need to setup a GODOT4 environment variable. On mac or linux, this can be done by adding
the following to your .zshrc, .bashrc, or .bash_profile file:
> export GODOT4="/Applications/Godot.app/Contents/MacOS/Godot"

# Addons

There is only One Addon at this time, its MU Resource Importer  
What do we have at this moment?

## Import Plugins
- BMD importer plugin  
    BMD version 0, 10, 12, 14, 15 are supported  
    It also import animations
- EncTerrain.att import plugin  
    It will import .att file as a "TileAttribute" resource (To read in runtime later)
- EncTerrain.map import plugin
    It will create a Mesh (ArrayMesh) including a ShaderMaterial 
- EncTerrain.obj import plugin
    This plugin will import obj files as custom "ObjectMap" resource
- TerrainHeight.ozb import plugin  
    This plugin will import Terrain Height as Height Map To use inside CollisionShape3D node
- TerrainLight.ozb import plugin  
    This plugin will import TerrainLight as Texture2D format 
- Texture import plugin  
    This plugin will import texture file, example ozt, ozj, ozp.

## Inspector Plugins
- Terrain Attribute Inspector plugin  
    This one will let you inspect Terrain Attribute by type in inspector
