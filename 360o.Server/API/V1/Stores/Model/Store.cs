using NpgsqlTypes;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        private Store()
        {
        }

        public Store(string userId, string displayName, string? englishShortDescription, string? englishLongDescription, ISet<string>? englishCategories, string? frenchShortDescription, string? frenchLongDescription, ISet<string>? frenchCategories, Place place)
        {
            UserId = userId;
            DisplayName = displayName;
            EnglishShortDescription = englishShortDescription ?? string.Empty;
            EnglishLongDescription = englishLongDescription ?? string.Empty;
            _englishCategories = englishCategories != null ? englishCategories.ToHashSet() : new HashSet<string>();
            FrenchShortDescription = frenchShortDescription ?? string.Empty;
            FrenchLongDescription = frenchLongDescription ?? string.Empty;
            _frenchCategories = frenchCategories != null ? frenchCategories.ToHashSet() : new HashSet<string>();
            Place = place;
        }

        public string UserId { get; private set; }

        public string DisplayName { get; private set; }

        public string EnglishShortDescription { get; private set; }

        public string EnglishLongDescription { get; private set; }

        public NpgsqlTsVector EnglishSearchVector { get; private set; }

        private readonly HashSet<string> _englishCategories;
        public IReadOnlySet<string> EnglishCategories => _englishCategories;

        public string FrenchShortDescription { get; private set; }

        public string FrenchLongDescription { get; private set; }

        private readonly HashSet<string> _frenchCategories;
        public IReadOnlySet<string> FrenchCategories => _frenchCategories;

        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public Place Place { get; private set; }

        public void AddEnglishCategory(string category)
        {
            _englishCategories.Add(category);
        }

        public void AddFrenchCategory(string category)
        {
            _frenchCategories.Add(category);
        }
    }
}

