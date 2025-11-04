using stops.Domain.Model.Commands.Geographic;

namespace stops.Domain.Model.Aggregates.Geographic
{
    public class Region
    {
        public int Id { get; }
        public string Name { get; set; }

        protected Region()
        { 
            Id = 0;
            Name = string.Empty;
        }
        public Region(CreateRegionCommand command)
        {
            Id = command.Id;
            Name = command.Name;
        }
    }
}
