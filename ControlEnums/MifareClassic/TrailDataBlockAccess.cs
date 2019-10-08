using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL8000_NFCReader.MifareClassicControlEnums
{
    public enum TrailDataBlockAccess
    {
        KeyA_OnlyWrite_A__AccessCon_OnlyRead_A__KeyB_ReadWrite_Any = 0,
        KeyA_ReadWrite_None__AccessCon_OnlyRead_A__KeyB_OnlyRead_Any = 2,
        KeyA_OnlyWrite_B__AccessCon_OnlyRead_Any__KeyB_OnlyWrite_B = 1,
        KeyA_ReadWrite_None__AccessCon_OnlyRead_Any__KeyB_ReadWrite_None = 3,//?or 7
        KeyA_OnlyWrite_A__AccessCon_ReadWrite_A_KeyB__ReadWrite_A = 4,
        KeyA_Write_B__AccessCon_Read_Any_Write_B__KeyB_Write_B = 6,
        KeyA_ReadWrite_None__AccessCon_Read_Any_Write_B__KeyB_ReadWrite_None = 5,
        //KeyA_ReadWrite_None__AccessCon_OnlyRead_Any__KeyB_ReadWrite_None = 7,
    }
}
