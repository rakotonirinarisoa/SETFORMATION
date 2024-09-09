using System;

namespace apptab.Data.Entities
{
    public class ProjectDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string AdminEmail { get; set; }
    }
}
