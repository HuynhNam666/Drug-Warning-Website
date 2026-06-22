namespace MedicineWarningAPI.Helpers
{
    public static class RiskLevelHelper
    {
        public const string Low = "Low";
        public const string Medium = "Medium";
        public const string High = "High";
        public const string Critical = "Critical";
        public const string Unknown = "Unknown";
        public const string NoWarningFound = "No warning found";

        public static readonly string[] Values =
        {
            Low,
            Medium,
            High,
            Critical
        };

        public static string Normalize(string? riskLevel)
        {
            if (string.IsNullOrWhiteSpace(riskLevel))
            {
                return Unknown;
            }

            var value = riskLevel.Trim().ToLowerInvariant();

            return value switch
            {
                "critical" or "nguy hiểm" or "rat cao" or "rất cao" => Critical,
                "high" or "cao" or "nguy cơ cao" or "nguy co cao" => High,
                "medium" or "trung bình" or "trung binh" or "thận trọng" or "than trong" => Medium,
                "low" or "thấp" or "thap" or "an toàn" or "an toan" => Low,
                _ => riskLevel.Trim()
            };
        }

        public static bool IsValid(string? riskLevel)
        {
            var normalized = Normalize(riskLevel);
            return Values.Contains(normalized, StringComparer.OrdinalIgnoreCase);
        }

        public static int ToScore(string? riskLevel)
        {
            return Normalize(riskLevel) switch
            {
                Critical => 100,
                High => 80,
                Medium => 50,
                Low => 20,
                _ => 0
            };
        }

        public static string FromScore(int score)
        {
            if (score >= 100) return Critical;
            if (score >= 80) return High;
            if (score >= 50) return Medium;
            if (score >= 20) return Low;
            return NoWarningFound;
        }
    }
}
