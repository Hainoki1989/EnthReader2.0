using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnthParser.Models;

namespace EnthParser
{
    public class CustomBinaryReader : BinaryReader
    {
        public CustomBinaryReader(Stream input) : base(input)
        {
        }

        public override byte[] ReadBytes(int count)
        {
            byte[] bytes = base.ReadBytes(count);


            if(GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            foreach(byte b in bytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return bytes;
        }

        public override bool ReadBoolean()
        {
            bool value = base.ReadBoolean();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            GlobalIdentifiers.GlobalBuffer.Add((byte)(value ? 1 : 0));

            return value;
        }

        public override short ReadInt16()
        {
            short value = base.ReadInt16();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] int16Bytes = BitConverter.GetBytes(value);

            foreach (byte b in int16Bytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return value;
        }

        public override double ReadDouble()
        {
            double value = base.ReadDouble();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] doubleBytes = BitConverter.GetBytes(value);

            foreach (byte b in doubleBytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return value;
        }

        public override char ReadChar()
        {
            char character = base.ReadChar();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] charBytes = BitConverter.GetBytes(character);

            foreach (byte b in charBytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return character;
        }

        public override string ReadString()
        {
            string str = base.ReadString();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] stringBytes = Encoding.Default.GetBytes(str);

            foreach (byte b in stringBytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return str;
        }

        public override float ReadSingle()
        {
            float value = base.ReadSingle();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] singleBytes = BitConverter.GetBytes(value);

            foreach (byte b in singleBytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return value;
        }

        public override int ReadInt32()
        {
            int value = base.ReadInt32();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            byte[] int32Bytes = BitConverter.GetBytes(value);

            foreach (byte b in int32Bytes)
            {
                GlobalIdentifiers.GlobalBuffer.Add(b);
            }

            return value;
        }

        public override byte ReadByte()
        {
            byte value = base.ReadByte();

            if (GlobalIdentifiers.GlobalBuffer == null)
            {
                GlobalIdentifiers.GlobalBuffer = new List<byte>();
            }

            GlobalIdentifiers.GlobalBuffer.Add(value);

            return value;
        }

    }
}
