using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppCreativity.PCL.GentseFeesten.Model;
using Newtonsoft.Json;
using PCLStorage;
using PortableRest;

namespace AppCreativity.PCL.GentseFeesten.Framework
{
    public class GFService
    {
        private RestClient _restClient;
        private string _restKey = "XeqAsustujew6re3&secret=trezenu3uzuDrecE4upruCruq5ba4tec";
        private string _baseUrlEvents = "http://gfapi.timleytens.be/gf-api/events";

        #region Constructor
        private static GFService s_Instance;
        private static object s_InstanceSync = new object();

        protected GFService()
        {
            _restClient = new RestClient();           
        }

        /// <summary>
        /// Returns an instance (a singleton)
        /// </summary>
        /// <returns>a singleton</returns>
        /// <remarks>
        /// This is an implementation of the singelton design pattern.
        /// </remarks>
        public static GFService GetInstance()
        {
            // This implementation of the singleton design pattern prevents 
            // unnecessary locks (using the double if-test)
            if (s_Instance == null)
            {
                lock (s_InstanceSync)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new GFService();
                    }
                }
            }
            return s_Instance;
        }
        #endregion

        public async Task<List<GFEvent>> GetEvents()
        {
            //TODO: When going online we need to query full url below!
            //http://gfapi.timleytens.be/gf-api/events/list.json?key=XeqAsustujew6re3&secret=trezenu3uzuDrecE4upruCruq5ba4tec

            return null;
        }

        public List<GFEvent> ParseEvents(string eventsAsJson)
        {
            var gfEventCollection = JsonConvert.DeserializeObject<List<GFEvent>>(eventsAsJson);
            return gfEventCollection;
        }

        public List<GFCategory> ParseCategories(string categoriesAsJson)
        {
            var gfCategoryCollection = JsonConvert.DeserializeObject<List<GFCategory>>(categoriesAsJson);
            return gfCategoryCollection;
        }

        public List<GFDay> ParseDays(string daysAsJson)
        {
            var gfDayCollection = JsonConvert.DeserializeObject<List<GFDay>>(daysAsJson);
            return gfDayCollection;
        }
    }
}
