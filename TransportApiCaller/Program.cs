using System;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

namespace TransportApiCaller
{
    class Program
    {
        static string storageLocation;
        static string transportAPIURL = $"https://transportapi.com/v3/uk/";
        static string appID = "c3d87955";
        static string appKEY = "d5a38cdf52fdce186e6338cfa5c5603c";

        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                storageLocation = args[0];
                
                Task.Run(async () => {

                    while (true)
                    {
                        if(DateTime.Now > DateTime.Now.Date.Add(new TimeSpan(8,30,0)) && DateTime.Now < DateTime.Now.Date.Add(new TimeSpan(18,30,0)))
                        {
                            FetchTrainDepartures("TBD", "GTW");
                            FetchBusDepartures(
                                "4400CY0088", 
                                "4400CY0087", 
                                "4400CY0399", 
                                "4400CY0400",
                                "4400CY0137",
                                "4400CY0138");
                        }


                        FetchTrafficInformation();
                        Console.WriteLine("Done");
                        await Task.Delay(TimeSpan.FromMinutes(5));

                    }

                });
            }
            

            Console.ReadLine();
        }

        /// <summary>
        /// Fetches the Railway Departure Board from Transport Api 
        /// </summary>
        private static async void FetchTrainDepartures(string threeBridges, string gatwick)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] TBDapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}train/station/{threeBridges}/live.json?app_id={appID}&app_key={appKEY}&darwin=false&train_status=passenger");
                    byte[] GTWapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}train/station/{gatwick}/live.json?app_id={appID}&app_key={appKEY}&darwin=false&train_status=passenger");

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/{threeBridges}_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(TBDapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/{gatwick}_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(GTWapiResponse));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="flemingWayEastWB"></param>
        /// <param name="flemingWayEastEB"></param>
        /// <param name="gatwickRoadCentralSWB"></param>
        /// <param name="gatwickRoadCentralNEB"></param>
        /// <param name="manorRoyalEastEB"></param>
        /// <param name="manorRoyalEastWB"></param>
        private static async void FetchBusDepartures(
            string flemingWayEastWB, 
            string flemingWayEastEB,
            string gatwickRoadCentralSWB,
            string gatwickRoadCentralNEB,
            string manorRoyalEastEB,
            string manorRoyalEastWB)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    
                    byte[] FlemingWayEastWBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{flemingWayEastWB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");
                    byte[] FlemingWayEastEBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{flemingWayEastEB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");
                    byte[] GatwickRoadCentralSWBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{gatwickRoadCentralSWB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");
                    byte[] GatwickRoadCentralNEBBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{gatwickRoadCentralNEB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");
                    byte[] ManorRoyalEBBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{manorRoyalEastEB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");
                    byte[] ManorRoyalEWBapiResponse = await wc.DownloadDataTaskAsync($"{transportAPIURL}bus/stop/{manorRoyalEastWB}/live.json?app_id={appID}&app_key={appKEY}&group=no&nextbuses=yes");


                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/flemingWayEast_WB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(FlemingWayEastWBapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/flemingWayEast_EB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(FlemingWayEastEBapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/gatwickRoadCentral_SWB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(GatwickRoadCentralSWBapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/gatwickRoadCentral_NEB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(GatwickRoadCentralNEBBapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/manorRoyalEast_EB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(ManorRoyalEBBapiResponse));
                    }

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/manorRoyalEast_WB_departures.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(ManorRoyalEWBapiResponse));
                    }

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// Fetches Traffice Information from Highways England
        /// </summary>
        private static async void FetchTrafficInformation()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] TrafficapiResponse = await wc.DownloadDataTaskAsync("https://highwaysengland.co.uk/wp-content/themes/x-child/getUnplannedIncidents.php");

                    using (StreamWriter sw = new StreamWriter($"{storageLocation}/TrafficInfo.json"))
                    {
                        sw.Write(Encoding.UTF8.GetString(TrafficapiResponse));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
