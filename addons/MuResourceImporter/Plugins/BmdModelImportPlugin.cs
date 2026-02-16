#if TOOLS

using Client.Data.BMD;
using Client.Data.OBJS;
using Godot;
using Godot.Collections;
using MuClient.addons.MuResourceImporter.Databases;
using MuClient.addons.MuResourceImporter.Extensions;
using MuClient.addons.MuResourceImporter.Readers;
using MuClient.addons.MuResourceImporter.Types;
using MuClient.Extensions;
using MuClient.Models.Terrain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class BmdModelImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.bmd-model";
    public override string _GetVisibleName() => "Bmd Model";
    public override string[] _GetRecognizedExtensions() => ["bmd"];
    public override string _GetResourceType() => "PackedScene";
    public override string _GetSaveExtension() => "scn";
    public override int _GetImportOrder()
    {
        return 100;
    }

    internal TempBMDReader bmdReader = new();

    internal DefaultTextureDatabaseSingleton textureDatabase = DefaultTextureDatabaseSingleton.Instance;
    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {

        return [
            new Dictionary
            {
                { "name", "modelScale" },
                { "default_value", 1.0f/100.0f },
            },
            new Dictionary
            {
                { "name", "MeshOverride" },
                { "default_value", new BmdMeshBlend()},
            },
            
            new Dictionary
            {
                { "name", "SkeletonTransform" },
                { "default_value", 0},
                { "property_hint", 2},
                { "hint_string", "None:0,Identity:1,FlipX:2,FlipY:3,FlipZ:4"},
            },
           
            new Dictionary
            {
                { "name", "AnimationOverride" },
                { "default_value", new BmdAnimationOverride() {
                    Override = new Godot.Collections.Dictionary<int, BmdAnimation> {
                        { 0, new BmdAnimation() },
                        { 1, new BmdAnimation() },
                        { 2, new BmdAnimation() },
                        { 3, new BmdAnimation() },
                        { 4, new BmdAnimation() },
                        { 5, new BmdAnimation() },
                    }
                }}
            },
        ];
    }

    public override int _GetPresetCount() => 0;

    public override Error _Import(string sourceFile, string savePath,
        Dictionary options, Array<string> platformVariants,
        Array<string> genFiles)
    {
        try
        {

            float modelScale = (float)options["modelScale"];
            BmdMeshBlend meshBlend = (BmdMeshBlend)(GodotObject)options["MeshOverride"];
            BmdSkeletonTransformType SkeletonTransform = (BmdSkeletonTransformType)(byte)options["SkeletonTransform"];
            BmdAnimationOverride AnimationOverride = (BmdAnimationOverride)(GodotObject)options["AnimationOverride"];

            BMD bmdData = Task.Run(async () => await bmdReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;
            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";
            string sourceFolder = sourceFile.GetBaseDir();

            Node3D model = GenerateNode(
                bmdData,
                sourceFolder,
                meshBlend,
                SkeletonTransform,
                AnimationOverride
            );

            model.Scale = new Vector3(modelScale, modelScale, modelScale);

            foreach (var item in model.GetChildren())
            {
                item.Owner = model;
                foreach (var grandItem in item.GetChildren())
                {
                    grandItem.Owner = model;
                }
            }
            PackedScene packedScene = new();
            packedScene.Pack(model);
            Error err = ResourceSaver.Save(packedScene, saveFilePath);
            return err;
        }
        catch (Exception e)
        {
            GD.PushError($"Import failed: {e.Message}");
            return Error.Failed;
        }
    }

    public Node3D GenerateNode(
        BMD bmdData,
        string sourceFolder,
        BmdMeshBlend meshBlend,
        BmdSkeletonTransformType skeletonTransform,
        BmdAnimationOverride AnimationOverride
    )
    {
        Node3D rootNode = new()
        {
            Name = "Model",
            EditorDescription = $@"
Folder: {sourceFolder} 
Original File Name: {bmdData.Name}
            ".Trim()
        };
        int FPS = 60;
        bool hasAnimation = false;
        // Key = Godot key
        // Value = Raw key
        short[] boneIdMap = new short[bmdData.Bones.Length];

        // short[] parrentBones = new short[];

        // Store a list of raw root bones
        HashSet<short> rootBones = [];

        System.Collections.Generic.Dictionary<short, string> boneNames = new();

        Skeleton3D skeleton = new()
        {
            Name = "skeleton"
        };
        Skin skin = new()
        {
            ResourceName = "Skin"
        };
        for (int i = 0; i < bmdData.Bones.Length; i++)
        {
            BMDTextureBone bone = bmdData.Bones[i];

            short rawId = (short)i;
            string boneName = bone.IsDummy() ? $"{bone.Name}{i:D2}" : bone.Name;
            if (boneNames.ContainsValue(boneName))
            {
                boneName += $"_{i:D2}";
            }
            boneNames[rawId] = boneName;
            short godotId = (short)skeleton.AddBone(boneName);

            boneIdMap[rawId] = godotId;
        }

        for (int i = 0; i < bmdData.Bones.Length; i++)
        {
            BMDTextureBone bone = bmdData.Bones[i];
            short parentIdx = bone.Parent;
            if (!bone.IsDummy() && parentIdx >= 0 && parentIdx < skeleton.GetBoneCount())
            {
                var godotChildId = boneIdMap[i];
                var godotParrentId = boneIdMap[parentIdx];
                skeleton.SetBoneParent(godotChildId, godotParrentId);
            }
            else
            {
                rootBones.Add((short)i);
            }
        }



        for (int i = 0; i < bmdData.Bones.Length; i++)
        {
            string boneName = boneNames[(short)i];
            skin.AddNamedBind(boneName, Transform3D.Identity);
        }
        switch (skeletonTransform)
        {
            case BmdSkeletonTransformType.Identity:
                skeleton.Transform = Transform3D.Identity;
                break;
            case BmdSkeletonTransformType.FlipX:
                skeleton.Transform = Transform3D.FlipX;
                break;
            case BmdSkeletonTransformType.FlipY:
                skeleton.Transform = Transform3D.FlipY;
                break;
            case BmdSkeletonTransformType.FlipZ:
                skeleton.Transform = Transform3D.FlipZ;
                break;
            default:
                break;
        }

        rootNode.AddChild(skeleton);
        for (int i = 0; i < bmdData.Meshes.Length; i++)
        {

            MeshInstance3D godotMeshInstance = new()
            {
                Name = "mesh_" + i,
            };
            BMDTextureMesh mesh = bmdData.Meshes[i];

            System.Collections.Generic.Dictionary<short, System.Numerics.Vector3> vertexMap = new();
            if (mesh.Vertices == null || mesh.Vertices.Length < 1)
            {
                continue;
            }


            SurfaceTool st = new();

            st.Begin(Mesh.PrimitiveType.Triangles);

            foreach (var triangle in mesh.Triangles)
            {
                var verticleIds = triangle.VertexIndex;
                var normalIds = triangle.NormalIndex;
                var textureCoordinateIds = triangle.TexCoordIndex;
                // TODO: Validate triagle before add verticles

                int minVertId = Math.Min(verticleIds[0], Math.Min(verticleIds[1], verticleIds[2]));
                int maxVertId = Math.Max(verticleIds[0], Math.Max(verticleIds[1], verticleIds[2]));

                int minNormalId = Math.Min(normalIds[0], Math.Min(normalIds[1], normalIds[2]));
                int maxNormalId = Math.Max(normalIds[0], Math.Max(normalIds[1], normalIds[2]));

                int minTexureCoordinateId = Math.Min(textureCoordinateIds[0], Math.Min(textureCoordinateIds[1], textureCoordinateIds[2]));
                int maxTexureCoordinateId = Math.Max(textureCoordinateIds[0], Math.Max(textureCoordinateIds[1], textureCoordinateIds[2]));

                if (triangle.Polygon == 4)
                {
                    minVertId = Math.Min(minVertId, verticleIds[3]);
                    minNormalId = Math.Min(minNormalId, normalIds[3]);
                    minTexureCoordinateId = Math.Min(minTexureCoordinateId, textureCoordinateIds[3]);

                    maxVertId = Math.Max(minVertId, verticleIds[3]);
                    maxNormalId = Math.Max(maxNormalId, normalIds[3]);
                    maxTexureCoordinateId = Math.Max(maxTexureCoordinateId, textureCoordinateIds[3]);
                }
                if (
                    minVertId < 0
                    || minNormalId < 0
                    || minTexureCoordinateId < 0
                    || maxVertId >= mesh.Vertices.Length
                    || maxNormalId >= mesh.Normals.Length
                    || maxTexureCoordinateId >= mesh.TexCoords.Length
                )
                {
                    continue;
                }

                void insertVertex(int at)
                {
                    // 0
                    st.SetNormal(mesh.Normals[normalIds[at]].Normal.ToGodotVector3());
                    st.SetUV(mesh.TexCoords[textureCoordinateIds[at]].ToGodotVector2());
                    st.SetWeights([1, 0, 0, 0]);
                    st.SetBones([Math.Max(mesh.Vertices[verticleIds[at]].Node, (short)0), 0, 0, 0]);
                    st.AddVertex(mesh.Vertices[verticleIds[at]].Position.ToGodotVector3());
                }
                // TODO: surface tool add triage
                // 0
                insertVertex(0);
                // 2
                insertVertex(2);
                // 1
                insertVertex(1);

                if (triangle.Polygon == 4)
                {
                    // 0
                    insertVertex(0);
                    // 2
                    insertVertex(2);
                    // 3
                    insertVertex(3);
                }

            }
            st.GenerateNormals();

            st.Index();


            ArrayMesh arrayMesh = st.Commit();

            // TODO: Add Texture:
            //
            if (mesh.TexturePath.Length > 0)
            {
                string finalTextureResourcePath = DiscoverTexture(sourceFolder, mesh.TexturePath);
                if (!string.IsNullOrEmpty(finalTextureResourcePath))
                {
                    Texture2D texture = ResourceLoader.Load<Texture2D>(finalTextureResourcePath);

                    bool hasTransparent = finalTextureResourcePath[(finalTextureResourcePath.Length - 3)..].ToLower() == "ozt";
                    BaseMaterial3D.TransparencyEnum transparency = hasTransparent ? BaseMaterial3D.TransparencyEnum.Alpha : BaseMaterial3D.TransparencyEnum.Disabled;
                    BaseMaterial3D.DepthDrawModeEnum depthDrawMode = hasTransparent ? BaseMaterial3D.DepthDrawModeEnum.Always : BaseMaterial3D.DepthDrawModeEnum.OpaqueOnly;
                    BaseMaterial3D.CullModeEnum cullMode = hasTransparent ? BaseMaterial3D.CullModeEnum.Disabled : BaseMaterial3D.CullModeEnum.Disabled;
                    BaseMaterial3D.BlendModeEnum blendMode = BaseMaterial3D.BlendModeEnum.Mix;
                    BmdMeshBlendType? configBlendType = meshBlend.GetBlendByIndex(i);
                    if (configBlendType != null && configBlendType != BmdMeshBlendType.None)
                    {
                        switch (configBlendType)
                        {
                            case BmdMeshBlendType.Opaque:

                                break;
                            case BmdMeshBlendType.Additive:
                                blendMode = BaseMaterial3D.BlendModeEnum.Add;
                                break;
                            default:
                                break;
                        }
                    }


                    StandardMaterial3D standardMaterial = new()
                    {
                        AlbedoTexture = texture,
                        DisableReceiveShadows = true,
                        Transparency = transparency,
                        DepthDrawMode = depthDrawMode,
                        CullMode = cullMode,
                        BlendMode = blendMode,
                        ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
                    };
                    arrayMesh.SurfaceSetMaterial(0, standardMaterial);
                }
                else
                {
                    GD.PushWarning($"Missing texture: {mesh.TexturePath}");
                }
            }

            arrayMesh.BlendShapeMode = Mesh.BlendShapeMode.Normalized;

            godotMeshInstance.Mesh = arrayMesh;

            godotMeshInstance.Skin = skin;

            godotMeshInstance.Skeleton = "..";
            skeleton.AddChild(godotMeshInstance);
        }



        bool isDefaultPoseAdded = false;

        AnimationPlayer animationPlayer = new()
        {
            Name = "AnimationPlayer"
        };
        AnimationLibrary animationLibrary = new()
        {
            ResourceName = "AnimationLibrary"
        };
        for (int i = 0; i < bmdData.Actions.Length; i++)
        {
            BMDTextureAction action = bmdData.Actions[i];
            if (action.NumAnimationKeys == 0)
            {
                continue;
            }
            if (!isDefaultPoseAdded)
            {
                for (int iBone = 0; iBone < bmdData.Bones.Length; iBone++)
                {
                    var bone = bmdData.Bones[iBone];
                    if (
                        bone.IsDummy()
                        || bone.Matrixes.Length == 0
                    )
                    {
                        continue;
                    }
                    BMDBoneMatrix boneMatrix = bone.Matrixes.First();
                    if (
                        boneMatrix.Quaternion.Length == 0
                        || boneMatrix.Position.Length == 0
                    )
                    {
                        continue;
                    }

                    short boneId = boneIdMap[iBone];

                    skeleton.SetBoneEnabled(boneId, true);
                    skeleton.SetBonePosePosition(boneId, boneMatrix.Position.First().ToGodotVector3());
                    skeleton.SetBonePoseRotation(boneId, boneMatrix.Quaternion.First().ToGodotQuaternion());
                    skeleton.SetBoneRest(boneId, skeleton.GetBonePose(boneId));
                }
                isDefaultPoseAdded = true;
            }
            BmdAnimation bmdAnimation = new();
            if (AnimationOverride.Override.TryGetValue(i, out BmdAnimation? value) && value != null)
            {
                bmdAnimation = value;
            }

            Animation animation = new()
            {
                ResourceName = "Action_" + i.ToString("D3"),
                LoopMode = bmdAnimation.LoopMode,
            };
            int loopCount = action.NumAnimationKeys;
            if (bmdAnimation.FirstKeyIsRestPose)
            {
                loopCount--;
            }
            if (bmdAnimation.SkipLastKey)
            {
                loopCount--;
            }
            if (loopCount < 1)
            {
                continue;
            }
            // Length per frame
            int totalFrameCount = Math.Max(loopCount - 1, 1);
            if (bmdAnimation.WillApplyLinearFix)
            {
                totalFrameCount += 1;
            }
            float lengthPerKeyFrame = FPS / (float)totalFrameCount / 60;
            for (int iBone = 0; iBone < bmdData.Bones.Length; iBone++)
            {
                BMDTextureBone bone = bmdData.Bones[iBone];
                if (bone.IsDummy() || bone.Matrixes.Length == 0)
                {
                    continue;
                }
                string boneName = boneNames[(short)iBone];


                int rotateTrackIndex = animation.AddTrack(Animation.TrackType.Rotation3D);
                int positionTrackIndex = animation.AddTrack(Animation.TrackType.Position3D);
                animation.TrackSetPath(rotateTrackIndex, skeleton.Name + ":" + boneName);
                animation.TrackSetPath(positionTrackIndex, skeleton.Name + ":" + boneName);
                BMDBoneMatrix currentAction = bone.Matrixes[i];

                for (var k = 0; k < loopCount; k++)
                {
                    int keyIndex = k;
                    if (bmdAnimation.FirstKeyIsRestPose)
                    {
                        keyIndex += 1;
                    }
                    var rotate = currentAction.Quaternion[keyIndex];
                    animation.TrackInsertKey(rotateTrackIndex, lengthPerKeyFrame * k, rotate.ToGodotQuaternion());
                    var position = currentAction.Position[keyIndex];
                    animation.TrackInsertKey(positionTrackIndex, lengthPerKeyFrame * k, position.ToGodotVector3());
                }
                if (bmdAnimation.WillApplyLinearFix)
                {
                    int keyIndex = 0;
                    if (bmdAnimation.FirstKeyIsRestPose)
                    {
                        keyIndex += 1;
                    }
                    var rotate = currentAction.Quaternion[keyIndex];
                    animation.TrackInsertKey(rotateTrackIndex, 1.0f, rotate.ToGodotQuaternion());
                    var position = currentAction.Position[keyIndex];
                    animation.TrackInsertKey(positionTrackIndex, 1.0f, position.ToGodotVector3());
                }
            }

            animation.Length = 1.0f;
            animationLibrary.AddAnimation(animation.ResourceName, animation);
            if (action.NumAnimationKeys > 1)
            {
                hasAnimation = true;
            }
        }
        animationPlayer.AddAnimationLibrary("Animations", animationLibrary);
        if (hasAnimation)
        {
            rootNode.AddChild(animationPlayer);
        }

        rootNode.RotateX((float)(-90).ToRadians());

        return rootNode;
    }

    internal string DiscoverTexture(string sourceFolder, string texturePath)
    {
        return textureDatabase.FindTextureFile(sourceFolder, texturePath);
    }


}
#endif
