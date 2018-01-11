namespace ProjectManagement.Model
{
    public class ProjectPI
    {
        public ProjectPI(int id, string title, string firstName, string lastName, string initialDate)
        {
            this.Id = id;
            this.Title = title;
            this.PIFirstName = firstName;
            this.PILastName = lastName;
            this.InitialDate = initialDate;
        }

        public ProjectPI()
        {
            // TODO: Complete member initialization
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string PIFirstName { get; set; }
        public string PILastName { get; set; }
        public string InitialDate { get; set; }
    }
}
