namespace VelesLibrary.DTOs;

public struct GroupNick
{
    public GroupNick(int groupId, string nick)
    {
        GroupId = groupId;
        Nick = nick ?? throw new ArgumentNullException(nameof(nick));
    }

    public int GroupId { get; set; }
    public string Nick { get; set; }
}