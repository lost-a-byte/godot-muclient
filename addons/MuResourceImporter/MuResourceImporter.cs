#if TOOLS
using Godot;
using MuClient.addons.MuResourceImporter.InspectorPlugins;
using MuClient.addons.MuResourceImporter.Plugins;
using System;

namespace MuClient.addons.MuResourceImporter;

[Tool]
public partial class MuResourceImporter : EditorPlugin
{

	EncTerrainObjImportPlugin encTerrainObjImportPlugin = new();
	TerrainHeightImportPlugin terrainHeightImportPlugin = new();
	TerrainLightImportPlugin terrainLightImportPlugin = new();
	TextureImportPlugin textureImportPlugin = new();
	EncTerrainAttImportPlugin encTerrainAttImportPlugin = new();

	EncTerrainAttInspectorPlugin encTerrainAttInspectorPlugin = new();
	EncTerrainMapImportPlugin encTerrainMapImportPlugin = new();
	BmdModelImportPlugin bmdModelImportPlugin = new();
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddImportPlugin(encTerrainObjImportPlugin);

		AddImportPlugin(terrainHeightImportPlugin);

		AddImportPlugin(terrainLightImportPlugin);

		AddImportPlugin(textureImportPlugin);

		AddImportPlugin(encTerrainAttImportPlugin);

		AddInspectorPlugin(encTerrainAttInspectorPlugin);

		AddImportPlugin(encTerrainMapImportPlugin);

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
