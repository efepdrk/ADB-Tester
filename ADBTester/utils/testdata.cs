public class TestAction
{
    public DateTime Timestamp { get; set; }
    public string Command { get; set; }
}

public class TestScript
{
    public List<TestAction> Actions { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}