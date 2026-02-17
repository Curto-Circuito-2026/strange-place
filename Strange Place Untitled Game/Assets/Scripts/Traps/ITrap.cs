using UnityEngine;

public interface ITrap
{
    bool IsOn {get;set;}
    public void SetState(bool state);
}
