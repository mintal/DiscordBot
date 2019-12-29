﻿using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace TestBot.Modules
{
    //https://secure.runescape.com/m=hiscore_oldschool/hiscorepersonal?user1=arrow
    public class SkillData
    {
        public float rank { get; private set; }
        public float level { get; private set; }
        public float xp { get; private set; }

        public SkillData(string rank, string level, string xp)
        {
            this.rank = float.Parse(rank,System.Globalization.CultureInfo.InvariantCulture);
            this.level = float.Parse(level, System.Globalization.CultureInfo.InvariantCulture);
            this.xp = float.Parse(xp, System.Globalization.CultureInfo.InvariantCulture);
        }
    }


    public class MiniData
    {
        public float rank { get; private set; }
        public float score { get; private set; }

        public MiniData(string rank, string score)
        {
            this.rank = float.Parse(rank, System.Globalization.CultureInfo.InvariantCulture);
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class BossData
    {
        public float rank { get; private set; }
        public float score { get; private set; }
        public float kph { get; private set; }
        public float ehb { get; private set; }

        public BossData(string rank, string score, string kph)
        {
            this.rank = float.Parse(rank, System.Globalization.CultureInfo.InvariantCulture);
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
            this.kph = float.Parse(kph, System.Globalization.CultureInfo.InvariantCulture);
        }

        public void CalculateEHB(string score)
        {
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
            ehb = (this.score <= 0 || this.kph <= 0) ? 0.0f : this.score / this.kph;
        }
    }


    public class RunescapeHS
    {
        /*public readonly List<string> skills = new List<string> {
            "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged",
            "Prayer", "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing",
            "Firemaking", "Crafting", "Smithing", "Mining", "Herblore",
            "Agility", "Thieving", "Slayer", "Farming", "Runecraft", "Hunter",
            "Construction"
        };*/
        /*public readonly List<string> minigames = new List<string> {
            "Unknown Minigame", "Bounty Hunter (Hunter)",
            "Bounty Hunter (Rogue)", "Clue Scrolls (all)", "UNKNOWN",
            "Clue Scrolls (easy)", "Clue Scrolls (medium)",
            "Clue Scrolls (hard)", "Clue Scrolls (elite)",
            "Clue Scrolls (master)", "LMS (rank)"
        };*/
        /*public readonly List<string> bossNames = new List<string>
        {
            "Abyssal Sire", "Alchemical Hydra", "Barrows Chests", "Bryophyta",
            "Chambers of Xeric", "Chambers of Xeric CM",
            "Chaos Elemental", "Chaos Fanatic", "Commander Zilyana",
            "Corporeal Beast", "Crazy Archaelogist", "Dagannoth Prime",
            "Dagannoth Rex", "Dagannoth Supreme", "Deranged Archaelogist",
            "General Graardor", "Giant Mole", "Grotesque Guardians", "Hespori",
            "Kalphite Queen", "King Black Dragon", "Kraken", "Kree'Arra",
            "K'ril Tsutsaroth", "Thermonuclear Smoke Devil", "Obor",
            "Sarachnis", "Scorpia", "Skotizo", "The Gauntlet", "The Corrupted Gauntlet?",
            "Theatre of Blood", "Thermonuclear Smoke Devil", "TzKal-Zuk", "TzTok-Jad",
            "Venenatis", "Vet'ion", "Vorkath", "Wintertodt", "Zalcano",
            "Zulrah"
        };*/

        //private Dictionary<string, SkillData> skillData = new Dictionary<string, SkillData>();
        //private Dictionary<string, MiniData> miniData = new Dictionary<string, MiniData>();
        //private Dictionary<string, BossData> bossData = new Dictionary<string, BossData>();
        //private int DISCORD_MAX_CHAR_LIMIT = 2000;

        /*private readonly List<string> killsPerHour = new List<string>
        {
            "0", "27", "0", "0", "3.5", "1.8", "48", "100", "25", "14", "0", "96", "96", "96", "0", "30", "90",
            "30", "0", "35", "100", "80", "28", "36", "45", "100", "10", "7.5", "0", "0", "0", "3", "110",
            "0.8", "2", "44", "30", "32", "0", "0", "35"
        };*/

        private string HSData;

        public RunescapeHS(string playerName)
        {
            WebClient client = new WebClient();
            string rs_url = "http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player=";
            rs_url += playerName;
            string hsData = string.Empty;
            try
            {
                HSData = client.DownloadString(rs_url);
                //ParseBossData(hsData);
            }
            catch (WebException)
            {
                HSData = string.Empty;
            }
        }

        /*public Dictionary<string, SkillData> GetSkillData()
        {
            return skillData;
        }*/

        public Dictionary<string, BossData> GetBossData()
        {
            string text = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Data/boss_data.json");
            Dictionary<string, BossData> bossData = JsonConvert.DeserializeObject<Dictionary<string, BossData>>(text);

            /*foreach(string boss in bossData.Keys)
            {
                Console.WriteLine(boss);
                Console.WriteLine("\t" + bossData[boss].rank);
                Console.WriteLine("\t" + bossData[boss].score);
                Console.WriteLine("\t" + bossData[boss].kph);
                Console.WriteLine("\t" + bossData[boss].ehb);
            }*/

            return bossData;
        }

        public List<string> GetBossTable()
        {
            List<string> table = new List<string>();
            return table;
        }
        /*public Dictionary<string, MiniData> GetMiniGameData()
        {
            return miniData;
        }*/
        public List<string> GetBossDataTable()
        {
            Dictionary<string, BossData> bossData = GetBossData();
            string[] dataLines = HSData.Split('\n');
            int bossIndex = 36;
            int bossCount = 0;

            List<List<string>> tableContents = new List<List<string>>();
            foreach (string boss in bossData.Keys)
            {
                string[] data = dataLines[bossIndex].Split(',');
                bossData[boss].CalculateEHB(data[1]);
                bossIndex++;

                List<string> bossContents = new List<string>
                {
                    boss,
                    bossData[boss].score.ToString(),
                    bossData[boss].kph.ToString(),
                    bossData[boss].ehb.ToString("n2")
                };
                tableContents.Add(bossContents);

                Console.WriteLine(boss);
                Console.WriteLine("\t" + bossData[boss].rank);
                Console.WriteLine("\t" + bossData[boss].score);
                Console.WriteLine("\t" + bossData[boss].kph);
                Console.WriteLine("\t" + bossData[boss].ehb);
                bossCount++;
            }

            string[] columnNames = { "Boss", "Kills", "KPH", "EHB"};
            int[] columnWidths = { 28, 9, 5, 9 };

            Console.WriteLine("!!!!!!!!!!!!!!!!!!");

            DiscordTable table = new DiscordTable(
                columnNames, columnWidths, bossCount, tableContents
            );
            Console.WriteLine("~~~~~~~~~~");
            Console.WriteLine("DISCORD TABLE\n" + table.Table);

            List<string> splitTable = DiscordMessage.SplitTable(table.Table);
            return DiscordMessage.ToCodeBlock(splitTable);
        }
        /*
        private void ParseHSData(string hsData)
        {
            GetBossData();

            string[] dataLines = hsData.Split('\n');
            int dataIndex = 0;
            foreach(string skill in skills)
            {
                string[] data = dataLines[dataIndex].Split(',');
                skillData[skill] = new SkillData(data[0], data[1], data[2]);
                dataIndex++;
            }

            foreach(string mini in minigames)
            {
                string[] data = dataLines[dataIndex].Split(',');
                miniData[mini] = new MiniData(data[0], data[1]);
                dataIndex++;
            }

            foreach(string boss in bossNames)
            {
                string[] data = dataLines[dataIndex].Split(',');
                bossData[boss] = new MiniData(data[0], data[1]);
                dataIndex++;
            }
        }*/

        /*public List<string> GetBossDataTable()
        {
            List<string> l = new List<string>();
            return l;
        }*/

        /*public List<string> GetBossDataTable2()
        {
            string table = "";
            int bossFieldLength = 28;  //must be even
            int numKillsLength = 9;    //must be odd
            int kphsLength = 5;        //must be odd
            int ehbLength = 9;         //must be odd
            int numDividers = 5;       //number of |

            string horizBorder = "";
            for(int i = 0; i < bossFieldLength + numKillsLength + kphsLength + ehbLength + numDividers; i++)
            {
                horizBorder += '-';
            }
            table += horizBorder + "\n|";
            table += GetCentered(bossFieldLength, "Boss");
            table += GetCentered(numKillsLength, "Kills");
            table += GetCentered(kphsLength, "KPH");
            table += GetCentered(ehbLength, "EHB") + "\n";
            table += horizBorder + "\n";
            
            int bossCounter = 0;
            foreach (string boss in bossNames)
            {
                // Boss column (+ 1 for the leading white space)
                int whiteSpace = bossFieldLength - (boss.Length + 1);
                table += "| " + boss;
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += "|";

                // Kills column (+1 for the trailing white space)
                MiniData entry = bossData[boss];
                whiteSpace = numKillsLength - (entry.score.ToString().Length + 1);
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += entry.score.ToString() + " |";

                // Kills per hour (KPH), (+1 for the leading white space)
                whiteSpace = kphsLength - (killsPerHour[bossCounter].Length + 1);
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += killsPerHour[bossCounter] + " |";

                // Efficient Hours Bossed (EHB), (+1 for the leading white space)
                float ehb = 0.0f;
                float kills = float.Parse(killsPerHour[bossCounter], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                if (kills > 0.0f && entry.score > 0.0f)
                {
                    ehb = entry.score / kills;
                }

                string theEHB = ehb.ToString("n2");
                whiteSpace = ehbLength - (theEHB.Length + 1);
                for (int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += theEHB + " |";
                bossCounter++;
                table += "\n";
            }

            List<string> tableParts = new List<string>();
            if (table.Length > DISCORD_MAX_CHAR_LIMIT)
            {
                string[] tableLines = table.Split('\n');
                int lineLength = tableLines[0].Length;

                int limit = (DISCORD_MAX_CHAR_LIMIT / lineLength) - 1;
                int linesAddedCounter = 0;
                string tableLineEntry = "";

                for (int i = 0; i < tableLines.Length; i++)
                {
                    if (linesAddedCounter == limit || i == tableLines.Length - 1)
                    {
                        linesAddedCounter = 0;
                        tableParts.Add("```" + tableLineEntry + "```");
                        tableLineEntry = "";
                    }

                    tableLineEntry += tableLines[i] + "\n";
                    linesAddedCounter++;
                }
            }
            else
            {
                tableParts.Add("```" + table + "```");
            }

            return tableParts;
        }*/

        private string GetCentered(int length, string name)
        {
            // length: the number of characters name should be centered on
            // name: the string to center in the given length
            string padding = "";
            int nameLength = name.Length;
            float center = length / 2.0f - (nameLength / 2.0f);

            for(int i = 0; i < center; i++)
            {
                padding += " ";
            }

            return padding + name + padding + "|";
        }
    }
}

/*
//player class - from Arrow
public Discord.Embed GetBossEmbedded(string name)
{
    if (bossNames.Contains(name))
    {
        MiniData boss = bossData[name];
        int index = bossNames.IndexOf(name);
        var builder = new Discord.EmbedBuilder();
        string kills = killsPerHour[index];
        string ehb = float.Parse(kills) > 0.0f && boss.score > 0.0f ? (boss.score / float.Parse(kills)).ToString() : "0";
        builder.WithTitle(name);
        builder.ThumbnailUrl = "https://oldschool.runescape.wiki/images/thumb/9/9b/Zulrah_%28tanzanite%2C_Christmas%29.png/250px-Zulrah_%28tanzanite%2C_Christmas%29.png?e8bc2";
        builder.AddField(GetField("Rank", boss.rank.ToString()));
        builder.AddField(GetField("Kills", boss.score.ToString()));
        builder.AddField(GetField("KPH", kills));
        builder.AddField(GetField("EHB", ehb));

        return builder.Build();
    }
    return null;
}

private Discord.EmbedFieldBuilder GetField(string name, string value)
{
    var field = new Discord.EmbedFieldBuilder();
    field.Name = name;
    field.Value = value;
    field.IsInline = true;

    return field;
}
//response
var embed = player.GetBossEmbedded("Zulrah");
await ReplyAsync(null, false, embed);

    //await ReplyAsync(null   - message, false - text to speech?, embed  - embedslot );

*/