using RL8000_NFCReader.MifareClassicControlEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL8000_NFCReader.NFCCardTypes
{
    public class ISO1443A_MifareClassic_NFCCard : IDisposable
    {

        RL8000_NFC Owner;

        internal UIntPtr CardHandle { private set; get; }
        public NFCCardInfo CardInfo { private set; get; }

        public bool Disposed { private set; get; } = false;

        const string DisposedError = "This card has been disposed already";

        /// <summary>
        /// Creates a card that you can then use
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="CardHandle"></param>
        /// <param name="CardInfo"></param>
        internal ISO1443A_MifareClassic_NFCCard(RL8000_NFC Owner, UIntPtr CardHandle, NFCCardInfo CardInfo)
        {
            this.Owner = Owner;
            this.CardHandle = CardHandle;
            this.CardInfo = CardInfo;
        }

        /// <summary>
        /// Athenthicates a sector against a key
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Key"></param>
        /// <param name="KeyType"></param>
        public void Athenthicate(byte BlockAddress, byte[] Key, KeyTypes KeyType)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            if (Key.Count() != 6)
                throw new Exception($"Invalid input all Keys are 6 bytes long where yous was:{Key.Count()}");
            int result = rfidlib_aip_iso14443A.MFCL_Authenticate(Owner.ReaderHandle, CardHandle, BlockAddress, (byte)KeyType, Key);
            if (result != 0)
                throw new Exception($"Failed to athenticate block {BlockAddress} with {KeyType} with result:{result}");
        }

        /// <summary>
        /// Reads raw data bytes to a block
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public byte[] ReadBlock(byte BlockAddress)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            const byte BlockSize = 16;
            byte[] ReadData = new byte[16];
            int result = rfidlib_aip_iso14443A.MFCL_ReadBlock(Owner.ReaderHandle, CardHandle, BlockAddress, ReadData, BlockSize);
            if (result != 0)
                throw new Exception($"Failed to read block {BlockAddress} with result:{result}");
            return ReadData;
        }

        /// <summary>
        /// Writes raw data bytes to a block
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public void WriteBlock(byte BlockAddress, byte[]Data)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            if (Data.Count() != 16)
                throw new Exception($"Invalid input all blocks are 16 bytes long where yous was:{Data.Count()}");
            int result = rfidlib_aip_iso14443A.MFCL_WriteBlock(Owner.ReaderHandle, CardHandle, BlockAddress, Data);
            if (result != 0)
                throw new Exception($"Failed to write to block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Increases Value
        /// Requires a value to be written to the block for formating
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public void IncrementValue(byte BlockAddress, UInt32 Value)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            int result = rfidlib_aip_iso14443A.MFCL_Increment(Owner.ReaderHandle, CardHandle, BlockAddress, Value);
            if (result != 0)
                throw new Exception($"Failed to write to block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Decreases Value
        /// Requires a value to be written to the block for formating
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public void DecrementValue(byte BlockAddress, UInt32 Value)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            int result = rfidlib_aip_iso14443A.MFCL_Decrement(Owner.ReaderHandle, CardHandle, BlockAddress, Value);
            if (result != 0)
                throw new Exception($"Failed to write to block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Writes a value to a block with formating
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public void WriteValue(byte BlockAddress, UInt32 Value)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            int result = rfidlib_aip_iso14443A.MFCL_FormatValueBlock(Owner.ReaderHandle, CardHandle, BlockAddress, Value);
            if (result != 0)
                throw new Exception($"Failed to write to block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Reads raw data bytes to a block
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        public UInt32 ReadValue(byte BlockAddress)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            const byte BlockSize = 16;
            byte[] ReadData = new byte[16];
            //since the maufacturer neglected to put in the Read Value (Standard API China) we will hack it in. start with a Raw Block Read 
            int result = rfidlib_aip_iso14443A.MFCL_ReadBlock(Owner.ReaderHandle, CardHandle, BlockAddress, ReadData, BlockSize);
            //Do our own format check [Value][~Value][Value][A,~A,A,~A]
            if (result == 0 && ((ReadData[0] ^ 0xFF) != ReadData[4] || (ReadData[1] ^ 0xFF) != ReadData[5] || (ReadData[2] ^ 0xFF) != ReadData[6] || (ReadData[3] ^ 0xFF) != ReadData[7] ||
                    (ReadData[4] ^ 0xFF) != ReadData[8] || (ReadData[5] ^ 0xFF) != ReadData[9] || (ReadData[6] ^ 0xFF) != ReadData[10] || (ReadData[7] ^ 0xFF) != ReadData[11] ||
                    ReadData[12] != 0x05 || ReadData[13] != 0xFA || ReadData[14] != 0x05 || ReadData[15] != 0xFA))
                    result = -17;
            if (result != 0)
                throw new Exception($"Failed to read block {BlockAddress} with result:{result}");
            //then take the front four bytes as this is where the value is stored as its least corrupt version luckily.
            return BitConverter.ToUInt32(ReadData,0);
            //[Value][~Value][Value][A,~A,A,~A] thank you Ada for that valueble info. (shes cool look up adafruit industies)
        }

        /// <summary>
        /// Temp stores data value in the reader for storage
        /// </summary>
        /// <param name="BlockAddress"></param>
        public void RestoreValue(byte BlockAddress)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            int result = rfidlib_aip_iso14443A.MFCL_Restore(Owner.ReaderHandle, CardHandle, BlockAddress);
            if (result != 0)
                throw new Exception($"Failed to read block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Transerfers the Temp data value stored in reader durring a restore to the memory address 
        /// </summary>
        /// <param name="BlockAddress"></param>
        public void TransferValue(byte BlockAddress)
        {
            if (Disposed)
                throw new Exception(DisposedError);
            int result = rfidlib_aip_iso14443A.MFCL_Transfer(Owner.ReaderHandle, CardHandle, BlockAddress);
            if (result != 0)
                throw new Exception($"Failed to read block {BlockAddress} with result:{result}");
        }

        /// <summary>
        /// Wouldn't recommend using it as the docs have me super nervous with a repeated value thinking there is a typeo
        /// Creates a access byte
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static byte[] MakeAccessbyte(DataBlockAccess Block0Access,  DataBlockAccess Block1Access, DataBlockAccess Block2Access, TrailDataBlockAccess TrailOptions)
        {
            byte[] Return = new byte[4];
            int result = rfidlib_aip_iso14443A.MFCL_CreateAccessCondition((byte)Block0Access, (byte)Block1Access, (byte)Block2Access, (byte)TrailOptions, Return);
            if (result != 0)
                throw new Exception($"Failed to create Access bytes");
            return Return;
        }

        /// <summary>
        /// Wouldn't recommend using it as the docs have me super nervous with a repeated value thinking there is a typeo
        /// Gets the Access bytes values
        /// </summary>
        /// <param name="AccessData"></param>
        /// <returns></returns>
        public static MifareClassicAccessDataPackage ParseAccessBytes(byte[] AccessData)
        {
            byte Block0Access = 255;
            byte Block1Access = 255;
            byte Block2Access = 255;
            byte TrailOptions = 255;
            if (AccessData.Count() != 4)
                throw new Exception($"Invalid input all Access Data is 4 bytes long where yous was:{AccessData.Count()}");
            byte[] Return = new byte[4];
            int result = rfidlib_aip_iso14443A.MFCL_ParseAccessCondi(AccessData, ref Block0Access, ref Block1Access, ref Block2Access, ref TrailOptions);
            if (result != 0)
                throw new Exception($"Failed to read Access bytes");
            return new MifareClassicAccessDataPackage() { Block0Access = (DataBlockAccess)Block0Access, Block1Access = (DataBlockAccess)Block1Access, Block2Access = (DataBlockAccess)Block2Access, AccessBlockAccess = (TrailDataBlockAccess)TrailOptions };
        }

        /// <summary>
        /// Returns the Sector of a given Block
        /// </summary>
        /// <param name="BlockAddress"></param>
        /// <returns></returns>
        public static byte GetSectorFromBlock(byte BlockAddress)
        {
            return rfidlib_aip_iso14443A.MFCL_Block2Sector(BlockAddress);
        }

        /// <summary>
        /// Returns the Block of a given Sector
        /// </summary>
        /// <param name="SectorAddress"></param>
        /// <returns></returns>
        public static byte GetBlockFromSector(byte SectorAddress)
        {
            return rfidlib_aip_iso14443A.MFCL_Sector2Block(SectorAddress);
        }

        /// <summary>
        /// returns if the block is a tail sector with access information
        /// </summary>
        /// <param name="SectorAddress"></param>
        /// <returns></returns>
        public static bool IsTailBlock(byte SectorAddress)
        {
            return rfidlib_aip_iso14443A.MFCL_IsTailerBlock(SectorAddress) == 1;
        }

        /// <summary>
        /// Dispose of things
        /// </summary>
        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                rfidlib_reader.RDR_TagDisconnect(Owner.ReaderHandle, CardHandle);
            }
            Owner?.DisconnectCard();
        }

        /// <summary>
        /// The deconstructor
        /// </summary>
        ~ISO1443A_MifareClassic_NFCCard()
        {
            if (!Disposed)
            {
                Disposed = true;
                rfidlib_reader.RDR_TagDisconnect(Owner.ReaderHandle, CardHandle);
            }
            Owner?.DisconnectCard();
        }
    }
}
