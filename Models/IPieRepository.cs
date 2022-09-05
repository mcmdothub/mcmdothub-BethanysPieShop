namespace mcmdothub_BethanysPieShop.Models
{
    // used to wrap all the data interaction logic 
    // make sure that the consuming classes in my application are just talking with this Repository
    public interface IPieRepository
    {
        IEnumerable<Pie> AllPies { get; }
        IEnumerable<Pie> PiesOfTheWeek { get; }
        Pie? GetPieById(int pieId);
    }
}
