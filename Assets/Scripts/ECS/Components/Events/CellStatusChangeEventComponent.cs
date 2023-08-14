using System.Collections.Generic;

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
