namespace LMSSystem.Helpers
{
    public static class Pagination
    {
        public static List<T> Paginate<T>(List<T> items, int page, int pageSize)
        {
            return items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }
    }
}
