using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
class Startup 
{
	static async Task Main() 
	{
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine("Acid MassTimeout.");
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write("Enter Token: ");
		Console.ForegroundColor = ConsoleColor.White;
		string AccountToken = Console.ReadLine().ToString();
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write("Enter Guild ID That You Want To Mass-Timeout: ");
		Console.ForegroundColor = ConsoleColor.White;
		string TimeoutingGuild = Console.ReadLine().ToString();
		Console.ForegroundColor = ConsoleColor.Blue;
		await Task.Run(()=>StartAsync(AccountToken,TimeoutingGuild));
		Console.ForegroundColor = ConsoleColor.White;
	}
	static async Task StartAsync(string token,string guild) 
	{
		var members;
		string url = "https://discord.com/api/v9/users/@me";
		var muteDate = DateTime.Now.AddSeconds(2240000).ToString("o");
		using (HttpClient client = new HttpClient()) 
		{
			client.DefaultRequestHeaders.Add("Authorization",token);
			var request = client.GetAsync(url);
			if (request.Result.StatusCode.ToString().Contains("OK"))
			{
				members = File.ReadLines(@"Members.txt");
				foreach (var member in members) 
				{
					var massTimeout = await client.PatchAsync($"https://discord.com/api/v9/guilds/{guild}/members/{member}",content:JsonContent.Create(new {communication_disabled_until=$"{muteDate}"}));
					if (massTimeout.IsSuccessStatusCode) 
					{
						Console.WriteLine($"{member} Is Timed Out For 28Days!");
					}
				}
			}
			else 
			{
				client.DefaultRequestHeaders.Remove("Authorization");
				client.DefaultRequestHeaders.Add("Authorization",$"Bot {token}");
				try {
					members = File.ReadLines(@"Members.txt.txt");
				} catch (Exception e) {
					members = File.ReadLines(@"Members.txt");
				}
				foreach (var member in members) 
				{
					var massTimeout = await client.PatchAsync($"https://discord.com/api/v9/guilds/{guild}/members/{member}",content:JsonContent.Create(new {communication_disabled_until=$"{muteDate}"}));
					if (massTimeout.IsSuccessStatusCode) 
					{
						Console.WriteLine($"{member} Is Timed Out!");
					}
				}
			}
		}
	}
}
