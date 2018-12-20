using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Schedule
{
	public class Message : ModuleBase
	{
		public static HttpClient client = new HttpClient();

		public class GeneralSchedules
		{
			[JsonProperty("result")]
			public Schedule[] Schedules { get; set; }
		}

		public class Schedule
		{
			[JsonProperty("rule")]
			public string Rule { get; set; }

			[JsonProperty("maps")]
			public string[] Maps { get; set; }

			[JsonProperty("stage")]
			public Stage Stage { get; set; }

			[JsonProperty("weapons")]
			public Weapon[] Weapons { get; set; }

			[JsonProperty("start")]
			public DateTime Start { get; set; }

			[JsonProperty("end")]
			public DateTime End { get; set; }
		}

		public class Stage
		{
			[JsonProperty("name")]
			public string Name { get; set; }
		}

		public class Weapon
		{
			[JsonProperty("name")]
			public string Name { get; set; }
		}

		[Command("リグマ")]
		public async Task GetLeagueMatch()
		{
			var leagueSchedule = await GetLeagueMatchSchedulesAsync();
			var message = string.Join("\r\n\r\n", leagueSchedule.Schedules.Where(s => s.Start < DateTime.Today.AddDays(1)).Select(s => "ルール : " + s.Rule.Replace("ガチ", "リーグ") + "\r\n" + "ステージ : " + string.Join(",", s.Maps) + "\r\n" + "開始 : " + s.Start.ToString("yyyy/MM/dd H") + "時" + "\r\n" + "終了 : " + s.End.ToString("yyyy/MM/dd H") + "時"));
			await ReplyAsync(message);
		}

		[Command("ガチマ")]
		public async Task GetGachiMatch()
		{
			var gatimaSchedule = await GetGachiMatchSchedulesAsync();
			var message = string.Join("\r\n\r\n", gatimaSchedule.Schedules.Where(s => s.Start < DateTime.Today.AddDays(1)).Select(s => "ルール : " + s.Rule + "\r\n" + "ステージ : " + string.Join(",", s.Maps) + "\r\n" + "開始 : " + s.Start.ToString("yyyy/MM/dd H") + "時" + "\r\n" + "終了 : " + s.End.ToString("yyyy/MM/dd H") + "時"));
			await ReplyAsync(message);
		}

		[Command("バイト")]
		public async Task GetCoop()
		{
			var coopSchedule = await GetCoopSchedulesAsync();
			var message = string.Join("\r\n\r\n", coopSchedule.Schedules.Where(s => s.Start < DateTime.Today.AddDays(1)).Select(s => "ステージ : " + s.Stage.Name + "\r\n" + "ブキ : " + string.Join(", ", s.Weapons.Select(w => w.Name)) + "\r\n" + "開始 : " + s.Start.ToString("yyyy/MM/dd H") + "時" + "\r\n" + "終了 : " + s.End.ToString("yyyy/MM/dd H") + "時"));
			await ReplyAsync(message);
		}

		private async Task<GeneralSchedules> GetLeagueMatchSchedulesAsync()
		{
			var result = await (await client.GetAsync(@"https://spla2.yuu26.com/league/schedule")).Content.ReadAsStringAsync();
			 return JsonConvert.DeserializeObject<GeneralSchedules>(result);
		}

		private async Task<GeneralSchedules> GetGachiMatchSchedulesAsync()
		{
			var result = await (await client.GetAsync(@"https://spla2.yuu26.com/gachi/schedule")).Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<GeneralSchedules>(result);
		}

		private async Task<GeneralSchedules> GetCoopSchedulesAsync()
		{
			var result = await (await client.GetAsync(@"https://spla2.yuu26.com/coop/schedule")).Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<GeneralSchedules>(result);
		}
	}
}
