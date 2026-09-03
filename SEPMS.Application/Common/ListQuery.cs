namespace SEPMS.Application.Common
{
    public class ListQuery
    {
        public string? Search { get; set; }
        public string Status { get; set; } = "all";
        public string Sort { get; set; } = "name";
        public string Dir { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int? DepartmentId { get; set; }
        public int TotalInDatabase { get; set; }
        public int ActiveInDatabase { get; set; }

        public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)Math.Max(1, PageSize)));
        public int FromItem => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;
        public int ToItem => Math.Min(Page * PageSize, TotalCount);

        public Dictionary<string, string> Route(int? page = null, string? sort = null, string? dir = null, int? pageSize = null)
        {
            var values = new Dictionary<string, string>
            {
                ["page"] = (page ?? Page).ToString(),
                ["pageSize"] = (pageSize ?? PageSize).ToString(),
                ["sort"] = sort ?? Sort,
                ["dir"] = dir ?? Dir,
                ["status"] = Status
            };

            if (!string.IsNullOrWhiteSpace(Search))
            {
                values["search"] = Search.Trim();
            }

            if (DepartmentId is int departmentId)
            {
                values["departmentId"] = departmentId.ToString();
            }

            return values;
        }

        public string NextDir(string column) =>
            string.Equals(Sort, column, StringComparison.OrdinalIgnoreCase) && Dir == "asc"
                ? "desc"
                : "asc";

        public static ListQuery Create(
            string? search,
            string? status,
            string? sort,
            string? dir,
            int page,
            int pageSize,
            string[] allowedSorts,
            int? departmentId = null)
        {
            return new ListQuery
            {
                Search = string.IsNullOrWhiteSpace(search) ? null : search.Trim(),
                Status = status is "active" or "inactive" ? status : "all",
                Sort = allowedSorts.Contains(sort) ? sort! : "name",
                Dir = dir == "desc" ? "desc" : "asc",
                Page = Math.Max(1, page),
                PageSize = pageSize is 5 or 25 ? pageSize : 10,
                DepartmentId = departmentId is > 0 ? departmentId : null
            };
        }
    }

    public class PagedResult<T> : ListQuery
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    }
}
