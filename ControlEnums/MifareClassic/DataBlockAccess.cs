using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL8000_NFCReader.MifareClassicControlEnums
{
    public enum DataBlockAccess
    {
        ReadWriteAny_IncDecAnyKey = 0,
        OnyRead_Any_NoIncDec = 2,
        Read_Any_Write_B_NoIncDec = 1,
        Read_Any_Write_B_Inc_B_Dec_Any = 3,
        OnlyRead_Any_OnyDec_Any = 4,
        OnlyReadWrite_B = 6,
        OnyRead_B = 5,
        LockOut = 7
    }
}
