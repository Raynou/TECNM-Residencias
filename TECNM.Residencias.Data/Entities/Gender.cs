namespace TECNM.Residencias.Data.Entities;

public sealed class Gender
{
    public long Id { get; set; }

    public string Label { get; set; } = "";

    public override string ToString()
    {
        return Label;
    }
}
