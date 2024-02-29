using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
