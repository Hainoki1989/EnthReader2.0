﻿using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json.Serialization;


namespace EnthParser
{
    public class EnthParser
    {
        ModelFile LoadedFile { get; set; }

        public EnthParser() { }

        public bool LoadModelFile(string filename)
        {
            LoadedFile = new ModelFile();

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs)) 
                {
                    fs.Seek(0x0,SeekOrigin.Begin);

                    byte[] MagicNumberBytes = br.ReadBytes(2);

                    if(MagicNumberBytes.SequenceEqual(GlobalIdentifiers.MagicNumberBytes))
                    {
                        LoadedFile.MagicNumberBytes = BitConverter.ToInt16(MagicNumberBytes, 0);
                        LoadedFile.UnkCount = BitConverter.ToInt16(br.ReadBytes(2), 0);

                        br.ReadBytes(4); // skip padding.

                        LoadedFile.TOC_Count = BitConverter.ToInt32(br.ReadBytes(4), 0);

                        for(int i=0; i<LoadedFile.TOC_Count; i++)
                        {
                            LoadedFile.HeaderPairs.Add(new HeaderPairs()
                            {
                                Address = BitConverter.ToInt32(br.ReadBytes(4),0),
                                Length = BitConverter.ToInt32(br.ReadBytes(4),0)

                            });
                        }

                        var Model_File_Location = LoadedFile.HeaderPairs[0];

                        fs.Seek(Model_File_Location.Address, SeekOrigin.Begin);
                        br.ReadBytes(60); //Skip For now;

                        int ModelHeaderDataEndPosition = BitConverter.ToInt32(br.ReadBytes(4), 0) + GlobalIdentifiers.Offset;
                        var CurrentPosition = fs.Position;

                        #region LOD Addresses

                        List<int> temp = new List<int>();

                        while(fs.Position < ModelHeaderDataEndPosition)
                        {
                            //header blocks are in 256 byte chunks all the way through;

                            byte[] Chunk = br.ReadBytes(256);

                            if(GlobalIdentifiers.IsDataBlockInModelHeader(Chunk))
                            {
                                int headerTemp = BitConverter.ToInt32(Chunk.Take(4).ToArray(), 0) + GlobalIdentifiers.Offset;
                                temp.Add(headerTemp);
                            }
                            else if(GlobalIdentifiers.IsSpacerBlockInModelHeader(Chunk))
                            {
                                if(temp.Count > 0)
                                {
                                    LoadedFile.LODAddresses.Add(temp);
                                    temp = new List<int>();
                                }
                            }
                        }

                        if(temp.Count > 0)
                        {
                            LoadedFile.LODAddresses.Add(temp);
                            temp = new List<int>();
                        
                        }
                        #endregion


                        fs.Seek(CurrentPosition, SeekOrigin.Begin);

                        byte[] VertexGroupStart = br.ReadBytes(2);
                        int ModelStartAddress = BitConverter.ToInt16(VertexGroupStart, 0);
                        ModelStartAddress += GlobalIdentifiers.Offset;

                        long FinalPosition = Model_File_Location.Address + Model_File_Location.Length;

                        while (fs.Position < ModelHeaderDataEndPosition) 
                        {
                            VertexBlock:
                                Console.WriteLine("Here");
                            VertexBlockHeader:

                                VertexBlock NewVertexBlock = new VertexBlock();
                                NewVertexBlock.STARTADDRESSFORTHIS = fs.Position;

                                NewVertexBlock.header.Unknown1 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown2 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown3 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown4 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown5 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown6 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown7 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);
                                NewVertexBlock.header.Unknown8 = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0);

                                NewVertexBlock.AddBytesAndReturn(br.ReadBytes(16));

                            VertexData NewVertexData = new VertexData();

                        GetVertexData:

                            byte[] HeaderSearch = NewVertexBlock.AddBytesAndReturn(br.ReadBytes(8));

                            if (HeaderSearch.SequenceEqual(GlobalIdentifiers.VertexBlockHeaderIdenfier))
                            {
                                NewVertexData.VertexCount = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                NewVertexData.NormalCount = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                NewVertexData.UVCount = BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);

                                NewVertexBlock.AddBytesAndReturn(br.ReadBytes(12));

                                #region Read Vertex Floats

                                for(int i=0; i<NewVertexData.VertexCount; i++)
                                {
                                    float x = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    float y = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    float z = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    NewVertexData.VertexList.Add(new Vector3() { X = x, Y = y, Z = z });
                                }

                                #endregion

                                #region Read Normal Half-Floats

                                if(NewVertexData.NormalCount != 0)
                                {
                                    for(int i=0; i<NewVertexData.NormalCount; i++)
                                    {
                                        float x = GlobalIdentifiers.ConvertHalfToFloat(BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0));
                                        float y = GlobalIdentifiers.ConvertHalfToFloat(BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0));
                                        float z = GlobalIdentifiers.ConvertHalfToFloat(BitConverter.ToInt16(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)), 0));
                                        NewVertexData.NormalList.Add(new Vector3() { X = x, Y = y, Z = z });
                                    }

                                    if(!(NewVertexData.NormalCount % 2 == 0))
                                    {
                                        NewVertexBlock.AddBytesAndReturn(br.ReadBytes(2)); // to make it even.
                                    }

                                }

                                #endregion


                                #region UV Floats

                                NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4));

                                for (int i = 0; i < NewVertexData.UVCount; i++)
                                {
                                    float x = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    float y = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    NewVertexData.UVList.Add(new Vector2() { X = x, Y = y });
                                }

                                #endregion

                                NewVertexBlock.VertexDataList.Add(NewVertexData); // add vertex data.
                                NewVertexData = new VertexData();

                                byte[] ShouldBeEnd = NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4));

                                if (!ShouldBeEnd.SequenceEqual(GlobalIdentifiers.EndIndicator))
                                {
                                    Console.WriteLine($"DUN DUN DUUUUUURRRRRRRRRR");
                                    return false;

                                }



                                byte [] result = br.ReadBytes(4);
                                NewVertexBlock.AddBytesAndReturn(result.Take(2).ToArray());


                                byte[] Idenfifier = NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4));

                                if (!Idenfifier.Take(2).ToArray().SequenceEqual(GlobalIdentifiers.uknownBlockIdentifierBytes.Take(2).ToArray()))
                                {
                                    Console.WriteLine("OH FUCK A DUCK WITH A BROOM");
                                    return false;
                                }

                            GetUnknownData:

                                fs.Position += -2;
                                UnknownData NewUnknownData = new UnknownData();
                                NewUnknownData.UnknownDataCount = NewVertexBlock.AddBytesAndReturn(br.ReadBytes(1))[0];
                                NewVertexBlock.AddBytesAndReturn(br.ReadBytes(1)); //padding;

                                for(int i = 0; i< NewUnknownData.UnknownDataCount; i++)
                                {
                                    float x = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);
                                    float y = BitConverter.ToSingle(NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4)), 0);

                                    Vector2 tempVector = new Vector2(x, y);

                                    NewUnknownData.UnknownDataItems.Add(tempVector);
                                }

                                ShouldBeEnd = NewVertexBlock.AddBytesAndReturn(br.ReadBytes(4));
                                NewVertexBlock.UnknownDataList.Add(NewUnknownData);

                                if(!ShouldBeEnd.SequenceEqual(GlobalIdentifiers.EndIndicator))
                                {
                                    Console.WriteLine("Shoot me im done");
                                    return false;
                                }

                                byte[] NextStepCheck = br.ReadBytes(4);

                                // Unknown Data Block
                                // Another Vertex Block

                                CheckAgain:

                                if(NextStepCheck.Take(2).ToArray().SequenceEqual(GlobalIdentifiers.uknownBlockIdentifierBytes))
                                {
                                    NewVertexBlock.AddBytesAndReturn(NextStepCheck.Take(2).ToArray());
                                    goto GetUnknownData;
                                }
                                else if(NextStepCheck.SequenceEqual(GlobalIdentifiers.VertexBlockPadding))
                                {
                                    NewVertexBlock.AddBytesAndReturn(NextStepCheck);
                                    goto CheckAgain;
                                }
                                else if(NextStepCheck.SequenceEqual(new byte[] { 0xff, 0xff, 0xff, 0xff }))
                                {
                                    NewVertexBlock.AddBytesAndReturn(NextStepCheck);
                                    goto CheckAgain;
                                }
                                else if( NextStepCheck.SequenceEqual(new byte[] { 0x00,0x00,0x00,0x10}))
                                {
                                    fs.Position += -4;
                                    goto GetVertexData;
                                }
                                else
                                {
                                    fs.Position += -4;
                                    LoadedFile.VertexBlocks.Add(NewVertexBlock);
                                    NewVertexBlock = new VertexBlock();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Fucked it ");
                    }
                }
            }
           return false;
        }
    }



    public class ModelFile
    {
        public ModelFile() 
        {
            HeaderPairs = new List<HeaderPairs>();
            LODAddresses = new List<List<int>>();
            VertexBlocks = new List<VertexBlock>();
        }

        public int MagicNumberBytes { get; set; }

        public int UnkCount {get;set;}

        public int TOC_Count { get; set; }

        public List<HeaderPairs> HeaderPairs { get; set; }

        public List<List<int>> LODAddresses { get; set; }

        public List<VertexBlock> VertexBlocks { get; set; }
    }

    public class HeaderPairs
    {
        public int Address;
        public int Length;
    }

    public class  VertexBlockHeader
    {
        public short Unknown1;
        public short Unknown2;
        public short Unknown3;
        public short Unknown4;
        public short Unknown5;
        public short Unknown6;
        public short Unknown7;
        public short Unknown8;
    }

    public class VertexData
    {
        public VertexData()
        {
            VertexList = new List<Vector3>();
            UVList = new List<Vector2>();
            NormalList = new List<Vector3>();
        }

        public int VertexCount { get; set; }
        public int NormalCount { get; set; }
        public int UVCount { get; set; }

        public List<Vector3> VertexList { get; set; }
        public List<Vector2> UVList { get;set; }
        public List<Vector3> NormalList { get; set; }
           
    }

    public class UnknownData
    {
        public UnknownData() 
        {
            UnknownDataItems = new List<Vector2>();
        }

        public int UnknownDataCount { get; set; }
        public List<Vector2> UnknownDataItems { get; set; }
    }


    public class VertexBlock
    {
        public byte[] AddBytesAndReturn(byte[] bytes)
        {
            ReadBytesForDebug.AddRange(bytes);
            return bytes;
        }

        [JsonIgnore]
        public List<byte> ReadBytesForDebug;

        public long STARTADDRESSFORTHIS;

        public VertexBlock()
        {
            header = new VertexBlockHeader();
            VertexDataList = new List<VertexData>();
            UnknownDataList = new List<UnknownData>();
            ReadBytesForDebug = new List<byte>();
        }
        public VertexBlockHeader header { get; set; }
        public List<VertexData> VertexDataList { get; set; }
        public List<UnknownData> UnknownDataList { get; set; }
    }



    public static class GlobalIdentifiers
    {
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
