Console.OutputEncoding = System.Text.Encoding.UTF8;

SchedulerService schedulerService = new();

Console.WriteLine("🔁 Solana takip otomasyonu başlatıldı.");
await schedulerService.StartScheduler();

Console.ReadLine();