
using System.Collections.Generic;
using UnityEngine;

struct CellStatusChangeEventComponent
{
    public List<CellStatusChangeInfo> StatusChangeInfoList;
}

public struct CellStatusChangeInfo
{
    public CellStatus OldStatus;
    public CellStatus NewStatus;

    public Cell Cell;
}
