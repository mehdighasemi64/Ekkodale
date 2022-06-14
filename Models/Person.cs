namespace Models.Ekkodale;
public class Person
{
    public int PersonID { get; set; }
    public string? Name { get; set; }
    public string? Family { get; set; }
    public int Age { get; set; }
    public int BirthYear { get; set; }
    public string FullName { get { return (Name + " " + Family); } }
}