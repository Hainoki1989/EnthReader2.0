using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static EnthParser.Models;

namespace EnthParser
{
    public class Models
    {
        public class EnthFile
        {
            public string ModelName;
            public int UnkCount;
            public int TOC_Count;

            public List<HeaderPairs> HeaderPairs;
            public List<List<int>> LODAddresses;

            public List<ModelBlock> ModelBlocks;



            public void ToIndividualLODOBJ(string filename = null, string modelname = null, bool MergeVerticies = false)
            {
                var vertexList = ModelBlocks.SelectMany(modelBlock => modelBlock.VertexBlocks).ToList();

               

                for (int i = 0; i < LODAddresses.Count; i++)
                {
                    int StartAddress = LODAddresses[i][0];
                    int EndAddress = (i < LODAddresses.Count - 1) ? LODAddresses[i + 1][0] : 0x999999;

                    var input = vertexList.Where(x => x.Address >= StartAddress && x.Address < EndAddress);

                    List<List<VertexBlock>> ModelSeperation = new List<List<VertexBlock>>();

                    for (int f = 0; f < LODAddresses[i].Count; f++)
                    {
                        int StartFAddress = LODAddresses[i][f];
                        int EndFAddress = (f < LODAddresses[i].Count - 1) ? LODAddresses[i][f + 1] : 0x999999;

                        var temp = input.Where(y => y.Address >= StartFAddress && y.Address < EndFAddress);
                        ModelSeperation.Add(temp.ToList());
                    }

                    ListToObj(ModelSeperation, filename, $"{modelname}_{i.ToString()}");
                }
            }


            public void ExportIndivdualLODMeshes(string folder, string model)
            {

                var cltr = CultureInfo.GetCultureInfo("en-GB");

                string LODFolder = $"{folder}\\LOD";
                if(!Directory.Exists(LODFolder))
                    Directory.CreateDirectory(LODFolder);

                var vertexList = ModelBlocks.SelectMany(modelBlock => modelBlock.VertexBlocks).ToList();

                for (int i = 0; i < LODAddresses.Count; i++)
                {
                    int StartAddress = LODAddresses[i][0];
                    int EndAddress = (i < LODAddresses.Count - 1) ? LODAddresses[i + 1][0] : 0x999999;


                    var input = vertexList.Where(x => x.Address >= StartAddress && x.Address < EndAddress);

                    int meshCounter = 0;
                    foreach(var mesh in input)
                    {
                        StringBuilder sb = new StringBuilder();
                        StringBuilder iv = new StringBuilder();

                        int counter = 0;

                        long Address = 0;

                        Address = mesh.Address;

                        foreach(var vertex in mesh.vertexBlockData.vertices)
                        {
                            sb.AppendLine($"v {vertex.X.ToString(cltr)} {vertex.Y.ToString(cltr)} {vertex.Z.ToString(cltr)}");
                        }

                        for (int ig = 0; ig < mesh.MeshGroup.Count; ig++)
                        {
                            for (int j = 0; j < mesh.MeshGroup[ig].indicies.Count-2; j++)
                            {
                                if (mesh.MeshGroup[ig].indicies[j + 2].IsValidTri)
                                {
                                    iv.AppendLine($"f {mesh.MeshGroup[ig].indicies[j].FaceIndex + 1 + counter} {mesh.MeshGroup[ig].indicies[j + 1].FaceIndex + 1 + counter} {mesh.MeshGroup[ig].indicies[j + 2].FaceIndex + 1 + counter}");
                                }
                            }
                        }

                        counter += mesh.vertexBlockData.vertices.Count;


                        string outputText = sb.ToString() + "\n" + iv.ToString();


                        string outputFolder = $"{folder}\\LOD\\{i}";

                        if(!Directory.Exists(outputFolder))
                            Directory.CreateDirectory(outputFolder);

                        File.WriteAllText($"{folder}\\LOD\\{i}\\{meshCounter}_{model}_{Address.ToString("X")}.obj", outputText);
                        meshCounter++;
                    }

                }
            }


            


            public void ListToObj( List<List<VertexBlock>> files, string folder, string model)
            {
                StringBuilder vertexString = new StringBuilder();
                StringBuilder indexString = new StringBuilder();
                StringBuilder textureCoords = new StringBuilder();

                int groupCounter = 0;

                int previousCount = 0;

                for(int f=0; f<files.Count; f++)
                {
                    indexString.AppendLine($"o Mesh{f}");

                    foreach (var block in files[f])
                    {
                        foreach (var vertex in block.vertexBlockData.vertices)
                        {
                            vertexString.AppendLine($"v {vertex.X.ToString(CultureInfo.GetCultureInfo("en-GB"))} {vertex.Y.ToString(CultureInfo.GetCultureInfo("en-GB"))} {vertex.Z.ToString(CultureInfo.GetCultureInfo("en-GB"))}");
                            
                        }

                        foreach(var texturecoord in block.vertexBlockData.UVs)
                        {
                            textureCoords.AppendLine($"vt {texturecoord.X.ToString(CultureInfo.GetCultureInfo("en-GB"))} {texturecoord.Y.ToString(CultureInfo.GetCultureInfo("en-GB"))} ");
                        }

                        int IndexCounter = 0;
                        //foreach (var indexG in block.MeshGroup)
                        for (int ig = 0; ig < block.MeshGroup.Count; ig++)
                        {
                            indexString.AppendLine($"g group{groupCounter++}");

                            for (int j = 0; j < block.MeshGroup[ig].indicies.Count - 2; j++)
                            {
                                if (block.MeshGroup[ig].indicies[j + 2].IsValidTri)
                                {
                                    indexString.AppendLine($"f {block.MeshGroup[ig].indicies[j].FaceIndex + 1 + previousCount}/{IndexCounter + 1} {block.MeshGroup[ig].indicies[j + 1].FaceIndex + 1 + previousCount}/{IndexCounter + 2} {block.MeshGroup[ig].indicies[j + 2].FaceIndex + 1 + previousCount}/{IndexCounter + 3}");
                                    IndexCounter++;
                                }
                            }
                        }

                        previousCount += block.vertexBlockData.vertices.Count;
                    }
                }

                string OutputText = vertexString.ToString() + "\n" + textureCoords.ToString() + "\n" +indexString.ToString();

                File.WriteAllText($"{folder}\\Model_{model}.obj", OutputText);
            }

            public void ToObj(string filename = null)
            {

                foreach(var item in ModelBlocks)
                {
                    foreach (var VB in item.VertexBlocks)
                    {
                        using (StreamWriter writer = new StreamWriter($"output\\{VB.Address}.obj"))
                        {
                            foreach (var vertex in VB.vertexBlockData.vertices)
                            {
                                writer.WriteLine($"v {vertex.X.ToString(CultureInfo.GetCultureInfo("en-GB"))} {vertex.Y.ToString(CultureInfo.GetCultureInfo("en-GB"))} {vertex.Z.ToString(CultureInfo.GetCultureInfo("en-GB"))}");
                            }

                            foreach(var index in VB.MeshGroup)
                            {
                                for(int i=0; i<index.indicies.Count-2; i++)
                                {
                                    if (index.indicies[i+2].IsValidTri)
                                    {
                                        writer.WriteLine($"f {index.indicies[i].FaceIndex +1 }/{i} {index.indicies[i + 1].FaceIndex +1}/{i} {index.indicies[i + 2].FaceIndex +1}/{i} ");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public class HeaderPairs
        {
            public int Address;
            public int Length;
        }


        public class ModelBlock
        {
            public int Address;
            public ModelBlockHeader ModelHeader;
            public List<VertexBlock> VertexBlocks;

            
        }

        public class ModelBlockHeader
        {
            public short Unknown1;
            public short Unknown2;
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;

        }

        public class VertexBlockHeader
        {
            public int VertexCount;
            public int NormalCount;
            public int UVCount;
        }

        public class VertexBlock
        {
            public long Address;
            public VertexBlockHeader vertexBlockHeader;
            public VertexBlockData vertexBlockData;
            public List<FaceBlock> MeshGroup;
        }

        public class VertexBlockData
        {
            public List<Vector3> vertices;
            public List<Vector3> Normals;
            public List<Vector2> UVs;
        }

        public class FaceBlock
        {
            public int FaceDataCount;
            public List<Face> indicies;
        }

        public class Face
        {
            public Face(short FaceIndex, short ValidCount)
            {
                this.FaceIndex = FaceIndex;
                this.ValidCount = ValidCount;
            }


            public short FaceIndex;
            private short ValidCount;

            public bool IsValidTri
            {
                get
                {
                    return (ValidCount != 128);
                }
            }
        }


        public static class GlobalIdentifiers
        {
            public static List<byte> GlobalBuffer;
            
            
            public static int Offset = 96; //fixed offset for a lot of things.

            public static byte[] VertexBlockHeaderIdenfier = new byte[]
            {
        0x00,0x00,0x00,0x10,0xFF,0x43,0x01,0x6C
            };

            public static byte[] MagicNumberBytes = new byte[] { 0x87, 0xFC };

            public static byte[] HeaderModelIndicatorDataBlockEnd = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x80, 0x3f, 0x00, 0x00, 0x00, 0x00 };

            public static byte[] uknownBlockIdentifierBytes = new byte[]
            {
        0x00,0xC0, 0x00, 0x6D
            };

            public static byte[] EndIndicator = new byte[]
            {
        0x00,0x00,0x00,0x17
            };

            public static byte[] VertexBlockPadding = new byte[]
            {
        0x97,0x5D, 0x00,0x00
            };

            public static bool ByteArraysAreEqual(byte[] array1, byte[] array2)
            {
                if (array1.Length != array2.Length)
                    return false;

                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                        return false;
                }

                return true;
            }

            public static float ConvertHalfToFloat(short halfValue)
            {
                // Extracting the sign, exponent, and mantissa bits
                int sign = (halfValue & 0x8000) << 16;
                int exponent = ((halfValue & 0x7C00) + 0x1C000) << 13;
                int mantissa = (halfValue & 0x03FF) << 13;

                // Combining the bits to form the float representation
                int floatBits = sign | exponent | mantissa;

                // Converting the combined bits to a float
                return BitConverter.ToSingle(BitConverter.GetBytes(floatBits), 0);
            }


            public static bool IsSpacerBlockInModelHeader(byte[] input)
            {
                if (input.Length != 256)
                    return false;
                else
                {
                    byte[] StartValues = input.Take(4).ToArray();
                    byte[] EndValue = input.Skip(252).Take(4).ToArray();

                    if (StartValues.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x00 }) && EndValue.SequenceEqual(new byte[] { 0x00, 0x00, 0x80, 0x3f }))
                        return true;
                }


                return false;
            }


            public static bool IsDataBlockInModelHeader(byte[] input)
            {
                if (input.Length != 256)
                    return false;
                else
                {
                    byte[] StartValues = input.Take(4).ToArray();
                    byte[] EndValue = input.Skip(224).Take(16).ToArray();

                    if ((!StartValues.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x00 })) && EndValue.SequenceEqual(GlobalIdentifiers.HeaderModelIndicatorDataBlockEnd))
                        return true;
                }


                return false;
            }

        }

    }
}
