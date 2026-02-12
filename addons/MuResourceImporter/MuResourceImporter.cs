#if TOOLS
using Godot;
using MuClient.addons.MuResourceImporter.InspectorPlugins;
using MuClient.addons.MuResourceImporter.Plugins;
using System;

namespace MuClient.addons.MuResourceImporter;

[Tool]
public partial class MuResourceImporter : EditorPlugin
{

	EncTerrainObjImportPlugin encTerrainObjImportPlugin;
	TerrainHeightImportPlugin terrainHeightImportPlugin;
	TerrainLightImportPlugin terrainLightImportPlugin;
	TextureImportPlugin textureImportPlugin;
	EncTerrainAttImportPlugin encTerrainAttImportPlugin;

	EncTerrainAttInspectorPlugin encTerrainAttInspectorPlugin;
	EncTerrainMapImportPlugin encTerrainMapImportPlugin;
	BmdModelImportPlugin bmdModelImportPlugin;
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		encTerrainObjImportPlugin = new();
		AddImportPlugin(encTerrainObjImportPlugin);

		terrainHeightImportPlugin = new();
		AddImportPlugin(terrainHeightImportPlugin);

		terrainLightImportPlugin = new();
		AddImportPlugin(terrainLightImportPlugin);

		textureImportPlugin = new();
		AddImportPlugin(textureImportPlugin);

		encTerrainAttImportPlugin = new();
		AddImportPlugin(encTerrainAttImportPlugin);

		encTerrainAttInspectorPlugin = new();
		AddInspectorPlugin(encTerrainAttInspectorPlugin);

		encTerrainMapImportPlugin = new();
		AddImportPlugin(encTerrainMapImportPlugin);

		bmdModelImportPlugin = new();
		AddImportPlugin(bmdModelImportPlugin);

	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.

		RemoveImportPlugin(encTerrainObjImportPlugin);

		RemoveImportPlugin(terrainHeightImportPlugin);

		RemoveImportPlugin(terrainLightImportPlugin);

		RemoveImportPlugin(textureImportPlugin);

		RemoveImportPlugin(encTerrainAttImportPlugin);

		RemoveInspectorPlugin(encTerrainAttInspectorPlugin);

		RemoveImportPlugin(encTerrainMapImportPlugin);

		RemoveImportPlugin(bmdModelImportPlugin);
	}
}
#endif
