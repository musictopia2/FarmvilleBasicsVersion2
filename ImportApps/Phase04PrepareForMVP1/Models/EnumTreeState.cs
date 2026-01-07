namespace Phase04PrepareForMVP1.Models;
public enum EnumTreeState
{
    Producing,
    //PartialReady,
    Collecting //if you are collecting, then producing will stop.  after you collect, will produce again.
}