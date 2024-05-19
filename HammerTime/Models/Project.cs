namespace HammerTime.Models
{
    internal class Project
    {
        public string Name { get; private set; } = string.Empty;
        public bool IsEmpty => string.IsNullOrEmpty(Name);

        public void Update(string projectName) => Name = projectName;
    
        public void Empty() => Name = string.Empty;

        public override bool Equals(object? obj)
        {
            return Equals((string)obj!);
        }

        public bool Equals(string name)
        {
            return !string.IsNullOrEmpty(name) && Name == name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name.GetHashCode());
        }
    }
}
