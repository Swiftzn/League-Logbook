// See https://aka.ms/new-console-template for more information
using Camille.Enums;
using Camille.RiotGames;

var matchStart = 0;
var matchEnd = 0;
var queue = Queue.SUMMONERS_RIFT_5V5_RANKED_SOLO;
var riotApi = RiotGamesApi.NewInstance("RGAPI-ba015301-ad96-4198-ab56-b2ad78db0718");

Console.WriteLine("Please provide your Summoner Name");
string userName = Console.ReadLine();
Console.WriteLine("how many matches would you like to see");
matchEnd = Convert.ToInt32(Console.ReadLine());

var summoner = riotApi.SummonerV4().GetBySummonerName(PlatformRoute.EUW1, userName);
var name = summoner.Name;
var level = summoner.SummonerLevel;
var accountId = summoner.AccountId;
var puuId = summoner.Puuid;
Console.WriteLine("============================================");
Console.WriteLine("Summoner Info");
Console.WriteLine("============================================");
Console.WriteLine("Summoner Name : " + name);
Console.WriteLine("Account ID    : " + accountId);
Console.WriteLine("PUUID         : " + puuId);
Console.WriteLine("Summoner Level: " + level);
Console.WriteLine("============================================");

var matchlist = riotApi.MatchV5().GetMatchIdsByPUUIDAsync(RegionalRoute.EUROPE, puuId, matchEnd, null, queue).Result;
foreach (var match in matchlist)
{
    var matchDetails = riotApi.MatchV5().GetMatchAsync(RegionalRoute.EUROPE, match).Result;
    var gDuration = matchDetails.Info.GameDuration;
    var gDate = DateTimeOffset.FromUnixTimeMilliseconds(matchDetails.Info.GameStartTimestamp);
    var summonerdata = matchDetails.Info.Participants.FirstOrDefault(p => p.Puuid == puuId);
    var sLane = summonerdata.IndividualPosition;
    decimal sKills = summonerdata.Kills;
    decimal sDeaths = summonerdata.Deaths;
    decimal sAssists = summonerdata.Assists;
    var sDuration = TimeSpan.FromSeconds(summonerdata.TimePlayed);
    var sCS = summonerdata.NeutralMinionsKilled + summonerdata.TotalMinionsKilled;
    var sChampion = summonerdata.ChampionName;
    var sResult = "";
    decimal sKDA = 0;
    if (sDeaths == 0)
    {
        sKDA = Math.Round((sKills + sAssists), 2);
    }
    else
    {
        sKDA = Math.Round((sKills + sAssists) / sDeaths, 2);
    }

    if (summonerdata.Win == true)
    {
        sResult = "Victory";
    }
    else
    {
        sResult = "Defeat";
    }

    Console.WriteLine("============================================");
    Console.WriteLine("Match ID: " + match);
    Console.WriteLine("============================================");
    Console.WriteLine("Date         : " + gDate.ToString("dd/MM/yyy"));
    Console.WriteLine("Result       : " + sResult);
    Console.WriteLine("Game Duration: " + sDuration);
    Console.WriteLine("Lane         : " + sLane);
    Console.WriteLine("Champion     : " + sChampion);
    Console.WriteLine("K/D/A        : " + sKills + "/" + sDeaths + "/" + sAssists);
    Console.WriteLine("KDA          : " + String.Format("{0:0.00}", sKDA));
    Console.WriteLine("Total CS     : " + sCS);
    Console.WriteLine("============================================");
}

