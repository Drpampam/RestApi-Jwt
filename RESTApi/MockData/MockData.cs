using RESTApi.Models;

namespace RESTApi.MockData
{
    public class MockData
    {
        public static List<User> Users { get; set; } = new List<User>() 
        {
        new User(){UserName = "Johnziland", Email = "johnziland@gmail.com", Password = "johnbravo",
            FirstName = "John", LastName = "Toye", Role = "Admin"},

         new User(){UserName = "Farem_Phil", Email = "powell4christ@gmail.com", Password = "john1234",
            FirstName = "Davids", LastName = "Dele", Role = "Client"}
        };
    }
}
