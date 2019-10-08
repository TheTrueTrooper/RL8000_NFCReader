using System;
using System.Collections.Generic;
using System.Text;

namespace RL8000_NFCReader
{
    public delegate void RFIDLIB_EVENT_CALLBACK(UIntPtr wparam, UIntPtr lparam);  //stdcall
    public delegate void RFID_EVENT_CALLBACK_NEW(UIntPtr inParam, UIntPtr wparam, UIntPtr lparam);  //stdcall
    class rfidlib_def
    {

        // Air protocol id
        public enum RFID_APL_Protocol
        {
            RFID_APL_UNKNOWN_ID = 0,
            RFID_APL_ISO15693_ID = 1,
            RFID_APL_ISO14443A_ID = 2,
            RFID_APL_ISO14443B_ID = 3,
            RFID_APL_EPCGEN2_ID = 4,
            RFID_APL_ISO18000P3M3 = 5
        }

        public const int RFID_APL_UNKNOWN_ID = 0;
        public const int RFID_APL_ISO15693_ID = 1;
        public const int RFID_APL_ISO14443A_ID = 2;
        public const int RFID_APL_ISO14443B_ID = 3;
        public const int RFID_APL_EPCGEN2_ID = 4;
        public const int RFID_APL_ISO18000P3M3 = 5;

        public enum ISO15693CardType
        {
            RFID_UNKNOWN_PICC_ID = 0,
            RFID_ISO15693_PICC_ICODE_SLI_ID = 1,
            RFID_ISO15693_PICC_TI_HFI_PLUS_ID = 2,
            RFID_ISO15693_PICC_ST_SERIES_ID = 3,    /* ST  serial */
            RFID_ISO15693_PICC_FUJ_MB89R118C_ID = 4,
            RFID_ISO15693_PICC_ST_M24LR64_ID = 5,
            RFID_ISO15693_PICC_ST_M24LR16E_ID = 6,
            RFID_ISO15693_PICC_ICODE_SLIX_ID = 7,
            RFID_ISO15693_PICC_TIHFI_STANDARD_ID = 8,
            RFID_ISO15693_PICC_TIHFI_PRO_ID = 9,
            RFID_ISO15693_PICC_ICODE_SLIX2_ID = 10,
            RFID_ISO15693_PICC_CIT83128_ID = 11,
            RFID_ISO15693_PICC_ST_LRI2k_ID = 12,
            RFID_ISO15693_PICC_ICODE_SLI_L_ID = 13,
            RFID_ISO15693_PICC_ICODE_SLIX_L_ID = 14,
            RFID_ISO15693_PICC_ICODE_SLI_S_ID = 15,
            RFID_ISO15693_PICC_ICODE_SLIX_S_ID = 16,
            RFID_ISO15693_PICC_EM4237SLIC_ID = 17,
            RFID_ISO15693_PICC_EM4237SLIX_ID = 18,
            RFID_ISO15693_PICC_RF430FR12XH_ID = 19,
            RFID_ISO15693_PICC_ST25DV04K_JF_ID = 20,
        }

        // ISO15693 Tag type id
        public const int RFID_UNKNOWN_PICC_ID = 0;
        public const int RFID_ISO15693_PICC_ICODE_SLI_ID = 1;
        public const int RFID_ISO15693_PICC_TI_HFI_PLUS_ID = 2;
        public const int RFID_ISO15693_PICC_ST_SERIES_ID = 3;	    /* ST  serial */
        public const int RFID_ISO15693_PICC_FUJ_MB89R118C_ID = 4;
        public const int RFID_ISO15693_PICC_ST_M24LR64_ID = 5;
        public const int RFID_ISO15693_PICC_ST_M24LR16E_ID = 6;
        public const int RFID_ISO15693_PICC_ICODE_SLIX_ID = 7;
        public const int RFID_ISO15693_PICC_TIHFI_STANDARD_ID = 8;
        public const int RFID_ISO15693_PICC_TIHFI_PRO_ID = 9;
        public const int RFID_ISO15693_PICC_ICODE_SLIX2_ID = 10;
        public const int RFID_ISO15693_PICC_CIT83128_ID = 11;
        public const int RFID_ISO15693_PICC_ST_LRI2k_ID = 12;
        public const int RFID_ISO15693_PICC_ICODE_SLI_L_ID = 13;
        public const int RFID_ISO15693_PICC_ICODE_SLIX_L_ID = 14;
        public const int RFID_ISO15693_PICC_ICODE_SLI_S_ID = 15;
        public const int RFID_ISO15693_PICC_ICODE_SLIX_S_ID = 16;
        public const int RFID_ISO15693_PICC_EM4237SLIC_ID = 17;
        public const int RFID_ISO15693_PICC_EM4237SLIX_ID = 18;
        public const int RFID_ISO15693_PICC_RF430FR12XH_ID = 19;
        public const int RFID_ISO15693_PICC_ST25DV04K_JF_ID = 20;


        public enum ISO14443ACardType
        {
            RFID_ISO14443A_PICC_NXP_ULTRALIGHT_ID = 1,
            RFID_ISO14443A_PICC_NXP_MIFARE_S50_ID = 2,
            RFID_ISO14443A_PICC_NXP_MIFARE_S70_ID = 3
        }

        //ISO14443a tag type id 
        public const int RFID_ISO14443A_PICC_NXP_ULTRALIGHT_ID = 1;
        public const int RFID_ISO14443A_PICC_NXP_MIFARE_S50_ID = 2;
        public const int RFID_ISO14443A_PICC_NXP_MIFARE_S70_ID = 3;

        public enum Inventory_AIType
        {
            AI_TYPE_NEW = 1,      // new antenna inventory  (reset RF power)
            AI_TYPE_CONTINUE = 2  // continue antenna inventory ;
        }

        //Inventory type 
        public const int AI_TYPE_NEW = 1;       // new antenna inventory  (reset RF power)
        public const int AI_TYPE_CONTINUE = 2;  // continue antenna inventory ;

        public enum Inventory_MovePostion
        {
            RFID_NO_SEEK = 0,           // No seeking 
            RFID_SEEK_FIRST = 1,        // Seek first
            RFID_SEEK_NEXT = 2,         // Seek next 
            RFID_SEEK_LAST = 3
        }
        // Move position 
        public const int RFID_NO_SEEK = 0;			// No seeking 
        public const int RFID_SEEK_FIRST = 1;					// Seek first
        public const int RFID_SEEK_NEXT = 2;				// Seek next 
        public const int RFID_SEEK_LAST = 3;				// Seek last


        /*
        usb enum information
        */
        public const int HID_ENUM_INF_TYPE_SERIALNUM = 1;
        public const int HID_ENUM_INF_TYPE_DRIVERPATH = 2;

        /*
        * Open connection string 
        */
        public const string CONNSTR_NAME_RDTYPE = "RDType";
        public const string CONNSTR_NAME_COMMTYPE = "CommType";

        public const string CONNSTR_NAME_COMMTYPE_COM = "COM";
        public const string CONNSTR_NAME_COMMTYPE_USB = "USB";
        public const string CONNSTR_NAME_COMMTYPE_NET = "NET";
        public const string CONNSTR_NAME_COMMTYPE_BLUETOOTH = "BLUETOOTH";

        //HID Param
        public const string CONNSTR_NAME_HIDADDRMODE = "AddrMode";
        public const string CONNSTR_NAME_HIDSERNUM = "SerNum";
        //COM Param
        public const string CONNSTR_NAME_COMNAME = "COMName";
        public const string CONNSTR_NAME_COMBARUD = "BaudRate";
        public const string CONNSTR_NAME_COMFRAME = "Frame";
        public const string CONNSTR_NAME_BUSADDR = "BusAddr";
        //TCP,UDP
        public const string CONNSTR_NAME_REMOTEIP = "RemoteIP";
        public const string CONNSTR_NAME_REMOTEPORT = "RemotePort";
        public const string CONNSTR_NAME_LOCALIP = "LocalIP";
        //Bluetooth
        public const string CONNSTR_NAME_BLUETOOTH_SN = "Addr";

        public const byte CONNSTR_Addressing_NoneAddressed = 0;
        public const byte CONNSTR_Addressing_NoneAddressed_SerialNumber = 1;
        /*
        * Get loaded reader driver option 
        */
        public const string LOADED_RDRDVR_OPT_CATALOG = "CATALOG";
        public const string LOADED_RDRDVR_OPT_NAME = "NAME";
        public const string LOADED_RDRDVR_OPT_ID = "ID";
        public const string LOADED_RDRDVR_OPT_COMMTYPESUPPORTED = "COMM_TYPE_SUPPORTED";

        /*
        * Reader driver type
        */
        public const string RDRDVR_TYPE_READER = "Reader";// general reader
        public const string RDRDVR_TYPE_MTGATE = "MTGate";// meeting gate
        public const string RDRDVR_TYPE_LSGATE = "LSGate";// Library secure gate

        public enum ReaderSupportedComType
        {
            COMMTYPE_COM_EN = 0x01,
            COMMTYPE_USB_EN = 0x02,
            COMMTYPE_NET_EN = 0x04
        }
        /* supported product type */
        public const byte COMMTYPE_COM_EN = 0x01;
        public const byte COMMTYPE_USB_EN = 0x02;
        public const byte COMMTYPE_NET_EN = 0x04;


        //Q value
        public const byte ISO18000p6C_Dynamic_Q = 0xFF;


    }
}
