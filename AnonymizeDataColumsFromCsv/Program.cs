using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace AnonymizeDataColumsFromCsv
{
  class Program
  {
    static void Main(string[] args)
    {
      // make a stringbuilder for the logfile
      StringBuilder sbLog = new StringBuilder();
      sbLog.AppendLine($"Anonymize csv-files started at: { DateTime.Now}");
      AnonymizeCsvFile(ConfigurationManager.AppSettings["Importfile1"], ConfigurationManager.AppSettings["Exportfile1"], ref sbLog);
      AnonymizeCsvFile(ConfigurationManager.AppSettings["Importfile2"], ConfigurationManager.AppSettings["Exportfile2"], ref sbLog);
      AnonymizeCsvFile(ConfigurationManager.AppSettings["Importfile3"], ConfigurationManager.AppSettings["Exportfile3"], ref sbLog);
      sbLog.AppendLine($"Ended at: { DateTime.Now}");
      WriteLogFile(sbLog);
    }

    /// <summary>
    /// Write the logfile to a textfile if possible. If not, then show the error message and show the log messages too
    /// </summary>
    /// <param name="sbLog"></param>
    private static void WriteLogFile(StringBuilder sbLog)
    {
      string logFilename = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LogFile"]) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.txt") : ConfigurationManager.AppSettings["LogFile"];
      try
      {
        File.WriteAllText(logFilename, $"{sbLog.ToString()}");
      }
      catch (Exception e)
      {
        MessageBox.Show($"Exception: --> can't write to logfile: { logFilename }! Reason: { e.Message }!\n\nLogmessages:\n{sbLog.ToString()}");
      }
    }

    /// <summary>
    /// Anonymize a csv file
    /// </summary>
    /// <param name="importfile"></param>
    /// <param name="exportfile"></param>
    /// <param name="sblogger"></param>
    private static void AnonymizeCsvFile(string importfile, string exportfile, ref StringBuilder sblogger)
    {
      try
      {
        if (!string.IsNullOrEmpty(importfile))
        {
          var dt = new DataTable();
          //Import all data into the datatable
          using (var reader = new StreamReader(importfile, Encoding.GetEncoding(Convert.ToInt32(ConfigurationManager.AppSettings["CodePage"]))))
          using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
          {
            //// Do any configuration to `CsvReader` before creating CsvDataReader.
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Delimiter = ";";
            csv.Configuration.HasHeaderRecord = true;
            using (var dr = new CsvDataReader(csv))
            {
              dt.Load(dr);
            }
          }

          //check columnsnumbers of datatable; must be 44
          if (dt.Columns.Count != 44)
          {
            sblogger.AppendLine($"Exception --> number of columns in importfile must be 44! This is: { dt.Columns.Count } in importfile: { importfile }");
            return;
          }

          //write data from datatable to csv-file
          using (var writer = new StreamWriter(exportfile, false, Encoding.GetEncoding(Convert.ToInt32(ConfigurationManager.AppSettings["CodePage"]))))
          using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { BadDataFound = null, Delimiter = ";" }))
          {
            csv.WriteRecords(ConvertToIEnumarable(dt));
          }
        }
      }
      catch (Exception e)
      {
        sblogger.AppendLine($"Exception -->  { e.Message}");
      }
    }

    /// <summary>
    /// Convert the datatable into an IEnumerable of record type to match csvHelper.WriteRecords 
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static IEnumerable<Record> ConvertToIEnumarable(DataTable table)
    {
      //read columns to remove
      var colummsToRemove = ConfigurationManager.AppSettings["AnonymizeColumnNumbers"].Split(',');
      foreach (DataRow row in table.Rows)
      {
        yield return new Record
        {
          Zoekcode = colummsToRemove.Contains("1") ? "" : row[0].ToString(),
          Achternaam = colummsToRemove.Contains("2") ? "" : row[1].ToString(),
          Voorvoegsel = colummsToRemove.Contains("3") ? "" : row[2].ToString(),
          Voorletters = colummsToRemove.Contains("4") ? "" : row[3].ToString(),
          Voornamen = colummsToRemove.Contains("5") ? "" : row[4].ToString(),
          Partnernaam = colummsToRemove.Contains("6") ? "" : row[5].ToString(),
          Partnervoorvoegsels = colummsToRemove.Contains("7") ? "" : row[6].ToString(),
          AanhefId = colummsToRemove.Contains("8") ? "" : row[7].ToString(),
          TitelsVoor = colummsToRemove.Contains("9") ? "" : row[8].ToString(),
          TitelsAchter = colummsToRemove.Contains("10") ? "" : row[9].ToString(),
          Geslacht = colummsToRemove.Contains("11") ? "" : row[10].ToString(),
          Geboortedatum = colummsToRemove.Contains("12") ? "" : row[11].ToString(),
          Geboorteplaats = colummsToRemove.Contains("13") ? "" : row[12].ToString(),
          Straatnaam = colummsToRemove.Contains("14") ? "" : row[13].ToString(),
          Huisnummer = colummsToRemove.Contains("15") ? "" : row[14].ToString(),
          Postcode = colummsToRemove.Contains("16") ? "" : row[15].ToString(),
          Woonplaats = colummsToRemove.Contains("17") ? "" : row[16].ToString(),
          Land = colummsToRemove.Contains("18") ? "" : row[17].ToString(),
          TelPrive = colummsToRemove.Contains("19") ? "" : row[18].ToString(),
          TelMobiel = colummsToRemove.Contains("20") ? "" : row[19].ToString(),
          TelZakelijk = colummsToRemove.Contains("21") ? "" : row[20].ToString(),
          EmailPrive = colummsToRemove.Contains("22") ? "" : row[21].ToString(),
          Datumindienst = colummsToRemove.Contains("23") ? "" : row[22].ToString(),
          Datumuitdienst = colummsToRemove.Contains("24") ? "" : row[23].ToString(),
          Functie = colummsToRemove.Contains("25") ? "" : row[24].ToString(),
          Afdeling = colummsToRemove.Contains("26") ? "" : row[25].ToString(),
          Inlognaam = colummsToRemove.Contains("27") ? "" : row[26].ToString(),
          Wachtwoord = colummsToRemove.Contains("28") ? "" : row[27].ToString(),
          WerkgeverZoekcode = colummsToRemove.Contains("29") ? "" : row[28].ToString(),
          WerkgeverNaam = colummsToRemove.Contains("30") ? "" : row[29].ToString(),
          WerkgeverNaamExtra = colummsToRemove.Contains("31") ? "" : row[30].ToString(),
          WerkgeverVestiging = colummsToRemove.Contains("32") ? "" : row[31].ToString(),
          DebiteurZoekcode = colummsToRemove.Contains("33") ? "" : row[32].ToString(),
          DebiteurNaam = colummsToRemove.Contains("34") ? "" : row[33].ToString(),
          DebiteurNaamExtra = colummsToRemove.Contains("35") ? "" : row[34].ToString(),
          DebiteurVestiging = colummsToRemove.Contains("36") ? "" : row[35].ToString(),
          ContactZoekcode = colummsToRemove.Contains("37") ? "" : row[36].ToString(),
          ContactNaam = colummsToRemove.Contains("38") ? "" : row[37].ToString(),
          ContactSoort = colummsToRemove.Contains("39") ? "" : row[38].ToString(),
          RolLMSAanmaken = colummsToRemove.Contains("40") ? "" : row[39].ToString(),
          ExtraAutorisatie = colummsToRemove.Contains("41") ? "" : row[40].ToString(),
          RolCursist = colummsToRemove.Contains("42") ? "" : row[41].ToString(),
          Hoofdwerkgever = colummsToRemove.Contains("43") ? "" : row[42].ToString(),
          HoofdBetaler = colummsToRemove.Contains("44") ? "" : row[43].ToString()
        };
      }
    }

    /// <summary>
    /// record class that holds all of the specified 44 columns
    /// </summary>
    private class Record
    {
      public string Zoekcode { get; set; }
      public string Achternaam { get; set; }
      public string Voorvoegsel { get; set; }
      public string Voorletters { get; set; }
      public string Voornamen { get; set; }
      public string Partnernaam { get; set; }
      public string Partnervoorvoegsels { get; set; }
      public string AanhefId { get; set; }
      public string TitelsVoor { get; set; }
      public string TitelsAchter { get; set; }
      public string Geslacht { get; set; }
      public string Geboortedatum { get; set; }
      public string Geboorteplaats { get; set; }
      public string Straatnaam { get; set; }
      public string Huisnummer { get; set; }
      public string Postcode { get; set; }
      public string Woonplaats { get; set; }
      public string Land { get; set; }
      public string TelPrive { get; set; }
      public string TelMobiel { get; set; }
      public string TelZakelijk { get; set; }
      public string EmailPrive { get; set; }
      public string Datumindienst { get; set; }
      public string Datumuitdienst { get; set; }
      public string Functie { get; set; }
      public string Afdeling { get; set; }
      public string Inlognaam { get; set; }
      public string Wachtwoord { get; set; }
      public string WerkgeverZoekcode { get; set; }
      public string WerkgeverNaam { get; set; }
      public string WerkgeverNaamExtra { get; set; }
      public string WerkgeverVestiging { get; set; }
      public string DebiteurZoekcode { get; set; }
      public string DebiteurNaam { get; set; }
      public string DebiteurNaamExtra { get; set; }
      public string DebiteurVestiging { get; set; }
      public string ContactZoekcode { get; set; }
      public string ContactNaam { get; set; }
      public string ContactSoort { get; set; }
      public string RolLMSAanmaken { get; set; }
      public string ExtraAutorisatie { get; set; }
      public string RolCursist { get; set; }
      public string Hoofdwerkgever { get; set; }
      public string HoofdBetaler { get; set; }
    }
  }
}
