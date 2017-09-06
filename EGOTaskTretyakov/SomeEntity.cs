using SQLite;

namespace EGOTaskTretyakov
{
    public class SomeEntity
    {
        [PrimaryKey,AutoIncrement, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Updated { get; set; }
    }
}