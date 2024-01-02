using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;


class Program
{
    static void Main()
    {
        List<DB_Data> data = ReadCsvFile("C:\\Users\\Betus\\Desktop\\exhibitA-input.csv");

        List<DB_Data> filteredData = FilterByDate(data, new DateTime(2016, 10, 8));

        Dictionary<int, HashSet<int>> songUserMapping = ProcessData(filteredData);
        Dictionary<int, int> songUserCount = ProcessData2(songUserMapping);
        Dictionary<int, int> finalCounts = ProcessData3(songUserCount);

        WriteResultToCsv("C:\\Users\\Betus\\Desktop\\exhibitA-output.csv", finalCounts);
    }

    static List<DB_Data> ReadCsvFile(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = context => { }
        }))
        {
            return csv.GetRecords<DB_Data>().ToList();
        }
    }

    static List<DB_Data> FilterByDate(List<DB_Data> data, DateTime targetDate)
    {
        return data.Where(item => item.PLAY_TS.Date == targetDate.Date).ToList();
    }

    static Dictionary<int, HashSet<int>> ProcessData(List<DB_Data> data)
    {
        Dictionary<int, HashSet<int>> songUserMapping = new Dictionary<int, HashSet<int>>();

        foreach (var item in data)
        {
            if (!songUserMapping.ContainsKey(item.CLIENT_ID))
            {
                songUserMapping[item.CLIENT_ID] = new HashSet<int>();
            }

            songUserMapping[item.CLIENT_ID].Add(item.SONG_ID);
        }

        return songUserMapping;
    }

    static Dictionary<int, int> ProcessData2(Dictionary<int, HashSet<int>> data)
    {
        Dictionary<int, int> songUserCount = new Dictionary<int, int>();

        foreach (var clientId in data.Keys)
        {
            songUserCount[clientId] = data[clientId].Count;
        }

        return songUserCount;
    }

    static Dictionary<int, int> ProcessData3(Dictionary<int, int> data)
    {
        Dictionary<int, int> songUserCount = new Dictionary<int, int>();

        foreach (var item in data)
        {
            if (!songUserCount.ContainsKey(item.Value))
            {
                songUserCount[item.Value] = 1;
            }
            else
            {
                songUserCount[item.Value]++;
            }
        }

        return songUserCount;
    }

    static void WriteResultToCsv(string filePath, Dictionary<int, int> result)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("DISTINCT_PLAY_COUNT\tCLIENT_COUNT");
            foreach (var kvp in result)
            {
                writer.WriteLine(kvp.Key + "\t" + kvp.Value);
            }
        }
    }
}

class DB_Data
{
    public string PLAY_ID { get; set; }
    public int SONG_ID { get; set; }
    public int CLIENT_ID { get; set; }
    public DateTime PLAY_TS { get; set; }
}