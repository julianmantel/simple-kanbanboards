namespace SimpleKanbanBoards.Business.Models.Project
{
    public class UpdateProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MaxDevs { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}