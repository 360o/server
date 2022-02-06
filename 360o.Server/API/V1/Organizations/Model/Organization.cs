using _360o.Server.Api.V1.Stores.Model;
using Ardalis.GuardClauses;
using NpgsqlTypes;

namespace _360o.Server.Api.V1.Organizations.Model
{
    public class Organization : BaseEntity
    {
        public Organization(string userId, string name)
        {
            UserId = Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }

        private Organization()
        {
        }

        public string UserId { get; private set; }

        public string Name { get; private set; }

        public string EnglishShortDescription { get; private set; } = string.Empty;
        public string EnglishLongDescription { get; private set; } = string.Empty;
        public List<string> EnglishCategories { get; private set; } = new List<string>();
        public string EnglishCategoriesJoined { get; private set; } = string.Empty;
        public NpgsqlTsVector EnglishSearchVector { get; private set; }

        public string FrenchShortDescription { get; private set; } = string.Empty;
        public string FrenchLongDescription { get; private set; } = string.Empty;
        public List<string> FrenchCategories { get; private set; } = new List<string>();
        public string FrenchCategoriesJoined { get; private set; } = string.Empty;
        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public List<Store> Stores { get; private set; }

        public void SetName(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }

        public void SetEnglishShortDescription(string englishShortDescription)
        {
            EnglishShortDescription = Guard.Against.Null(englishShortDescription, nameof(englishShortDescription)).Trim();
        }

        public void SetEnglishLongDescription(string englishLongDescription)
        {
            EnglishLongDescription = Guard.Against.Null(englishLongDescription, nameof(englishLongDescription)).Trim();
        }

        public void SetEnglishCategories(ISet<string> categories)
        {
            EnglishCategories = categories.Select(c => c.Trim()).ToList();
            EnglishCategoriesJoined = JoinCategories(EnglishCategories);
        }

        public void SetFrenchShortDescription(string frenchShortDescription)
        {
            FrenchShortDescription = Guard.Against.Null(frenchShortDescription, nameof(frenchShortDescription)).Trim();
        }

        public void SetFrenchLongDescription(string frenchLongDescription)
        {
            FrenchLongDescription = Guard.Against.Null(frenchLongDescription, nameof(frenchLongDescription)).Trim();
        }

        public void SetFrenchCategories(ISet<string> categories)
        {
            FrenchCategories = categories.Select(c => c.Trim()).ToList();
            FrenchCategoriesJoined = JoinCategories(FrenchCategories);
        }

        private string JoinCategories(IReadOnlyList<string> categories) => string.Join(" ", categories);
    }
}