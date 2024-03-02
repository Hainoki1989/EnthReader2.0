using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static EnthParser.Models;

namespace EnthParser
{
    public class EnthParser2
    {
        private string filename;

        public EnthFile enthFile = new EnthFile();

        public bool LoadFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (CustomBinaryReader reader = new CustomBinaryReader(fs))
                {
                    fs.Seek(0x0,SeekOrigin.Begin);

                    byte[] MagicNumberBytes = reader.ReadBytes(2);

                    if(MagicNumberBytes.SequenceEqual(GlobalIdentifiers.MagicNumberBytes))
                    {
                        enthFile = new EnthFile();
                        enthFile.UnkCount = BitConverter.ToInt16(reader.ReadBytes(2),0);
                        reader.ReadBytes(4);

                        enthFile.TOC_Count = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                        enthFile.HeaderPairs = new List<HeaderPairs>();

                        for (int i= 0; i<enthFile.TOC_Count; i++)
                        {
                            enthFile.HeaderPairs.Add(new HeaderPairs()
                            {
                                Address = BitConverter.ToInt32(reader.ReadBytes(4), 0),
                                Length = BitConverter.ToInt32(reader.ReadBytes(4), 0)
                            });
                        }

                        var ModelFileLocation = enthFile.HeaderPairs[0];

                        fs.Seek(ModelFileLocation.Address, SeekOrigin.Begin);
                        reader.ReadBytes(60);

                        int LodEndPosition = BitConverter.ToInt32(reader.ReadBytes(4), 0) + GlobalIdentifiers.Offset;

                        var CurrentPosition = fs.Position;


                        enthFile.LODAddresses = GetLODDetails(fs, reader, LodEndPosition);


                        fs.Seek(CurrentPosition,SeekOrigin.Begin);

                        int VertexListStartAddress = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        VertexListStartAddress += GlobalIdentifiers.Offset;
                        int FinalPosition = ModelFileLocation.Address + ModelFileLocation.Length;

                        enthFile.ModelBlocks = new List<ModelBlock>();

                        fs.Seek(VertexListStartAddress, SeekOrigin.Begin);

                        while (fs.Position < FinalPosition) 
                        {
                            ModelBlock returnBlock = GetModelBlock(fs,reader);
                            if( returnBlock != null )
                            {
                                if (returnBlock.VertexBlocks == null)
                                    return true;

                                if (returnBlock.VertexBlocks.Count == 0)
                                    return true;

                                enthFile.ModelBlocks.Add(returnBlock);
                            }
                            else
                            {
                                break;
                            }
                           
                        
                        }


                    }
                }
            }



                return true;
        }

        private ModelBlock GetModelBlock(FileStream fs, CustomBinaryReader reader)
        {
            ModelBlock modelBlock = new ModelBlock();
            modelBlock.ModelHeader = new ModelBlockHeader();
            modelBlock.VertexBlocks = new List<VertexBlock>();
            modelBlock.ModelHeader.Unknown1 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown2 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown3 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown4 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown5 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown6 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown7 = reader.ReadInt16();
            modelBlock.ModelHeader.Unknown8 = reader.ReadInt16();

            reader.ReadBytes(16);

            byte[] Identifier = reader.ReadBytes(4);
            fs.Position += -4;

            while (Identifier.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x10 }))
            {
                modelBlock.VertexBlocks.Add(GetVertexBlock(fs, reader));

                Identifier = reader.ReadBytes(4);
                fs.Position += -4;

                if (Identifier.SequenceEqual(new byte[] { 0x97, 0x5d, 0x0, 0x0 }))
                {
                    byte[] fullPadding = reader.ReadBytes(16);
                    fs.Position += -16;
                    if (fullPadding.SequenceEqual(new byte[] { 0x97, 0x5d, 0x0, 0x0, 0x97, 0x5d, 0x0, 0x0, 0x97, 0x5d, 0x0, 0x0, 0x97, 0x5d, 0x0, 0x0 }))
                    {
                        reader.ReadBytes(16);
                    }
                    else
                    {
                        //clear offset
                        var positionOffset = (fs.Position % 16) == 0 ? 16 : (fs.Position % 16);
                        var offset = 16 - positionOffset;
                        reader.ReadBytes((int)offset);
                    }
                }

                Identifier = reader.ReadBytes(4);
                fs.Position += -4;

                if (Identifier.SequenceEqual(new byte[] { 0xff, 0xff, 0xff, 0xff }))
                {
                        byte[] fullRow = reader.ReadBytes(16);
                        fs.Position += -16;

                        if (fullRow.SequenceEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }))
                        {
                            reader.ReadBytes(16);
                        }
                        else
                        {
                            //clear offset
                            var positionOffset = (fs.Position % 16) == 0 ? 16 : (fs.Position % 16);
                            var offset = 16 - positionOffset;
                            reader.ReadBytes((int)offset);
                        }
                }
                

            }

            



            return modelBlock;
        }

        private VertexBlock GetVertexBlock(FileStream fs, CustomBinaryReader reader)
        {
            
            byte[] HeaderSearch = reader.ReadBytes(8);

            VertexBlock vertexBlock = new VertexBlock();
            vertexBlock.Address = fs.Position;
            vertexBlock.vertexBlockData = new VertexBlockData();
            vertexBlock.vertexBlockData.vertices = new List<Vector3>();
            vertexBlock.vertexBlockData.Normals = new List<Vector3>();
            vertexBlock.vertexBlockData.UVs = new List<Vector2>();
            vertexBlock.MeshGroup = new List<FaceBlock>();


            vertexBlock.vertexBlockHeader = new VertexBlockHeader();
            //vertexBlock.vertexBlockData = new VertexBlockData();
           

            if(HeaderSearch.SequenceEqual(GlobalIdentifiers.VertexBlockHeaderIdenfier))
            {
                vertexBlock.vertexBlockHeader.VertexCount = reader.ReadInt32();
                vertexBlock.vertexBlockHeader.NormalCount = reader.ReadInt32();
                vertexBlock.vertexBlockHeader.UVCount = reader.ReadInt32();

                

                reader.ReadBytes(12);

                for(int i=0; i<vertexBlock.vertexBlockHeader.VertexCount; i++)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();  
                    vertexBlock.vertexBlockData.vertices.Add(new Vector3(x,y, z));
                }

                if (vertexBlock.vertexBlockHeader.NormalCount != 0)
                {
                    reader.ReadBytes(4);

                    for (int i = 0; i < vertexBlock.vertexBlockHeader.NormalCount; i++)
                    {

                        float x = TwoByteToFloat(reader.ReadBytes(2));
                        float y = TwoByteToFloat(reader.ReadBytes(2));
                        float z = TwoByteToFloat(reader.ReadBytes(2));

                        vertexBlock.vertexBlockData.Normals.Add(new Vector3(x,y,z));
                    }
                }

                if (vertexBlock.vertexBlockData.Normals.Count % 2 != 0)
                    reader.ReadBytes(2);

                reader.ReadBytes(4);

                for(int i =0; i<vertexBlock.vertexBlockHeader.UVCount; i++)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    vertexBlock.vertexBlockData.UVs.Add(new Vector2(x, y));
                }

                byte[] ShouldBeEnd = reader.ReadBytes(4);

                if(!ShouldBeEnd.SequenceEqual(GlobalIdentifiers.EndIndicator))
                {
                    Console.WriteLine("Broke after reading UVs");
                    return null;
                }

                reader.ReadBytes(4);

                byte[] Identifier = reader.ReadBytes(2);
                fs.Position += -2;

                while(Identifier.SequenceEqual(GlobalIdentifiers.uknownBlockIdentifierBytes.Take(2).ToArray()))
                {
                    vertexBlock.MeshGroup.Add(GetFaceBlock(fs, reader));

                    Identifier = reader.ReadBytes(2);
                    fs.Position += -2;
                }

                

            }


            return vertexBlock;
        }


        private FaceBlock GetFaceBlock(FileStream fs, CustomBinaryReader reader)
        {
            FaceBlock block = new FaceBlock();
            block.indicies = new List<Face>();
            reader.ReadBytes(2);
            block.FaceDataCount = reader.ReadBytes(1)[0];
            reader.ReadBytes(1);

            for(int i=0; i<block.FaceDataCount; i++)
            {
                Face temp = new Face(reader.ReadByte(), reader.ReadByte());
                block.indicies.Add(temp);
                reader.ReadBytes(6);
            }

            if(!reader.ReadBytes(4).SequenceEqual(GlobalIdentifiers.EndIndicator))
            {
                return null;
            }
            return block;

        }

        
        private float TwoByteToFloat(byte[] input)
        {
            ushort half = BitConverter.ToUInt16(input, 0);

            int sign = (half & 0x8000) << 16;
            int exp = ((half & 0x7C00) + 0x1C000) << 13;
            int mantissa = (half & 0x03FF) << 13;

            uint result = (uint)(sign | exp | mantissa);
            return BitConverter.ToSingle(BitConverter.GetBytes(result), 0);
        }

        private List<List<int>> GetLODDetails(FileStream fs, CustomBinaryReader reader, int endPosition)
        {
            
            List<List<int>> OutputList = new List<List<int>>();
            
            List<int> temp = new List<int>();

            while(fs.Position < endPosition)
            {
                byte[] Chunk = reader.ReadBytes(256);

                if(GlobalIdentifiers.IsDataBlockInModelHeader(Chunk))
                {
                    int headerTemp = BitConverter.ToInt32(Chunk.Take(4).ToArray(), 0);
                    headerTemp += GlobalIdentifiers.Offset;
                    temp.Add(headerTemp);
                }
                else if(GlobalIdentifiers.IsSpacerBlockInModelHeader(Chunk))
                {
                    if(temp.Count > 0)
                    {
                        OutputList.Add(temp);
                        temp = new List<int>();
                    }
                }
            }

            if(temp.Count > 0)
            {
                OutputList.Add(temp);
                temp = new List<int>(); 
            }

            return OutputList;
        }
    }


}
