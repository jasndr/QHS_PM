using System;

namespace ProjectManagement.Dtos
{
    //internal class PaperCoAuthorAffil
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }        
    //    public string Type { get; set; }
    //}

    internal class PaperHistory
    {
        public int Id { get; set; }
        public int PubType { get; set; }
        public DateTime PaperDate { get; set; }
        public string Name { get; set; }
        public string JournalName { get; set; }
        public string ConfName { get; set; }
    }
}