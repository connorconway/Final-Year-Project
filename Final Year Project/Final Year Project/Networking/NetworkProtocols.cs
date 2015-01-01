namespace Multiplayer_Software_Game_Engineering.Networking
{
    public enum Protocol
    {
        Disconnected  = 0,
        Connected     = 2,
        PlayerMoved   = 3,
        BulletCreated = 4,
        GameOver      = 5,
        SyncGame      = 6,
        CheckForHosts = 7
    }
}