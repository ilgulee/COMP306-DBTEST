namespace COMP306_DBTEST.Models
{
    public enum District:byte
    {
        [System.ComponentModel.Description("East York")]
        EastYork=1,
        Etobicoke=2,
        [System.ComponentModel.Description("North York")]
        NorthYork=3,
        Scarborough=4,
        Toronto=5,
        York=6
    }
}